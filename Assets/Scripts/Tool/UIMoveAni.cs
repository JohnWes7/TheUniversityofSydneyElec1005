using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMoveAni : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] public bool activeStart = true;
    [SerializeField] public bool activeEnd = true;

    private void Awake()
    {
        pos = transform.position;
    }


    public void OnEnable()
    {
        if (activeStart)
        {
            transform.position = new Vector3(pos.x + Screen.width, pos.y);
            transform.DOMove(pos, 0.5f);
        }
    }

    public void Disable()
    {
        if (activeEnd)
        {
            transform.DOMove(new Vector3(pos.x - Screen.width, pos.y), 0.5f).OnComplete(() => {
                gameObject.SetActive(false);
            });
        }
    }
}
