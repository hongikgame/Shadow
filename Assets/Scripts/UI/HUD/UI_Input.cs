using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Input : UI_HUD
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Button _button;
    [SerializeField] private string _initString = "입력하세요";

    public event Action<string> InputStringAction;

    public void SetText(string text)
    {
        _titleText.text = text;
        _input.text = _initString;
    }

    public void FinishInput()
    {
        InputStringAction?.Invoke(_input.text);
    }
}
