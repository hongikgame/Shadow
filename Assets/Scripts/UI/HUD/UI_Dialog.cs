using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_Dialog : UI_HUD
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dialogText;
    [SerializeField] private GameObject _blinkIconObject;
    [SerializeField] private DataManager _dialog;

    [SerializeField] private float _charPerSecond = 0.05f;

    private Coroutine _blinkCoroutine;
    private bool _isFinish = true;

    public DialogJob CurrentDialogJob;

    public override void Active()
    {
        base.Active();
    }

    public void SetText(string dialog, string id)
    {
        if( DataManager.Instance.ReplaceDict.TryGetValue(id, out string narratorName))
        {
            _nameText.text = narratorName;
        }
        _isFinish = false;

        StartCoroutine(DoText(dialog));
    }

    private IEnumerator DoText(string text)
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
        }
        _blinkIconObject.SetActive(false);


        foreach(var pair in DataManager.Instance.ReplaceDict)
        {
            text = text.Replace($"{{{pair.Key}}}", pair.Value);
        }

        StringBuilder sb = new StringBuilder();
        WaitForSeconds wfs = new WaitForSeconds(_charPerSecond);

        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            _dialogText.text = sb.ToString();
            yield return wfs;
        }

        _isFinish = true;
        if(CurrentDialogJob != null)
        {
            CurrentDialogJob.IsFinish = true;
        }

        _blinkCoroutine = StartCoroutine(DoTextEndBlink());
    }

    private IEnumerator DoTextEndBlink()
    {
        WaitForSecondsRealtime wfs = new WaitForSecondsRealtime(0.5f);
        while (true)
        {
            _blinkIconObject.SetActive(!_blinkIconObject.activeSelf);
            yield return wfs;
        }
    }
}
