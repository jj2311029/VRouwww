using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject keyPanel;
    [SerializeField] private GameObject beforeMainPanel;
    public bool Pause=false;

    protected override void Awake()
    {
        base.Awake();
        settingPanel.SetActive(false);
        soundPanel.SetActive(false);
        keyPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (soundPanel.activeSelf)
            {
                CloseSoundPanel();
                SoundManager.Instance.PlaySFX(0);
            }
            else if (keyPanel.activeSelf) 
            {
                CloseKeyPanel();
                SoundManager.Instance.PlaySFX(0);
            }
            else if(beforeMainPanel.activeSelf)
            {
                CloseBeforeMainPanel();
                SoundManager.Instance.PlaySFX(0);
            }
            else
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
                Pause = !Pause;
                if (Pause)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
    
    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
        Pause = false;
        Time.timeScale = 1;
    }



    public void OpenKeyPanel()
    {
        keyPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(0);
    }
    public void CloseKeyPanel()
    {
        keyPanel.SetActive(false);
        SoundManager.Instance.PlaySFX(0);
    }

    public void OpenBeforeMainPanel()
    {
        beforeMainPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(0);
    }
    public void CloseBeforeMainPanel()
    {
        beforeMainPanel.SetActive(false);
        SoundManager.Instance.PlaySFX(0);
    }

    public void ContinueGame()
    {
        settingPanel.SetActive(false);
        SoundManager.Instance.PlaySFX(0);
        Pause = false;
        Time.timeScale = 1;
    }

    public void OpenSoundPanel()
    {
        soundPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(0);
    }
    public void CloseSoundPanel()
    {
        soundPanel.SetActive(false);
        SoundManager.Instance.PlaySFX(0);
    }
    
    
    public void MainMenu()
    {
        SoundManager.Instance.PlaySFX(0);
        CloseBeforeMainPanel();
        CloseSetting();
        SceneManager.LoadScene("LobbyScene");
    }
}
