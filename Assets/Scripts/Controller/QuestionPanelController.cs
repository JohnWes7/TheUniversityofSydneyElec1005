using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestionPanelController : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private GameObject OptionIconPrefab;
    [SerializeField] private Transform optionContent;
    [SerializeField] private List<OptionIconController> options = new List<OptionIconController>();


    public void UpdatePanel(QuestionDetailInfo questionDetailInfo, int titleindex)
    {
        // 更改标题
        //title.text = "Question" + titleindex;
        title.DOText("Question" + titleindex, 0.3f);
        // 更改题目
        //description.text = questionDetailInfo.QuestionDesc;
        description.DOText(questionDetailInfo.QuestionDesc, 0.3f);

        // 生成选项
        // 删除之前的
        foreach (var op in options)
        {
            Destroy(op.gameObject);
        }
        options.Clear();

        //生成现在的
        int index = 0;
        foreach (var item in questionDetailInfo.Options)
        {
            // 实例化
            OptionIconController temp = Instantiate<GameObject>(OptionIconPrefab, optionContent).GetComponent<OptionIconController>();
            if (temp)
            {
                options.Add(temp);
                //选项初始化
                temp.Init(questionDetailInfo, index);
                index++;
            }
            else
            {
                Debug.LogWarning("QuestionPanelController:UpdatePanel OptionIconController temp 是null");
            }
        }

    }
}
