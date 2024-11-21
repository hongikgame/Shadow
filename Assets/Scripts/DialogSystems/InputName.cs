using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputName : MonoBehaviour
{
    [SerializeField]
    private DialogSystem[] dialogSystems;

    [SerializeField]
    private TMP_InputField playerNameInputField;
    [SerializeField]
    private TMP_InputField catNameInputField;

    [SerializeField]
    private GameObject dialogCanvas;
    [SerializeField]
    private GameObject playerNamePanel;
    [SerializeField]
    private GameObject catNamePanel;

    private bool isPlayerNameSet = false;
    private bool isCatNameSet = false;


    private void Start()
    {

        dialogCanvas.SetActive(false);

    }

    public void OnPlayerNameInputComplete()
    {
        string inputName = playerNameInputField.text;

        if(!string.IsNullOrEmpty(inputName) && inputName.Length >= 2 && inputName.Length <= 8)
        {
            foreach(var dialogSystem in dialogSystems)
            {
                for (int i = 0; i < dialogSystem.dialogs.Length; i++)
                {
                    if (dialogSystem.dialogs[i].speakerIndex == 0)
                    {
                        dialogSystem.dialogs[i].name = inputName; // 이름 업데이트
                    }
                }
            }


            dialogCanvas.SetActive(true);
            playerNamePanel.SetActive(false);

            isPlayerNameSet = true;
        }
    }

    public void OnCatNameInputComplete()
    {
        string catName = catNameInputField.text;

        if (!string.IsNullOrEmpty(catName) && catName.Length >= 2 && catName.Length <= 8)
        {
            foreach (var dialogSystem in dialogSystems)
            {
                for (int i = 0; i < dialogSystem.dialogs.Length; i++)
                {
                    if (dialogSystem.dialogs[i].speakerIndex == 1)
                    {
                        dialogSystem.dialogs[i].name = catName; // 이름 업데이트
                    }
                }
            }

            catNamePanel.SetActive(false);
            dialogCanvas.SetActive(true);

            isCatNameSet = true;
        }
    }

    public bool IsPlayerNameSet()
    {
        return isPlayerNameSet;
    }

    public bool IsCatNameSet()
    {
        return isCatNameSet;
    }
}
