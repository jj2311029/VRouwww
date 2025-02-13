using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject arrowMark;

    private void Start()
    {
        SoundManager.Instance.StopSFX();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX(4);

        arrowMark.SetActive(false);
    }

    public void Redo()
    {
        SaveLoad.currentSelectedSlot = SavePoint.diePoint - 1;
        SceneManager.LoadScene("1LevelDesign");
    }

    public void ArrowMark()
    {
        arrowMark.SetActive(true);
    }
    public void NoArrowMark()
    {
        arrowMark.SetActive(false);
    }
}
