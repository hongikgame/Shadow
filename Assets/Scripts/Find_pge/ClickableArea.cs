using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableArea : MonoBehaviour
{
    public GameObject circle;
    private bool isFound = false;

    void OnMouseDown()
    {
        if (isFound) return;
        isFound = true;

        if (circle != null)
        {
            circle.SetActive(true);
            Debug.Log($"{gameObject.name} found!");

            // GameManager에 상태 전달
            FindObjectOfType<FindManager>().ObjectFound();
        }
    }
}