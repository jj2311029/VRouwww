using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class LastInterval : MonoBehaviour
{
    private Interval interval;
    private bool isStageClear = false;
    public GameObject diePanel;

    void Start()
    {
        interval = GetComponent<Interval>(); // Interval 컴포넌트 가져오기
    }

    void Update()
    {
        if (isStageClear || interval == null) return; // 중복 실행 방지

        if (interval.stageClear) // Interval에서 stageClear 확인
        {
            isStageClear = true;
            Invoke("ChangeScene", 2f); // 1초 후 씬 전환
            DiePanel panelScript = diePanel.GetComponent<DiePanel>();
            if (panelScript != null)
            {
                panelScript.Bravo6(); // 애니메이션 실행
            }
        }
    }

    void ChangeScene()
    {
        SaveLoad.savePointIndex = 9;
        SceneManager.LoadScene("SaveLoad"); // 다음 씬 로드
    }
}
