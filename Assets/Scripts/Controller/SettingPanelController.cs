using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    public void BackFallBack()
    {
        if (StartPanelController.Instance)
        {
            StartPanelController.Instance.gameObject.SetActive(true);
        }
        
        GetComponent<UIMoveAni>().Disable(true);
    }
}
