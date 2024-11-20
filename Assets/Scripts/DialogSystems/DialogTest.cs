using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTest : MonoBehaviour
{
    [SerializeField]
    private DialogSystem dialogSystem01;
    [SerializeField]
    private InputName inputName;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => inputName.IsNameSet());

        yield return new WaitUntil(()=>dialogSystem01.UpdateDialog());
    }
}
