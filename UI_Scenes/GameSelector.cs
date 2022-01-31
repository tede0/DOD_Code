using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour
{
    public void LoadRacing()
    {
        SceneManager.LoadScene("EndlessRacing");
    }
    
    public void LoadZombieWaves()
    {
        SceneManager.LoadScene("ZombieWaves");
    }
    
    public void LoadBoss()
    {
        SceneManager.LoadScene("BossFight");
    }
    
    public void LoadHighscore()
    {
        SceneManager.LoadScene("HighScores");
    }
    
    
    public void Back()
    {
        SceneManager.LoadScene("StartScene");
    }
}
