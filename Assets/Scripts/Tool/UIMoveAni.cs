using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIMoveAni : MonoBehaviour
{
    [SerializeField] public bool activeStart = true;
    [SerializeField] public bool activeEnd = true;

    private void Awake()
    {
        
    }

    public static Vector3 GetScreenCenter()
    {
        return new Vector3(Screen.width / 2, Screen.height / 2);
    }

    public static Vector3 GetRightScreenCenter()
    {
        return new Vector3(Screen.width * 3 / 2, Screen.height / 2);
    }
    
    public static Vector3 GetLeftScreenCenter()
    {
        return new Vector3(-Screen.width / 2, Screen.height / 2);
    }


    public void OnEnable()
    {
        if (activeStart)
        {
            transform.position = GetRightScreenCenter();
            transform.DOMove(GetScreenCenter(), 0.5f);
        }
    }

    //private void Update()
    //{
    //    Debug.Log(gameObject.name + " : " + transform.position);
    //}

    public void Disable(bool destroy = false)
    {
        if (activeEnd)
        {
            if (destroy)
            {
                transform.DOMove(GetLeftScreenCenter(), 0.5f).OnComplete(() => {
                    Destroy(gameObject);
                });
            }
            else
            {
                transform.DOMove(GetLeftScreenCenter(), 0.5f).OnComplete(() => {
                    gameObject.SetActive(false);
                });
            }
        }
    }
}
