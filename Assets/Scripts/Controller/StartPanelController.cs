using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelController : MonoBehaviour
{
    // 单例
    private static StartPanelController instance;

    public static StartPanelController Instance { get => instance; }

    [SerializeField] private GameObject GameManagerPrefab;
    [SerializeField] private GameObject SettingPanelPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void StartCallback()
    {
        // 生成开始控制器 代表开始
        Instantiate<GameObject>(GameManagerPrefab);
        // 关闭自己
        this.GetComponent<UIMoveAni>().Disable();
    }

    public void SettingCallback()
    {
        Instantiate<GameObject>(SettingPanelPrefab, GameObject.Find("Canvas").transform.Find("Panel"));
        GetComponent<UIMoveAni>().Disable();
    }

    public void Quit()
    {
        // 退出
        Application.Quit();
    }
}
