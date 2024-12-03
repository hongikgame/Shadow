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
    /// UI�� Ȱ��ȭ �մϴ�. �� �Լ��� ��ӹ޴´ٸ�, base.Active()�� ���� ȣ���ؾ� �մϴ�.
    /// </summary>
    public virtual void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// UI�� ��Ȱ��ȭ �մϴ�. �� �Լ��� ��ӹ޴´ٸ�, base.Deactive()�� �������� ȣ���ؾ� �մϴ�.
    /// </summary>
    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }
}
