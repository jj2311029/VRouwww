using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bk_source; // 배경음악 오디오 소스
    public AudioSource ef_source; // 효과음 오디오 소스

    [Header("Audio Clips")]
    [SerializeField] public List<AudioClip> bk_music; // BGM 리스트
    [SerializeField] public List<AudioClip> ef_music; // 효과음 리스트

    [Header("Volume Controls")]
    public Slider bkSlider;
    public Slider efSlider;

    [Header("Default Settings")]
    [SerializeField] private int defaultBGMIndex = 0; // 시작할 BGM 인덱스

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 기존 AudioSource 가져오기 (필요하면 추가)
        if (bk_source == null) bk_source = gameObject.AddComponent<AudioSource>();
        if (ef_source == null) ef_source = gameObject.AddComponent<AudioSource>();

        bk_source.loop = true; // BGM 반복 재생

        // 시작할 때 자동 재생할 BGM
        if (bk_music.Count > 0 && defaultBGMIndex >= 0 && defaultBGMIndex < bk_music.Count)
        {
            PlayBGM(defaultBGMIndex);
        }
    }

    // 배경 음악 재생 함수
    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bk_music.Count)
        {
            bk_source.clip = bk_music[index];
            bk_source.Play();
        }
        else
        {
            Debug.LogError($"[SoundManager] BGM Index {index} is out of range!");
        }
    }

    // 효과음 재생 함수 (PlayOneShot 사용)
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < ef_music.Count)
        {
            ef_source.PlayOneShot(ef_music[index]);
        }
        else
        {
            Debug.LogError($"[SoundManager] SFX Index {index} is out of range!");
        }
    }
    public void StopBGM()
    {
        bk_source.Stop();  // BGM 정지
    }
    public void StopSFX()
    {
        ef_source.Stop();  // BGM 정지
    }

    // BGM 볼륨 조절 (슬라이더 연동)
    public void SetBGMVolume()
    {
        if (bkSlider != null)
        {
            bk_source.volume = bkSlider.value;
        }
    }

    // 효과음 볼륨 조절 (슬라이더 연동)
    public void SetSFXVolume()
    {
        if (efSlider != null)
        {
            ef_source.volume = efSlider.value;
        }
    }
}
