using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindManager : MonoBehaviour
{
    private int foundObjects = 0;       // 찾은 물건 개수
    public int totalObjects = 4;       // 총 숨은 물건 개수
    public Animator animator;          // 애니메이션 컨트롤러

    public void ObjectFound()
    {
        foundObjects++;
        Debug.Log($"Objects found: {foundObjects}/{totalObjects}");

        if (foundObjects >= totalObjects)
        {
            Debug.Log("All objects found! Starting animation...");
            StartAnimation();
        }
    }

    private void StartAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("ChangeImage"); // 애니메이션 트리거 실행
        }
        else
        {
            Debug.LogError("Animator is not assigned!");
        }
    }
}