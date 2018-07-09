using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Running = 0,
    Paused = 1,
    Over = 2
}

public static class GameManager
{

    public delegate void DamageChange(float damage, float maxDamage);
    public delegate void ScoreChange(int score);
    public delegate void LivesChange(int lives);
    public delegate void GameStateChange(GameState gameState);

    public static float Damage
    {
        get { return _damage; }
        set
        {
            if (!Mathf.Approximately(_damage, value))
            {
                _damage = value;
            }

            if (maxDamage <= _damage)
            {
                Lives--;
                _damage = 0;
            }

            if (DamageChanged != null)
                DamageChanged(_damage, maxDamage);
        }
    }
    static float _damage;
    public const float maxDamage = 100;
    public static event DamageChange DamageChanged;

    public static int HighScore
    {
        get
        {
            if (_highScore == null)
                _highScore = PlayerPrefs.GetInt("HighScore");
            return _highScore.Value;
        }
        set
        {
            if (value > _highScore)
            {
                _highScore = value;
                PlayerPrefs.SetInt("HighScore", _highScore.Value);
                if (HighScoreChanged != null)
                    HighScoreChanged(_highScore.Value);
            }
        }
    }
    static int? _highScore;
    public static event ScoreChange HighScoreChanged;

    public static int Lives
    {
        get
        {
            if (_lives == null)
                _lives = startingLives;
            return _lives.Value;
        }
        set
        {
            if (_lives != value)
            {
                _lives = value;

                if (LivesChanged != null)
                    LivesChanged(_lives.Value);

                if (_lives <= 0)
                    GameState = GameState.Over;
            }
        }
    }
    static int? _lives;
    public const int startingLives = 5;
    public static event LivesChange LivesChanged;

    public static int Score
    {
        get { return _score; }
        set
        {
            if (_score != value)
            {
                _score = value;

                if (ScoreChanged != null)
                    ScoreChanged(_score);

                if (_score > HighScore)
                    HighScore = _score;
            }
        }
    }
    static int _score;
    public static event ScoreChange ScoreChanged;

    public static AbstractUserControl[] UserControls
    {
        get
        {
            if (_userControls == null || !(_userControls.Length > 0))
            {
                _userControls = Object.FindObjectsOfType<AbstractUserControl>();
            }

            return _userControls;
        }
    }
    static AbstractUserControl[] _userControls;

    public static bool UserControlsEnabled
    {
        get { return _userControlsEnabled; }
        set
        {
            if (_userControlsEnabled != value)
            {
                _userControlsEnabled = value;
                if (_userControlsEnabled && _disabledUserControls != null)
                {
                    foreach (var c in _disabledUserControls)
                    {
                        c.enabled = true;
                    }
                }
                else if (!_userControlsEnabled)
                {
                    _disabledUserControls = UserControls.Where(uc => uc.enabled).ToList();
                    foreach (var c in _disabledUserControls)
                    {
                        c.enabled = false;
                    }
                }
            }
        }
    }
    static bool _userControlsEnabled = true;
    static List<AbstractUserControl> _disabledUserControls;


    public static GameState GameState
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                _state = value;
                if (GameStateChanged != null)
                    GameStateChanged(_state);

                switch (_state)
                {
                    case GameState.Over:
                        Time.timeScale = 0;
                        UserControlsEnabled = false;
                        break;

                    case GameState.Paused:
                        Time.timeScale = 0;
                        UserControlsEnabled = false;
                        break;

                    case GameState.Running:
                        UserControlsEnabled = true;
                        Time.timeScale = 1;
                        break;

                    default:
                        break;
                }
            }
        }
    }
    static GameState _state;
    public static GameStateChange GameStateChanged;

    public static void RestartGame()
    {
        ObjectPool.ClearAllPools();

        var spawners = Object.FindObjectsOfType<Spawner>();
        foreach (var s in spawners)
        {
            s.Restart();
        }

        var players = Object.FindObjectsOfType<AbstractPlayerController>();
        foreach (var p in players)
        {
            p.ResetPlayer();
        }

        var savedTransforms = Object.FindObjectsOfType<SaveTransform>();
        foreach (var t in savedTransforms)
        {
            if (t.IsSaved)
                t.RestoreTransform();
        }

        Lives = startingLives;
        Score = 0;
        Damage = 0;

        //LivesChanged = null;
        //ScoreChanged = null;
        //DamageChanged = null;
        //HighScoreChanged = null;
        //GameStateChanged = null;
        //SceneManager.LoadScene(0);

        GameState = GameState.Running;
    }
}
