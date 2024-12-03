using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UI_Select : UI_HUD
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private GameObject _buttonPrefap;

    private List<Button> _buttonList = new List<Button>();
    public DialogJobBase CurrentJob;

    public override void Active()
    {
        base.Active();

        foreach (Button button in _buttonList)
        {
            Destroy(button.gameObject);
        }
    }

    public override void Deactive()
    {
        base.Deactive();
    }

    public void SetText(string text)
    {
        _titleText.text = text;
    }

    public void CreateNewSelectButton_Dialog(string dialog, string path)
    {
        GameObject go = Instantiate(_buttonPrefap, _buttonParent);

        if(go.TryGetComponent<Button>(out Button button))
        {
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            if(text != null)
            {
                text.text = dialog;
            }

            button.onClick.AddListener(() => { OnButtonClieckEvent_Dialog(path); });
        }
    }

    public void OnButtonClieckEvent_Dialog(string path)
    {
        DataManager.Instance.LoadDialogFromCSV(path);
        if(CurrentJob != null)
        {
            CurrentJob.IsFinish = true;
            DataManager.Instance.DoNextJob();
        }    
    }
}
