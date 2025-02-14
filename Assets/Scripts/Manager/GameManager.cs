using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("1LevelDesign");
        SoundManager.Instance.PlaySFX(0);
    }

    public void SaveLoad()
    {
        SceneManager.LoadScene("SaveLoad");
        SoundManager.Instance.PlaySFX(0);
    }
    public void FinishGame()
    {
        SoundManager.Instance.PlaySFX(0);
        Application.Quit();
    }
}