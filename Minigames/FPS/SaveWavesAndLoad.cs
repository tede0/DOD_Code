using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveWavesAndLoad : MonoBehaviour
{
    private EnemySpawner _wavesInfo;
    private PlayerStats _playerStats;

    private bool highscoreSaved;
    void Start()
    {
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        _wavesInfo = GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (_wavesInfo.CompletedAllWaves || _playerStats.IsDead())
            SaveWaveToFile();
    }

    private void SaveWaveToFile()
    {
        if (!highscoreSaved)
        {
            string json = File.ReadAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json");
            LoadHighscores.Highscore highscore = JsonUtility.FromJson<LoadHighscores.Highscore>(json);

            int previosWavesAmount = Int32.Parse(highscore.WaveScore);

            if (_wavesInfo.CurrentWave > previosWavesAmount)
            {
                if (_wavesInfo.CompletedAllWaves)
                    highscore.WaveScore = (_wavesInfo.CurrentWave + 1).ToString();
                else
                    highscore.WaveScore = _wavesInfo.CurrentWave.ToString();
            }

            string jsonToWrite = JsonUtility.ToJson(highscore);
            File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json", jsonToWrite);
            Debug.Log(jsonToWrite);
            highscoreSaved = true;
        }
    }
}
