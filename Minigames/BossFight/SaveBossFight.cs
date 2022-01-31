using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveBossFight : MonoBehaviour
{
    private EnemyStats _bossStats;

    private bool isBossDead;
    private bool highscoreSaved;
    private bool load;
    void Start()
    {
        _bossStats = GetComponent<EnemyStats>();
    }
    
    void Update()
    {
        isBossDead = _bossStats.IsDead();
        SetScreenOnVictory();
    }
    
    private void SetScreenOnVictory()
    {
        if (isBossDead)
        {
            if (!highscoreSaved)
            {
                string json = File.ReadAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json");
                LoadHighscores.Highscore highscore = JsonUtility.FromJson<LoadHighscores.Highscore>(json);

                highscore.BossDefeated = true;
                
                string jsonToWrite = JsonUtility.ToJson(highscore);
                File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json", jsonToWrite);
                Debug.Log(jsonToWrite);
                highscoreSaved = true;
            }


            StartCoroutine(Delay(5));
            if (load)
                SceneManager.LoadScene("Victory");
        }
    }

    private IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
        load = true;
    }
}
