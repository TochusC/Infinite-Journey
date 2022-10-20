using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioClip menuBGM;
    [SerializeField] AudioClip battleBGM;
    [SerializeField] AudioClip bossBGM;
    [SerializeField] AudioClip changePlayerSFX;
    [SerializeField] AudioClip lastTimeBGM;
    [SerializeField] AudioClip finalStageBGM;
    [SerializeField] AudioClip gameStartSFX;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject newInstance;
    [SerializeField] GameObject enemyRotation;
    [SerializeField] GameObject newPlayerEffect;
    public bool finalStage = false;
    public bool lastTime = false;
    bool isplaying = false;
    [SerializeField] public bool isGameStarted = false;
    [SerializeField] public bool isPlayerDead = false;
    [SerializeField] public bool isGenerating = false;
    [SerializeField] public bool isIntialized = false;
    [SerializeField] public float rightBoundary = 5.25f;
    [SerializeField] protected float cameraSpeed = 10f;
    [SerializeField] protected float cameraDistance = 0f;
    [SerializeField] public float upBoundary = 3f;
    [SerializeField] GameObject MenuWindow;
    [SerializeField] public GameObject NewPlayer;
    [SerializeField] GameObject DeathMenu;
    [SerializeField] GameObject UserInterface;
    [SerializeField] GameObject waveMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endMenu;
    [SerializeField] GameObject lostMenu;
    [SerializeField] GameObject[] EnemyPrefabs;
    [SerializeField] GameObject[] elliteEnemyPrefabs;
    [SerializeField] GameObject[] bossPrefabs;
    [SerializeField] GameObject finalBoss;
    [SerializeField] GameObject defaultPlayer;
    bool boosPhase = false;
    public bool isGamePaused = false;
    GameObject InitialPlayer;
    Rigidbody2D PlayerRb;
    AudioSource audioSorce;
    [SerializeField] int waves = 0;
    [SerializeField] int enemyNumbers;
    [SerializeField] public Unit PlayerUnit;
    [SerializeField] Text healthText;
    [SerializeField] Text boostText;
    [SerializeField] Text wavesText;
    // Start is called before the first frame update
    void Start()
    {
        audioSorce = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        CemeraMove();
        if (isGameStarted)
        {
            if (!isplaying || boosPhase)
            {
                if (boosPhase && isplaying)
                {
                    ChangeCameraDistance(-10.5f, 5);
                    audioSorce.clip = bossBGM;
                    audioSorce.Play();
                    isplaying = false;
                }
                else if (!isplaying && !boosPhase)
                {
                    if(PlayerUnit!=null && !PlayerUnit.isBoss)
                        ChangeCameraDistance(-6.75f, 5);
                    audioSorce.clip = battleBGM;
                    audioSorce.Play();
                    isplaying = true;
                }
            }
            if (lastTime && !finalStage)
            {
                lastTime = false;
                audioSorce.clip = lastTimeBGM;
                audioSorce.Play();
            }
            if (finalStage && !lastTime)
            {
                lastTime = true;
                audioSorce.clip = finalStageBGM;
                audioSorce.Play();
            }
            if (isPlayerDead == false)
            {
                if (isIntialized)
                {
                    PlayerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
                }
                else
                {
                    SetCameraDistance(-6.75f, 10);
                    UserInterface.SetActive(true);
                    MenuWindow.SetActive(false);
                    SpawnPlayer();
                    isIntialized = true;
                }
                boostText.text = "Boost:" + PlayerUnit.playerBoost.ToString("0.00");
                healthText.text = "Health:" + (int)(PlayerUnit.health) + "/" + ((int)(PlayerUnit.maxHealth));
                enemyNumbers = GameObject.FindGameObjectsWithTag("Enermy").Length;
                if ((enemyNumbers == 0 && isGenerating == false && waves != 10) || waves == 11)
                {
                    waves++;
                    StartCoroutine(GenerateEnemyWave(waves));
                }
            }
            else
            {
                if(NewPlayer.GetComponent<Unit>().lost)
                {
                    ChangeCameraDistance(-30f, 6.25f);
                    lostMenu.SetActive(true);
                }
                else
                {
                    UserInterface.SetActive(false);
                    DeathMenu.SetActive(true);
                    if (!boosPhase)
                        ChangeCameraDistance(-10.5f, 6.25f);
                }
            }
        }
        if(waves == 10 && isGenerating == false)
        {
            if (GameObject.FindGameObjectsWithTag("Enermy").Length == 0 && GameObject.FindGameObjectsWithTag("FinalBoss").Length != 0)
            {
                if(newInstance.GetComponent<Unit>().health <= newInstance.GetComponent<Unit>().setmaxHealth * 0.60)
                {
                    finalStage = true;
                    for(int cnt = 0; cnt < Random.Range(16, 28); cnt++)
                        Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], GetRandomSpawnPos() + new Vector3(0, 0.5f, 0), enemyRotation.transform.rotation);
                    for (int cnt = 0; cnt < Random.Range(2,5); cnt++)
                        Instantiate(elliteEnemyPrefabs[Random.Range(0, elliteEnemyPrefabs.Length)], GetRandomSpawnPos() + new Vector3(0, 0.5f, 0), enemyRotation.transform.rotation);
                    for (int cnt = 0; cnt < Random.Range(0,3); cnt++)
                            Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation);
                }
            }
            if (finalStage && GameObject.FindGameObjectsWithTag("FinalBoss").Length == 0)
                waves++;
        }
    }
    IEnumerator GenerateEnemyWave(int waves)
    {
        boosPhase = false;
        isGenerating = true;
        if (waves >= 10)
        {
            if(waves == 10)
            {
                waveMenu.SetActive(true);
                wavesText.text = "Final Waves";
                yield return new WaitForSeconds(2);
                waveMenu.SetActive(false);
                audioSorce.PlayOneShot(gameStartSFX, 0.6f);
                yield return new WaitForSeconds(3);
                newInstance = Instantiate(finalBoss, GetRandomSpawnPos(), enemyRotation.transform.rotation);
                boosPhase = true;
            }
            else
            {
                Destroy(GameObject.FindGameObjectWithTag("Player"));
                endMenu.SetActive(true);
            }
        }
        else
        {
            waveMenu.SetActive(true);
            wavesText.text = "Waves:" + waves;
            yield return new WaitForSeconds(2);
            waveMenu.SetActive(false);
            audioSorce.PlayOneShot(gameStartSFX, 0.6f);
            yield return new WaitForSeconds(3);
            for (int cnt = 0; cnt <= waves / 2; cnt++)
            {
                if(waves == 7 && cnt == waves / 2)
                {
                    for (int tcnt = 0; tcnt < 2 + waves / 2; tcnt++)
                    {
                        newInstance = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation);
                    }
                    if (waves == 7 && cnt == waves / 2)
                    {
                        Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation);
                        boosPhase = true;
                    }

                }
                else
                {
                    for (int tcnt = 0; tcnt < 2 + waves / 2; tcnt++)
                    {
                        newInstance = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation);
                    }
                    if (waves >= 3)
                    {
                        for (int tcnt = 0; tcnt <= waves / 4; tcnt++)
                        {
                            newInstance = Instantiate(elliteEnemyPrefabs[Random.Range(0, elliteEnemyPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation); ;
                        }
                    }
                }
                while (GameObject.FindGameObjectsWithTag("Enermy").Length != 0)
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
        /*for (int cnt = 0; cnt < waves%5 /2; cnt++)
        {
            newInstance = Instantiate(elliteEnemyPrefabs[Random.Range(0, elliteEnemyPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation);
        }
        for (int cnt = 0; cnt < waves%8 / 4; cnt++)
        {
            boosPhase = true;
            newInstance = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], GetRandomSpawnPos(), enemyRotation.transform.rotation);
        }*/
        isGenerating = false;
        waveMenu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }
    void SpawnPlayer()
    {
        InitialPlayer = Instantiate(defaultPlayer, new Vector3(0, 0, 0), EnemyPrefabs[0].transform.rotation);
        PlayerUnit = InitialPlayer.GetComponent<Unit>();
        PlayerUnit.isPlayer = true;
        PlayerUnit.tag = "Player";
        PlayerRb = InitialPlayer.GetComponent<Rigidbody2D>();
    }
    public void ChangePlayer()
    {
        if(!boosPhase)
            ChangeCameraDistance(-6.75f, 10);
        audioSorce.PlayOneShot(changePlayerSFX, 0.4f);
        if(NewPlayer == null)
        {
            SpawnPlayer();
            return;
        }
        PlayerUnit.isPlayer = true;
        Instantiate(newPlayerEffect, NewPlayer.transform.position, newPlayerEffect.transform.rotation);
        NewPlayer.tag = "Player";
        DeathMenu.SetActive(false);
        UserInterface.SetActive(true);
        isPlayerDead = false;
    }
    Vector3 GetRandomSpawnPos()
    {
        return new Vector3(Random.Range(-rightBoundary, rightBoundary), upBoundary + 2f, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    void CemeraMove()
    {
        Vector3 targetPosition = new Vector3(0, 0, cameraDistance);
        if(Vector3.Distance(mainCamera.transform.position, targetPosition) >= 0.1f)
        {
            Vector3 moveDirection = (targetPosition - mainCamera.transform.position).normalized;
            mainCamera.transform.Translate(moveDirection * Time.deltaTime * cameraSpeed);
        }
    }

    public void SetCameraDistance(float targetDistance, float targetSpeed)
    {
        cameraSpeed = targetSpeed;
        cameraDistance = targetDistance;
    }
    public void ChangeCameraDistance(float targetDistance, float targetSpeed)
    {
        cameraSpeed = targetSpeed;
        if (targetDistance < cameraDistance)
        {
            rightBoundary = 8f;
            upBoundary = 5.2f;
        }
        else
        {
            rightBoundary = 5.8f;
            upBoundary = 3.5f;
        }
        cameraDistance = targetDistance;
    }
}
