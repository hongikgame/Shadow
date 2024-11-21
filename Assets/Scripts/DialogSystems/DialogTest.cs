using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogTest : MonoBehaviour
{
    [SerializeField]
    private DialogSystem dialogSystem01;
    [SerializeField]
    private DialogSystem dialogSystem02;
    [SerializeField]
    private DialogSystem liveDialogSystem;
    [SerializeField]
    private DialogSystem dieDialogSystem;
    
    [SerializeField]
    private InputName playerInputName;
    [SerializeField]
    private InputName catInputName;

    [SerializeField]
    private GameObject dialogCanvas;
    [SerializeField]
    private GameObject catNamePanel;
    [SerializeField]
    private GameObject selectCanvas;

    GameObject clickObject;

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=> playerInputName.IsPlayerNameSet());

        yield return new WaitUntil(()=> dialogSystem01.UpdateDialog());

        dialogCanvas.SetActive(false);
        catNamePanel.SetActive(true);

        yield return new WaitUntil(()=> catInputName.IsCatNameSet());

        yield return new WaitUntil(()=> dialogSystem02.UpdateDialog());

        selectCanvas.SetActive(true);
        dialogCanvas.SetActive(false);

        yield return new WaitUntil(() => (clickObject = EventSystem.current.currentSelectedGameObject) != null);
        switch(clickObject.name)
        {
            case "LiveSelect":
                dialogCanvas.SetActive(true);
                yield return new WaitUntil(() => liveDialogSystem.UpdateDialog());
                break;

            case "DieSelect":
                dialogCanvas.SetActive(true);
                yield return new WaitUntil(() => dieDialogSystem.UpdateDialog());
                break;
        }
    }
}
