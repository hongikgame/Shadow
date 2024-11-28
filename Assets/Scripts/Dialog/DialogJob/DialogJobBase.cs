using System;
using System.Collections.Generic;


public abstract class DialogJobBase
{
    public string Narrator;
    public string Dialog;
    public bool IsFinish = false;

    public DialogJobBase(string narrator, string dialog)
    {
        this.Narrator = narrator;
        this.Dialog = dialog;
    }

    public abstract void Excute();
    public abstract void AfterFinish();
}


public class DialogJob : DialogJobBase
{
    public DialogJob(string narrator, string dialog) : base(narrator, dialog)
    {
        this.Narrator = narrator;
        this.Dialog = dialog;
    }

    public override void Excute()
    {
        UIManager.Instance.ChangeHUDUI<UI_Dialog>();
        UI_Dialog ui_Dialog = UIManager.Instance.GetUICopmonent<UI_Dialog>() as UI_Dialog;
        if (ui_Dialog == null) return;

        ui_Dialog.CurrentDialogJob = this;

        ui_Dialog.SetText(Dialog, Narrator);
    }

    public override void AfterFinish()
    {
        
    }
}


public abstract class InputJob : DialogJobBase
{
    public UI_Input _uiInput;

    public InputJob(string narrator, string dialog) : base(narrator, dialog)
    {
        this.Narrator = narrator;
        this.Dialog = dialog;

    }
    public override void Excute()
    {
        UIManager.Instance.ChangeHUDUI<UI_Input>();
        this._uiInput = UIManager.Instance.GetUICopmonent<UI_Input>() as UI_Input;

        if (_uiInput == null) return;

        _uiInput.SetText(Dialog);
        _uiInput.InputStringAction += FinishInput;
    }
    public override void AfterFinish()
    {
        this._uiInput.InputStringAction -= FinishInput;
    }

    public abstract void FinishInput(string inputString);
}


public class SelectJob : DialogJobBase
{
    public UI_Select _uiChoose;
    public SelectJob(string narrator, string dialog) : base(narrator, dialog)
    {
        this.Narrator = narrator;
        this.Dialog = dialog;
    }

    public override void Excute()
    {
        UIManager.Instance.ChangeHUDUI<UI_Select>();
        this._uiChoose = UIManager.Instance.GetUICopmonent<UI_Select>() as UI_Select;

        string[] buttonTextArray = SplitString(Dialog);
        string[] nextDialogPathArray = SplitString(Narrator);

        if (buttonTextArray.Length == 0 || nextDialogPathArray.Length == 0)
        {
            _uiChoose.CreateNewSelectButton_Dialog("Error", "Error");
            return;
        }

        if (buttonTextArray.Length != nextDialogPathArray.Length + 1)
        {
            _uiChoose.CreateNewSelectButton_Dialog("Error", "Error");
            return;
        }

        _uiChoose.CurrentJob = this;
        _uiChoose.SetText(buttonTextArray[0]);
        for(int i = 1; i < buttonTextArray.Length; i++)
        {
            int index = i;
            string textString = buttonTextArray[index];
            string dialogPath = nextDialogPathArray[index - 1];
            _uiChoose.CreateNewSelectButton_Dialog(textString, dialogPath);
        }
    }

    public override void AfterFinish()
    {
        
    }

    private static string[] SplitString(string dialog)
    {
        if(string.IsNullOrEmpty(dialog)) return new string[0];

        return dialog.Split("|");
    }
}


public class InputJob_NarratorName : InputJob
{
    public InputJob_NarratorName(string narrator, string dialog) : base(narrator, dialog)
    {
        this.Narrator = narrator;
        this.Dialog = dialog;
    }

    public override void FinishInput(string inputString)
    {
        DataManager.Instance.RegisterNarratorName(Narrator, inputString);
        IsFinish = true;
        DataManager.Instance.DoNextJob();
    }
}



public class InputJob_LLM : InputJob
{
    public InputJob_LLM(string narrator, string dialog) : base(narrator, dialog)
    {
        this.Narrator = narrator;
        this.Dialog = dialog;

        EventHandler.AfterRecvLLMData += RecvData;
    }

    public override void Excute()
    {
        //TEST
        EventHandler.CallAfterRecvLLMData(Dialog);
    }

    public override void FinishInput(string inputString)
    {
        //LLM에게 요청 전송
    }

    public void RecvData(string recv)
    {
        //LLM에게 답변 받은 후
        DialogJobBase newJob = new DialogJob(Narrator, recv);

        DataManager.Instance.RegeisterDialogJob(newJob);
        IsFinish = true;

        DataManager.Instance.DoNextJob();
    }

    public override void AfterFinish()
    {

    }
}
