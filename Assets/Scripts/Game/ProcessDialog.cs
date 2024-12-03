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
                //이번 job이 마지막
                Debug.Log("등록된 Job이 없습니다. 다음 이벤트 실행");
            }
        }
    }
}
