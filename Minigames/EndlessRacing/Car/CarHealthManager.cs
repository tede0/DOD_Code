using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarHealthManager : CharacterStats
{
    [SerializeField] private GameObject carExplosionFx;

    private Transform player;
    private PlayerHUD _hud;
    private bool fxDone;
    private bool audioDone;
    private bool highscoreSaved;
    
    private void Start()
    {
        player = gameObject.transform;
        _hud = GetComponent<PlayerHUD>();
        SetVarieblesOnStart();
    }

    private void Update()
    {
        SetScreenOnDeath();
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        _hud.UpdateHealth(health, maxHealth);
    }
    
    private void SetScreenOnDeath()
    {
        if (isDead)
        {
            if (!highscoreSaved)
            {
                var score = (int) player.position.z;

                string json = File.ReadAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json");
                LoadHighscores.Highscore highscore = JsonUtility.FromJson<LoadHighscores.Highscore>(json);

                int prevRaceScore = Int32.Parse(highscore.RaceScore);

                if (score > prevRaceScore)
                {
                    highscore.RaceScore = score.ToString();
                }

                string jsonToWrite = JsonUtility.ToJson(highscore);
                File.WriteAllText(Application.dataPath + "/StreamingAssets" + "/highscore.json", jsonToWrite);
                Debug.Log(jsonToWrite);
                highscoreSaved = true;
            }


            StartCoroutine(DelayForExplosion());
            if (fxDone)
                SceneManager.LoadScene("IsDead");
        }
    }

    private IEnumerator DelayForExplosion()
    {
        if (!audioDone)
        {
            AudioManager.instance.Play("CarExplosion");
            audioDone = true;
        }
        
        Instantiate(carExplosionFx, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        fxDone = true;
    }
}
