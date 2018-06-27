using UnityEngine;

public static class GameManager
{
    //Maybe ScoreManager is a better name for this class
    public delegate void DamageChange(float damage, float maxDamage);
    public delegate void ScoreChange(int score);
    public delegate void LivesChange(int lives);


    public static float Damage
    {
        get { return _damage; }
        set
        {
            if (_damage != value)
            {
                _damage = value;
            }

            if (maxDamage <= _damage)
            {
                Lives--;
                _damage = 0;
                //TODO: do some noise
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
}
