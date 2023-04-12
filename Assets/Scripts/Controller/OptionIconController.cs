using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class OptionIconController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text desc;
    [SerializeField] private string text;
    [SerializeField] private int score;

    public void Init(QuestionDetailInfo questionDetailInfo, int optionindex)
    {
        this.text = questionDetailInfo.Options[optionindex].Key;
        this.score = questionDetailInfo.Options[optionindex].Value;

        if (desc)
        {
            //desc.text = text;
            desc.DOText(text, 0.3f);
        }

        if (button)
        {
            button.onClick.AddListener(() =>
            {
                GameManager.Instance.OptionButtonCallBack(questionDetailInfo, optionindex);
            });
        }
    }


}
