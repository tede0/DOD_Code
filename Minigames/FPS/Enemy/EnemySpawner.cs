using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting
    };

    [SerializeField] private GameObject waveCompleted;
    [SerializeField] private GameObject countdownUI;
    

    [SerializeField] private List<CharacterStats> enemyList;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float waveCountDown;
    
    [SerializeField] private Wave[] _waves;

    private SpawnState _state = SpawnState.Counting;

    private int currentWave;
    private bool counting;
    private bool completedAllWaves;
    private int secondsLeftForUI;

    public int CurrentWave => currentWave;
    public bool CompletedAllWaves => completedAllWaves;

    private void Start()
    {
        secondsLeftForUI = (int)timeBetweenWaves;
        waveCountDown = timeBetweenWaves;
        currentWave = 0;
    }

    private void Update()
    {
        DisplayCountdown(countdownUI);
        if (_state == SpawnState.Waiting)
        {
            if (!AllEnemiesAreDead())
            {
                return;
            }
            else
            {
                CompleteWave();
            }
        }
        
        if (waveCountDown <= 0)
        {
            if (_state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(_waves[currentWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        _state = SpawnState.Spawning;
        
        for (int i = 0; i < wave.enemiesAmount; i++)
        {
            SpawnZombie(wave.enemy);
            yield return new WaitForSeconds(wave.delay);
        }
        _state = SpawnState.Waiting;
        yield break;
    }

    private void SpawnZombie(GameObject enemy)
    {
        int randInt = Random.Range(1, spawnPoints.Length);
        Transform randomSpawner = spawnPoints[randInt];
        GameObject newEnemy = Instantiate(enemy, randomSpawner.position, randomSpawner.rotation);
        CharacterStats newEnemyStats = newEnemy.GetComponent<CharacterStats>();
        
        enemyList.Add(newEnemyStats);
    }

    private bool AllEnemiesAreDead()
    {
        int i = 0;
        foreach (var enemy in enemyList)
        {
            if (enemy.IsDead())
            {
                i++;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void CompleteWave()
    {
        StartCoroutine(WaveCompletedTextDuration(waveCompleted, 5));
        _state = SpawnState.Counting;
        waveCountDown = timeBetweenWaves;
        
        if (currentWave + 1 > _waves.Length - 1)
        {
            completedAllWaves = true;
            StartCoroutine(LoadVictoryScene());
        }
        else
        {
            currentWave++;
        }
    }
    
    private IEnumerator WaveCompletedTextDuration(GameObject waveCompleted, int duration)
    {
        waveCompleted.GetComponent<Text>().text = "Wave #" + (currentWave + 1) + " completed";
        waveCompleted.SetActive(true);
        yield return new WaitForSeconds(duration);
        waveCompleted.SetActive(false);
    }

    private IEnumerator LoadVictoryScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Victory");
    }

    private void DisplayCountdown(GameObject countDown)
    {
        if (_state == SpawnState.Counting)
        {
            countDown.SetActive(true);
            countDown.GetComponent<Text>().text = "Time until next wave: " + ((int)waveCountDown).ToString();
        }
        else
        {
            countDown.SetActive(false);
        }
    }
}
