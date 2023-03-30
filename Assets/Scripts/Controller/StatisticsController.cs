using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatisticsController : MonoBehaviour
{
    [SerializeField] private Text text;
    
    public void UpdateHistory(Dictionary<QuestionDetailInfo, int> history)
    {
        string a = "";
        
        foreach (var item in history)
        {
            a += "Question" + (item.Key.Index + 1) + " : ";
            a += item.Key.QuestionDesc;
            a += "\n";

            a += "Answer : " + item.Key.Options[item.Value].Key;
            a += "\n\n";
        }

        text.text = a;
    }


}
