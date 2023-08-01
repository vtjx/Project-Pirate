using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private GameObject powerUpUI;
    [SerializeField]
    private GameObject gameOverUI;
    [HideInInspector]
    public int enemiesRemaining;
    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public bool roundEnded;
    [SerializeField]
    private AudioClip victorySound;
    [SerializeField]
    private AudioClip defeatSound;
    [SerializeField]
    private AudioClip powerUpSound;
    [SerializeField]
    private TMP_Text enemiesRemainingText;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private AudioSource gmAS;

    private GameObject[] enemiesPresent;
    private int level;
    private int highestLevel;
    [SerializeField]
    private TMP_Text highestLevelTxt;
    private float spawnDelay;
    private float spawnDelayValue;
    private int amtToSpawn;
    private int amtToSpawnValue;
    private int enemiesRemainingValue;
    private int enemyRandomizer;
    private int xRandomizer;
    private float randomSpawnX;
    private bool spawned;
    private bool UIshowed;
    private Player player;
    public Crabby crabby;
    public FierceTooth fierceTooth;
    public PinkStar pinkStar;
    private AudioSource aS;

    private void Awake()
    {
        highestLevel = PlayerPrefs.GetInt("Highscore");
        player = GameObject.Find("Player").GetComponent<Player>();
        aS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        spawnDelayValue = 5;
        level = 1;
        amtToSpawn = 3;
        amtToSpawnValue = 3;
        enemiesRemaining = 3;
        enemiesRemainingValue = 3;
        spawnDelay = spawnDelayValue;
    }

    // Update is called once per frame
    void Update()
    {
        Game();
        EscMenu();
        toString();

        if (enemiesRemaining == 0)
        {
            roundEnded = true;
            spawned = false;
            aS.PlayOneShot(victorySound);
            powerUpUI.SetActive(true);
            level += 1;
            amtToSpawnValue += 2;
            enemiesRemainingValue += 2;
            amtToSpawn = amtToSpawnValue;
            enemiesRemaining = enemiesRemainingValue;
            crabby.healthValue += 1;
            fierceTooth.healthValue += 1;
            pinkStar.healthValue += 1;
            if (level < 6)
            {
                spawnDelayValue -= 0.5f;
            }
        }

        enemiesRemainingText.text = "Enemies Remaining: " + enemiesRemaining.ToString();
        levelText.text = "Level: " + level.ToString();
    }

    private void Game()
    {
        if (!gameOver)
        {
            if (!roundEnded && !player.dead && !spawned)
            {
                enemiesPresent = GameObject.FindGameObjectsWithTag("Enemies");
                if (amtToSpawn > 0)
                {
                    enemyRandomizer = Random.Range(0, 3);
                    xRandomizer = Random.Range(0, 2);
                    if (xRandomizer == 0)
                    {
                        randomSpawnX = -2.85f;
                    }
                    else
                    {
                        randomSpawnX = 2.85f;
                    }
                    Instantiate(enemies[enemyRandomizer], new Vector2(randomSpawnX, -0.5f), Quaternion.identity);
                    amtToSpawn -= 1;
                    spawned = true;
                }
            }

            if (spawned)
            {
                spawnDelay -= Time.deltaTime;
                if (spawnDelay <= 0)
                {
                    spawnDelay = spawnDelayValue;
                    spawned = false;
                }
            }
        }
        else if (gameOver && !UIshowed)
        {
            UIshowed = true;
            gmAS.mute = true;
            aS.PlayOneShot(defeatSound);
            gameOverUI.SetActive(true);
        }
    }

    private void EscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0){
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 1)
        {
            Time.timeScale = 1;
            gameOverUI.SetActive(false);
        }
    }

    public void healthPotion()
    {
        aS.PlayOneShot(powerUpSound);
        player.health += 1;
        powerUpUI.SetActive(false);
        roundEnded = false;
    }

    public void attackPotion()
    {
        aS.PlayOneShot(powerUpSound);
        player.atkDmg += 1;
        powerUpUI.SetActive(false);
        roundEnded = false;
    }

    public void agilityPotion()
    {
        aS.PlayOneShot(powerUpSound);
        player.moveSpd += 1;
        powerUpUI.SetActive(false);
        roundEnded = false;
    }

    public void retryBtn()
    {
        crabby.healthValue = 1;
        fierceTooth.healthValue = 1;
        pinkStar.healthValue = 1;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("Game");
    }

    public void exitBtn()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("Menu");
    }

    private void toString()
    {
        if (level > highestLevel)
        {
            highestLevel = level;
            PlayerPrefs.SetInt("Highscore", highestLevel);
        }
        highestLevelTxt.text = "HIGHEST LEVEL: " + highestLevel;
    }
}
