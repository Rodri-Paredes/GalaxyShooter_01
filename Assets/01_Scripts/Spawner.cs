using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public float timeBtwSpawn = 1.5f;
    private float timer = 0;
    public Transform leftPoint;
    public Transform rightPoint;
    public List<GameObject> enemyPrefabs;
    public GameObject bossPrefab;
    public int score = 0;
    public Text ScoreText;
    public Text GameOverText;
    public static Spawner instance;

    public int currentWave = 0;
    public int maxWavesBeforeBoss = 3;
    private bool bossSpawned = false;
    private int enemiesToSpawnThisWave = 0;
    private int enemiesRemaining = 0;
    private bool bossDefeated = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameOverText.gameObject.SetActive(false);
        ScoreText.text = "SCORE: " + score;
        StartWave();
    }

    void Update()
    {
        if (!bossSpawned && enemiesRemaining == 0)
        {
            if (currentWave >= maxWavesBeforeBoss)
            {
                SpawnBoss();
            }
            else
            {
                StartWave();
            }
        }
    }

    void StartWave()
    {
        currentWave++;
        enemiesToSpawnThisWave += 2; 
        enemiesRemaining = enemiesToSpawnThisWave;
        SpawnEnemies();
    }


    void SpawnEnemies()
    {
        timer = 0;
        for (int i = 0; i < enemiesToSpawnThisWave; i++)
        {
            float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
            int enemy = Random.Range(0, enemyPrefabs.Count);
            Vector3 newPos = new Vector3(x, transform.position.y, 0);
            Instantiate(enemyPrefabs[enemy], newPos, Quaternion.Euler(0, 0, 180));
        }
    }

    public void OnEnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            if (currentWave >= maxWavesBeforeBoss)
            {
                SpawnBoss();
            }
            else if (bossSpawned && bossDefeated)
            {
                EndGame();
            }
            else
            {
                StartWave();
            }
        }
    }

    public void OnBossDefeated()
    {
        bossDefeated = true;

        if (currentWave >= maxWavesBeforeBoss)
        {
            EndGame();
            GameOverText.gameObject.SetActive(true);
        }

        currentWave = 0;
        bossSpawned = false;
        StartWave();
    }

    public void AddScore(int points)
    {
        score += points;
        ScoreText.text = "SCORE: " + score;
    }

    void SpawnBoss()
    {
        if (!bossSpawned)
        {
            Vector3 bossPosition = new Vector3((leftPoint.position.x + rightPoint.position.x) / 2, transform.position.y, 0);
            Instantiate(bossPrefab, bossPosition, Quaternion.Euler(0, 0, 180));
            bossSpawned = true;
        }
    }

    void EndGame()
    {
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
        {
            rb.simulated = false;
        }

        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        
            GameOverText.gameObject.SetActive(true);
        
    }
}
