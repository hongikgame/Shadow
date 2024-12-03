using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UI_Base : MonoBehaviour
{
    protected virtual void Start()
    {
        UIManager.Instance.RegisterUI(this);
    }

    /// <summary>
    /// UI를 활성화 합니다. 이 함수를 상속받는다면, base.Active()를 먼저 호출해야 합니다.
    /// </summary>
    public virtual void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// UI를 비활성화 합니다. 이 함수를 상속받는다면, base.Deactive()를 마지막에 호출해야 합니다.
    /// </summary>
    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }
}
