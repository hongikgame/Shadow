using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] speakers; //대화 참여 캐릭터
    [SerializeField]
    private DialogData[] dialogs; //현재 분기 대사 목록
    [SerializeField]
    private bool isAutoStart = true;
    private bool isFirst = true;
    private int currentDialogIndex = -1;
    private int currentSpeakerIndex = 0;

    private float typingSpeed = 0.1f;
    private bool isTypingEffect = false;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        //모든 대화 관련 게임오브젝트 비활성화
        for(int i = 0; i < speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);
            speakers[i].spriteRenderer.gameObject.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        //대사 분기 시작될때 1회만 호출
        if(isFirst == true)
        {
            //초기화
            Setup();

            //자동 재생
            if(isAutoStart) SetNextDialog();

            isFirst = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(isTypingEffect == true)
            {
                isTypingEffect = false;

                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                return false;
            }

            //대사가 남았을 경우 다음 대사 진행
            if(dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            //대사가 더 이상 없을 경우 
            else
            {
                for(int i = 0; i < speakers.Length; ++i)
                {
                    SetActiveObjects(speakers[i], false);
                    speakers[i].spriteRenderer.gameObject.SetActive(false);
                }

                return true;
            }
        }

        return false;
    }

    private void SetNextDialog()
    {
        SetActiveObjects(speakers[currentSpeakerIndex], false);
        currentDialogIndex++;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        SetActiveObjects(speakers[currentSpeakerIndex], true);
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        //speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(Speaker speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        speaker.objectArrow.SetActive(false);

        Color color = speaker.spriteRenderer.color;
        color.a = visible == true ? 1 : 0f;
        speaker.spriteRenderer.color = color;
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        while (index < dialogs[currentDialogIndex].dialogue.Length)
        {
            speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingEffect = false;
        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
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
