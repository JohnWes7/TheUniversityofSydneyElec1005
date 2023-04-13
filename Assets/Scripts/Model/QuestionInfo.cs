using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text;

/// <summary>
/// 数据类
/// </summary>
public class QuestionInfo
{
    // 单例属性
    private static QuestionInfo instance;
    
    // 
    public static QuestionInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestionInfo();
            }

            return instance;
        }
    }

    private List<QuestionDetailInfo> questionDetailInfos = new List<QuestionDetailInfo>();

    private QuestionInfo()
    {
        questionDetailInfos = new List<QuestionDetailInfo>();

        // 读取数据
        // TODO: 更改读取方式到文件夹查找
        //TextAsset datastring = Resources.Load<TextAsset>(PATH.QuestionDataPath);
        //Debug.Log(datastring.text);
        //Debug.Log(Application.streamingAssetsPath);

        //Debug.Log(CsvLine2QuestionDetailInfo("\"Function names are optional in function definitions.If it is omitted, Python will randomly generate a function name!\",Ture,0,False,5,,,,,", 0));
        //Debug.Log(CsvLine2QuestionDetailInfo("ax^2+bx+c,root1,1,root2,1,\"roo1, root2\",6,,,", 0));

        //string str = "ax^2+bx+c,root1,1,root2,1,\"roo1, root2\",6,,,";
        //string str1 = "\"Function names are optional in function definitions.If it is omitted, Python will randomly generate a function name!\",Ture,0,False,5,,,,,";
        //string[] split = Regex.Split(str1, @",(?=(?:[^""\\]*""[^""\\]*"")*[^""\\]*$)");//将每行内容中的特殊字符进行转换替换。
        //for (int i = 0; i < split.Length; i++)
        //{
        //    if (split[i].Length >= 2)
        //    {
        //        if (split[i].IndexOf("\"") == 0) split[i] = split[i].Substring(1, split[i].Length - 1);
        //        if (split[i].LastIndexOf("\"") == (split[i].Length - 1)) split[i] = split[i].Substring(0, split[i].Length - 1);
        //    }
        //    split[i].Replace("\"\"", "\"");
        //}

        // 获取文件夹下的所有csv中的所有字符串
        List<string> allcsv = GetAllCSV(Application.streamingAssetsPath);

        // 解析数据
        foreach (var item in allcsv)
        {
            Debug.Log("QuestionInfo : QuestionInfo : csv文件内容" + item);
            AddCSVData(item);
        }
    }

    private void AddCSVData(string oneCSVText)
    {
        string[] lines = oneCSVText.Split("\n");

        bool firstline = true;
        int line_index = 0;
        foreach (var item in lines)
        {
            line_index++;

            if (firstline)
            {
                firstline = false;
                continue;
            }

            if (item == "")
            {
                Debug.LogWarning("该文件 " + line_index.ToString() + "行 为\"\"");
                continue;
            }

            if (item == null)
            {
                Debug.LogWarning("该文件 " + line_index.ToString() + "行 为null");
                continue;
            }

            QuestionDetailInfo temp = CsvLine2QuestionDetailInfo(item, questionDetailInfos.Count);
            if (temp != null)
            {
                //Debug.Log(temp);
                questionDetailInfos.Add(temp);
            }
            else
            {
                Debug.LogWarning("QuestionInfo : QuestionInfo" + "QuestionDetailInfo生成为空\n第" + line_index.ToString() + "行\n" + item);
            }
        }
    }

    private List<string> GetAllCSV(string directoryPath)
    {
        // 打开文件夹
        DirectoryInfo datadire = new DirectoryInfo(directoryPath);
        // 获取所有csv文件
        FileInfo[] fileInfos = datadire.GetFiles("*.csv");
        Debug.Log("QuestionInfo : GetAllCSV 一共找到的csv文件数: " + fileInfos.Length);

        List<string> ans = new List<string>();

        // 遍历所有csv文件
        foreach (var item in fileInfos)
        {
            //读取文件
            FileStream fs = item.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string csvcontent = sr.ReadToEnd();

            //加入到返回值
            ans.Add(csvcontent);
        }

        return ans;
    }

    public QuestionDetailInfo GetQuestion(int index)
    {
        if (index >= questionDetailInfos.Count)
        {
            return null;
        }

        return questionDetailInfos[index];
    }

    private QuestionDetailInfo CsvLine2QuestionDetailInfo(string line, int index)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;
        }

        // 正则解析csv
        //var MatchValues = Regex.Matches(line, "(?<=^|,)[^\"]*?(?=,|$)|(?<=^|,\")(?:(\"\")?[^\"]*?)*(?=\",?|$)", RegexOptions.ExplicitCapture);
        //var MatchValues = Regex.Matches(line, "[\\t,](?= (?:[^\"]|\"[^\"]*\")*$)", RegexOptions.ExplicitCapture);        
        //var MatchValues = Regex.Matches(line, "("".*?""|"[^"]*")", RegexOptions.ExplicitCapture);

        string[] split = Regex.Split(line, @",(?=(?:[^""\\]*""[^""\\]*"")*[^""\\]*$)");//将每行内容中的特殊字符进行转换替换。
        for (int i = 0; i < split.Length; i++)
        {
            if (split[i].Length >= 2)
            {
                if (split[i].IndexOf("\"") == 0) split[i] = split[i].Substring(1, split[i].Length - 1);
                if (split[i].LastIndexOf("\"") == (split[i].Length - 1)) split[i] = split[i].Substring(0, split[i].Length - 1);
            }
            split[i].Replace("\"\"", "\"");
        }

        string a = "";
        // 解析出来的值存入 values
        List<string> values = new List<string>();

        foreach (var detail in split)
        {
            a += detail.ToString() + "|\n";
            values.Add(detail.ToString());
        }
        //Debug.Log(a);

        // 解析出选项和对应分值
        string desc = values[0];
        if (string.IsNullOrEmpty(desc))
        {
            return null;
        }
        List<KeyValuePair<string, int>> option = new List<KeyValuePair<string, int>>();

        for (int i = 1; i < values.Count; i += 2)
        {
            if (!string.IsNullOrWhiteSpace(values[i]))
            {

                // option
                string opdesc = values[i];
                int score = -1;
                try
                {
                    int optionscore;
                    if (int.TryParse(values[i + 1], out optionscore))
                    {
                        score = optionscore;
                    }
                    else
                    {
                        Debug.LogWarning("QuestionInfo : QuestionInfo 解析分数失败, string: " + values[i + 1]);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("QuestionInfo : QuestionInfo 解析分数失败 " + e.Message);
                }

                option.Add(new KeyValuePair<string, int>(opdesc, score));
            }
        }

        return new QuestionDetailInfo(index, desc, option);
    }
}

public class QuestionDetailInfo
{
    public int Index { get; }
    public string QuestionDesc { get; }
    public List<KeyValuePair<string, int>> Options { get; }
   
    public QuestionDetailInfo(int index, string questionDesc, List<KeyValuePair<string, int>> options)
    {
        QuestionDesc = questionDesc;
        Options = options;
        Index = index;
    }

    public override string ToString()
    {
        string op = "";
        foreach (var item in Options)
        {
            op += item.Key + " : " + item.Value + "\n";
        }

        return Index + " " + "选项个数: " + Options.Count + "\n desc: " + QuestionDesc + "\noptions:\n" + op;
    }
}