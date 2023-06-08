using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField] private Dropdown Resolution;
    [SerializeField] private Text nowResolution;

    private void Start()
    {

    }

    public void BackFallBack()
    {
        if (StartPanelController.Instance)
        {
            StartPanelController.Instance.gameObject.SetActive(true);
        }
        
        GetComponent<UIMoveAni>().Disable(true);
    }

    public void ScreenResolutionChange(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            default:
                break;
        }

        PlayerPrefs.SetInt("Resolution", value);
    }
}
