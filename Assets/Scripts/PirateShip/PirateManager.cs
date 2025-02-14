using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PirateManager : MonoBehaviour
{
    [Header("Hearts")]
    [SerializeField] public GameObject heart1;
    [SerializeField] public GameObject heart2;
    [SerializeField] public GameObject heart3;
    [SerializeField] public GameObject heart4;
    [SerializeField] public GameObject heart5;
    int heart = 5;

    [Header("ClearRate")]
    [SerializeField] float clearRate = 0f;
    [SerializeField] private float targetTime = 10f;
    private float ratePerFrame;
    private float targetRate = 100f;

    [Header("Player")]
    [SerializeField] private GameObject player;
    private SpriteRenderer playerSprite;

    [Header("SpawnManager")]
    [SerializeField] public SpawnEnemy spawnEnemy;
    [SerializeField] public SpawnObject spawnObject;

    private void Awake()
    {
        SoundManager.Instance.StopSFX();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(6);
        clearRate = 0f;
        heart = 5;
        ratePerFrame = targetRate / targetTime;
        if (spawnEnemy == null) spawnEnemy =FindObjectOfType<SpawnEnemy>();
        if (spawnObject == null) spawnObject = FindObjectOfType<SpawnObject>();
    }
    private void Start()
    {
        clearRate = 0f;
        ratePerFrame = targetRate / targetTime;

        if (player != null)
        {
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Player is Null");
        }
    }

    void FixedUpdate()
    {
        if (clearRate >= targetRate)
        {
            Debug.Log("clear");
            spawnObject.StopSpawn();
            spawnEnemy.StopSpawn();
            // SceneManager.LoadScene("BossScene");
        }
        else
        {
            clearRate += ratePerFrame * Time.deltaTime;
        }
    }

    public void ReStart()
    {
        clearRate = 0f;
        heart = 5;
        SceneManager.LoadScene("PirateShip");
    }

    public void UpHeart()
    {
        if (heart == 5) return;
        heart++;
        CheckHeart();
    }

    public void DownHeart()
    {
        heart--;
        CheckHeart();

        if (playerSprite != null)
        {
            StartCoroutine(BlinkEffect());
        }

        if (heart == 0) ReStart();
    }

    public int GetHeart()
    {
        return heart;
    }

    void CheckHeart()
    {
        heart1.SetActive(heart >= 1);
        heart2.SetActive(heart >= 2);
        heart3.SetActive(heart >= 3);
        heart4.SetActive(heart >= 4);
        heart5.SetActive(heart >= 5);
    }

    private IEnumerator BlinkEffect()
    {
        float blinkDuration = 1f;
        float blinkInterval = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        playerSprite.enabled = true;
    }
}
