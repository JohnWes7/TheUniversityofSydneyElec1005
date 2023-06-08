using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.IO;

public class StatisticsController : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private bool isSaveRank = false;
    [SerializeField] private InputField nameInput;
    [SerializeField] private Dictionary<QuestionDetailInfo, int> history;
    [SerializeField] private Text rankText;
    [SerializeField] private Text confirm;
    [SerializeField] private Text save;


    public void UpdateHistory(Dictionary<QuestionDetailInfo, int> history)
    {
        this.history = history;
        string a = "";

        foreach (var item in history)
        {
            a += "Question" + (item.Key.Index + 1) + " : ";
            a += item.Key.QuestionDesc;
            a += "\n";

            a += "Answer : " + item.Key.Options[item.Value].Key;
            a += "\n\n";
        }

        if (a.Equals(""))
        {
            a = "No question here, Please add in " + Application.streamingAssetsPath + "\n";
        }

        a += "Socre: ";
        a += GameManager.Instance.GetSocre().ToString();

        text.text = a;
        UpdateRank();
    }

    public void SaveRank()
    {
        if (isSaveRank)
        {
            return;
        }
        isSaveRank = true;

        string playerName = nameInput.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Unknow Player";
        }
        nameInput.readOnly = true;

        string rank = PlayerPrefs.GetString("Rank");
        if (string.IsNullOrWhiteSpace(rank))
        {
            rank = "";
        }

        // 记录数据
        rank += "#" + playerName + "+" + GameManager.Instance.GetSocre().ToString();

        // 保存数据
        PlayerPrefs.SetString("Rank", rank);
        confirm.text = "Save in rank!";
        UpdateRank();
    }

    public void ExportHistoryCSV()
    {
        string playerName = nameInput.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Unknow Player";
        }

        // 判断文件夹存在
        if (!Directory.Exists(Application.streamingAssetsPath + "/Output"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Output");
        }

        playerName.Replace(" ", "_");
        //创建文件流file1
        try
        {
            FileStream file1 = new FileStream(Application.streamingAssetsPath + "/Output/" + playerName + "_output.csv", FileMode.OpenOrCreate, FileAccess.Write);

            //根据文件流file1创建写数据流wf
            StreamWriter wf = new StreamWriter(file1);
            wf.WriteLine("description,answer,score");
            foreach (var item in history)
            {
                string outline = "\"" + item.Key.QuestionDesc + "\",\"" + item.Key.Options[item.Value].Key + "\"," + item.Key.Options[item.Value].Value.ToString();
                wf.WriteLine(outline);
            }

            wf.Flush();
            wf.Close();
            file1.Close();

            Debug.Log("UpdateHistory:ExportHistoryCSV 成功写入: " + Application.streamingAssetsPath + "/Output/" + playerName + "_output.csv");
            save.text = "Save in streamingAssetsPath successfully!";
        }
        catch (Exception e)
        {
            Debug.LogWarning("UpdateHistory:ExportHistoryCSV 成功写入 csv 保存失败 " + e);
            save.text = "Save Error! " + e;
        }
        
    }


    public void BackCallback()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToStart();
        }
    }

    private void UpdateRank()
    {
        string rank = PlayerPrefs.GetString("Rank");
        if (string.IsNullOrWhiteSpace(rank))
        {
            return;
        }

        List<PlayerSocrePair> players = new List<PlayerSocrePair>();

        string[] ranklist = rank.Split("#");
        foreach (var s in ranklist)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                continue;
            }

            try
            {
                string[] detail = s.Split("+");
                string name = detail[0];
                int score = int.Parse(detail[1]);

                players.Add(new PlayerSocrePair(name, score));
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }

        }

        // 选取top10
        players.Sort();

        int count = 0;
        string toptext = "";
        foreach (var item in players)
        {
            if (count >= 10)
            {
                break;
            }

            toptext += item.name + " : " + item.score + "\n";
            count++;
        }

        // 更新显示
        rankText.text = toptext;
    }

    class PlayerSocrePair : IComparable<PlayerSocrePair>
    {
        public int score;
        public string name;

        public PlayerSocrePair(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public int CompareTo(PlayerSocrePair other)
        {
            return other.score - this.score;
        }
    }
}
