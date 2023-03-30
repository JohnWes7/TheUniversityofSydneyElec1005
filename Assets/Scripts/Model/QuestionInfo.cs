using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class QuestionInfo
{
    private static QuestionInfo instance;

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
        // 读取数据
        //TODO: 更改读取方式到文件夹查找
        TextAsset datastring = Resources.Load<TextAsset>(PATH.QuestionDataPath);
        Debug.Log(datastring.text);

        // 解析数据
        questionDetailInfos = new List<QuestionDetailInfo>();
        string[] lines = datastring.text.Split("\n");

        bool firstline = true;
        int index = 0;
        foreach (var item in lines)
        {
            if (firstline)
            {
                firstline = false;
                continue;
            }

            if (item == "")
            {
                continue;
            }

            QuestionDetailInfo temp = CsvLine2QuestionDetailInfo(item, index++);
            if (temp != null)
            {
                Debug.Log(temp);
                questionDetailInfos.Add(temp);
            }
            else
            {
                Debug.LogWarning("QuestionInfo : QuestionInfo" + "QuestionDetailInfo生成为空");
            }
        }
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
        if (line == null)
        {
            return null;
        }

        // 正则解析csv
        var MatchValues = Regex.Matches(line, "(?<=^|,)[^\"]*?(?=,|$)|(?<=^|,\")(?:(\"\")?[^\"]*?)*(?=\",?|$)", RegexOptions.ExplicitCapture);
        string a = "";
        // 解析出来的值存入 values
        List<string> values = new List<string>();

        foreach (var detail in MatchValues)
        {
            a += detail.ToString() + "|\n";
            values.Add(detail.ToString());
        }
        //Debug.Log(a);

        // 解析出选项和对应分值
        string desc = values[0];
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

        return Index + " " + "问题个数: " + Options.Count + "\n desc: " + QuestionDesc + "\noptions:\n" + op;
    }
}