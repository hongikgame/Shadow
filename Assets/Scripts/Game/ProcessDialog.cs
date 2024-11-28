using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessDialog : MonoBehaviour
{
    [SerializeField] private string _dataName = "Dialog";
    private void Start()
    {
        DataManager.Instance.LoadDialogFromCSV(_dataName);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int leftJobCount = DataManager.Instance.DoNextJob();
            if(leftJobCount == 0)
            {
                //�̹� job�� ������
                Debug.Log("��ϵ� Job�� �����ϴ�. ���� �̺�Ʈ ����");
            }
        }
    }
}
