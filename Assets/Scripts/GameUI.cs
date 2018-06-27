using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text txtScore;
    public Text txtHighScore;
    public Text txtLives;

    public Slider sliderDamage;

    void Start()
    {
        GameManager_DamageChanged(0, GameManager.maxDamage);
        GameManager.DamageChanged += GameManager_DamageChanged;

        GameManager_LivesChanged(GameManager.Lives);
        GameManager.LivesChanged += GameManager_LivesChanged;

        GameManager_ScoreChanged(GameManager.Score);
        GameManager.ScoreChanged += GameManager_ScoreChanged;

        GameManager_HighScoreChanged(GameManager.HighScore);
        GameManager.HighScoreChanged += GameManager_HighScoreChanged;
    }

    void GameManager_DamageChanged(float damage, float maxDamage)
    {
        sliderDamage.value = damage / maxDamage;
    }

    void GameManager_LivesChanged(int lives)
    {
        txtLives.text = string.Format("{0} {1}", GameManager.Lives, GameManager.Lives > 1 ? "LIVES" : "LIFE");
    }

    void GameManager_ScoreChanged(int score)
    {
        txtScore.text = string.Format("SCORE: {0}", GameManager.Score);
    }

    void GameManager_HighScoreChanged(int score)
    {
        txtHighScore.text = string.Format("HIGH SCORE: {0}", GameManager.HighScore);
    }
}