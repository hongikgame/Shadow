using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputName : MonoBehaviour
{
    [SerializeField]
    private DialogSystem dialogSystem;
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private GameObject dialogCanvas;
    [SerializeField]
    private GameObject playerNamePanel;

    [SerializeField] 
    private Button inputButton;

    private bool isNameSet = false;


    private void Start()
    {

        dialogCanvas.SetActive(false);

    }

    public void OnInputComplete()
    {
        string inputName = nameInputField.text;

        if(!string.IsNullOrEmpty(inputName) && inputName.Length >= 2 && inputName.Length <= 8)
        {
            for(int i = 0; i < dialogSystem.dialogs.Length; i++)
            {
                if(dialogSystem.dialogs[i].speakerIndex == 0)
                {
                    dialogSystem.dialogs[i].name = inputName;
                }
            }

            dialogCanvas.SetActive(true);
            playerNamePanel.SetActive(false);

            isNameSet = true;
        }
    }

    public bool IsNameSet()
    {
        return isNameSet;
    }
}
