using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;
    [SerializeField] Image img_Character; // 추가: 캐릭터 이미지 UI

    Dialogue[] dialogues;

    bool isDialogue = false;
    bool isNext = false;

    int lineCount = 0;
    int contextCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDialogue)
            {
                Dialogue testDialogue = new Dialogue();
                testDialogue.name = "NPC";
                testDialogue.contexts = new string[]
                {
                    "조작법: \n방향키\n검 공격: Z\n총 공격: X\n상호작용: Space",
                    
                };
                testDialogue.characterSprite = null; // 기본값 (이미지 없음)

                ShowDialogue(new Dialogue[] { testDialogue });
            }
            else if (isNext)
            {
                isNext = false;
                txt_Dialogue.text = "";
                if (++contextCount < dialogues[lineCount].contexts.Length)
                {
                    StartCoroutine(TypeWriter());
                }
                else
                {
                    contextCount = 0;
                    if (++lineCount < dialogues.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        EndDialogue();
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;
        dialogues = p_dialogues;
        txt_Dialogue.text = "";
        txt_Name.text = dialogues[0].name; // 첫 번째 대화의 이름 표시

        // 캐릭터 이미지 표시
        if (dialogues[0].characterSprite != null)
        {
            img_Character.sprite = dialogues[0].characterSprite;
            img_Character.gameObject.SetActive(true);
        }
        else
        {
            img_Character.gameObject.SetActive(false);
        }

        SettingUI(true); // UI와 텍스트 + 이미지 활성화
        StartCoroutine(TypeWriter());
    }

    void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;

        SettingUI(false); // UI와 텍스트 + 이미지 비활성화
    }

    IEnumerator TypeWriter()
    {
        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("`", ",");

        txt_Dialogue.text = "";

        foreach (char c in t_ReplaceText)
        {
            txt_Dialogue.text += c;
            yield return new WaitForSeconds(0.003f); // 타이핑 효과
        }

        isNext = true;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
        txt_Dialogue.gameObject.SetActive(p_flag);
        txt_Name.gameObject.SetActive(p_flag);
        img_Character.gameObject.SetActive(p_flag); // 추가: 이미지도 같이 나타나고 사라지게 설정
    }
}
