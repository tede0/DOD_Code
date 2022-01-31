using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadHighscores : MonoBehaviour
{
    [SerializeField] private Text raceScore;
    [SerializeField] private Text waveCound;
    [SerializeField] private Text bossDefeated;
    private void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json");
        Highscore highscore = JsonUtility.FromJson<Highscore>(json);

        var bossDefeatedStr = highscore.BossDefeated ? "YES" : "NO";
        
        raceScore.text = "RACE SCORE: " + highscore.RaceScore;
        waveCound.text = "MAX WAVE: " + highscore.WaveScore;
        bossDefeated.text = "BOSS DEFEATED: " + bossDefeatedStr;

        /*Highscore highscore = new Highscore();
        highscore.RaceScore = "0";
        highscore.WaveScore = "0";
        highscore.BossDefeated = false;

        string json = JsonUtility.ToJson(highscore);
        File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json", json);*/
    }


    public class Highscore
    {
        public string RaceScore;
        public string WaveScore;
        public bool BossDefeated;
    }
}
