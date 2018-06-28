using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("HUD")]
    public Text txtScore;
    public Text txtHighScore;
    public Text txtLives;

    public Slider sliderDamage;
    Image _imgDamageFillArea;
    public Color colorDamageMin = Color.yellow;
    public Color colorDamageMax = Color.red;

    [Header("Menu Items")]
    public Button pauseButton;
    public Button resumeButton;
    public Image pauseMenu;

    public Text textGameState;

    [Header("Settings")]
    public Slider sliderVolumeMusic;
    public Slider sliderVolumeSFX;


    public float MusicVolume
    {
        get { return PlayerSettings.MusicVolume; }
        set { PlayerSettings.MusicVolume = value; }
    }

    public float SfxVolume
    {
        get { return PlayerSettings.SfxVolume; }
        set { PlayerSettings.SfxVolume = value; }
    }

    void Awake()
    {
        _imgDamageFillArea = sliderDamage.fillRect.GetComponent<Image>();
    }

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

        updateUI(GameManager.GameState);
        GameManager.GameStateChanged += updateUI;

        sliderVolumeMusic.value = MusicVolume;
        sliderVolumeSFX.value = SfxVolume;
    }

    //Its better way to unsubscribe events inside from the GameManager
    //void OnDestroy()
    //{
    //    GameManager.DamageChanged -= GameManager_DamageChanged;
    //    GameManager.LivesChanged -= GameManager_LivesChanged;
    //    GameManager.ScoreChanged -= GameManager_ScoreChanged;
    //    GameManager.HighScoreChanged -= GameManager_HighScoreChanged;
    //    GameManager.GameStateChanged -= updateUI;
    //}

    void GameManager_DamageChanged(float damage, float maxDamage)
    {
        sliderDamage.value = damage / maxDamage;
        _imgDamageFillArea.color = Color.Lerp(colorDamageMin, colorDamageMax, damage / maxDamage);
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

    public void PauseGame()
    {
        GameManager.GameState = GameState.Paused;
    }

    public void RestartGame()
    {
        GameManager.RestartGame();
    }

    public void ResumeGame()
    {
        GameManager.GameState = GameState.Running;
    }

    void updateUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Over:
                textGameState.text = "GAME OVER";
                break;

            case GameState.Paused:
                textGameState.text = "GAME PAUSED";
                break;

            case GameState.Running:
                break;

            default:
                break;
        }

        pauseButton.gameObject.SetActive(gameState == GameState.Running);
        resumeButton.gameObject.SetActive(gameState == GameState.Paused);
        pauseMenu.gameObject.SetActive(gameState != GameState.Running);
    }

}