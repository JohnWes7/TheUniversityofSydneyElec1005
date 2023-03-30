using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    // 单例
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    // 统计
    [SerializeField] private Dictionary<QuestionDetailInfo, int> history = new Dictionary<QuestionDetailInfo, int>();

    // 组件
    [SerializeField] private GameObject questionPanelPrefab;
    [SerializeField] private QuestionPanelController questionPanelController;
    [SerializeField] private GameObject statisticsPanelPrefab;
    [SerializeField] private StatisticsController statisticsPanel;

    // 控制台 Debug
    [SerializeField] private Text debugtext;
    [SerializeReference] private QuestionIterator questionIterator;

    // 问题迭代器
    [Serializable]
    public class QuestionIterator
    {
        [SerializeField] private int questionNum = 0;

        public bool NextQuestion(out QuestionDetailInfo queinfo, out int index)
        {
            queinfo = QuestionInfo.Instance.GetQuestion(questionNum);
            
            questionNum++;
            if (queinfo != null)
            {
                index = queinfo.Index;
                return true;
            }

            index = -1;
            return false;
        }
    }


    private void Awake()
    {
        // 单例
        instance = this;
        // 最一开始生成一个问题面板
        questionPanelController = Instantiate<GameObject>(questionPanelPrefab, GameObject.Find("Canvas").transform.Find("Panel")).GetComponent<QuestionPanelController>();
        questionIterator = new QuestionIterator();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始第一个问题
        NextQuestAndUpdatePanelOrDone();
    }

    private void Update()
    {
        GMDebug();
    }

    /// <summary>
    /// 选项调用回调函数
    /// </summary>
    /// <param name="info"></param>
    /// <param name="optionindex"></param>
    public void OptionButtonCallBack(QuestionDetailInfo info, int optionindex)
    {
        history.Add(info, optionindex);
        NextQuestAndUpdatePanelOrDone();
    }

    /// <summary>
    /// 执行下一问题 更新问题面板 并判断是否问题结束
    /// </summary>
    private void NextQuestAndUpdatePanelOrDone()
    {
        QuestionDetailInfo queinfo;
        int index;
        if (questionIterator.NextQuestion(out queinfo, out index))
        {
            questionPanelController.UpdatePanel(queinfo, index + 1);
        }
        else
        {
            //没有问题 直接结束
            Done();
        }
    }

    public void Done()
    {
        Debug.Log("done");

        // 关闭问题面板
        CloseUI(questionPanelController.gameObject);

        // 开启统计面板
        statisticsPanel = Instantiate<GameObject>(statisticsPanelPrefab, GameObject.Find("Canvas").transform.Find("Panel")).GetComponent<StatisticsController>();
        statisticsPanel.UpdateHistory(history);
    }

    private void CloseUI(GameObject ui)
    {
        UIMoveAni uIMoveAni;
        if (ui.TryGetComponent<UIMoveAni>(out uIMoveAni))
        {
            uIMoveAni.Disable();
        }
        else
        {
            ui.SetActive(false);
        }
    }

    private void GMDebug()
    {
        if (debugtext)
        {
            debugtext.text = "Debug:\n" + "cur score : " + GetSocre();
        }
    }

    public int GetSocre()
    {
        int sum = 0;
        foreach (var item in history)
        {
            sum += item.Key.Options[item.Value].Value;
        }
        return sum;
    }
}
