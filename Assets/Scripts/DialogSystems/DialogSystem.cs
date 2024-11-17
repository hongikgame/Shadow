using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] speakers; //��ȭ ���� ĳ����
    [SerializeField]
    private DialogData[] dialogs; //���� �б� ��� ���
    [SerializeField]
    private bool isAutoStart = true;
    private bool isFirst = true;
    private int currentDialogIndex = -1;
    private int currentSpeakerIndex = 0;

    private void Awake()
    {
        
    }

    private void Setup()
    {
        //��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
        for(int i = 0; i < speakers.Length; ++i)
        {
            //SetActiveObjects(speakers[i], false)
            speakers[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        //��� �б� ���۵ɶ� 1ȸ�� ȣ��
        if(isFirst == true)
        {
            //�ʱ�ȭ
            Setup();

            //�ڵ� ���
            //if(isAutoStart) SetNextDialog();

            isFirst = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            //��簡 ������ ��� ���� ��� ����
            if(dialogs.Length > currentDialogIndex + 1)
            {
                //SetNextDialog();
            }
            //��簡 �� �̻� ���� ��� 
            else
            {
                for(int i = 0; i < speakers.Length; ++i)
                {
                    //SetActiveObjects(speakers[i], false);
                    speakers[i].spriteRenderer.gameObject.SetActive(false);
                }

                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        //SetActiveObject(speakers[currentSpeakerIndex], false);
        currentDialogIndex++;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
    }

    [System.Serializable]
    public struct Speaker
    {
        public SpriteRenderer spriteRenderer;
        public Image imageDialog;
        public TextMeshProUGUI textName;
        public TextMeshProUGUI textDialogue;
        public GameObject objectArrow;
    }

    [System.Serializable]
    public struct DialogData
    {
        public int speakerIndex;
        public string name;
        [TextArea(3, 5)]
        public string dialogue;
    }
}
