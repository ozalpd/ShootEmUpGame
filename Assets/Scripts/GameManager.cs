using UnityEngine;

public static class GameManager
{
    public const float maxDamage = 100;

    public static float Damage
    {
        get { return _damage; }
        set
        {
            if (_damage != value)
            {
                _damage = value;
                Debug.Log(_damage);
            }

            if (maxDamage <= _damage)
            {
                Lives--;
                _damage = 0;
                //TODO: do some noise
            }
        }
    }
    static float _damage;

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
                Debug.Log("New High Score: " + _highScore.Value);
            }
        }
    }
    static int? _highScore;

    public static int Lives
    {
        get
        {
            if (_lives == null)
                _lives = 3;
            return _lives.Value;
        }
        set
        {
            if (_lives != value)
            {
                _lives = value;
            }
        }
    }
    static int? _lives;

    public static int Score
    {
        get { return _score; }
        set
        {
            if (_score != value)
            {
                _score = value;
                Debug.Log("Score: " + _score);
                if (_score > HighScore)
                    HighScore = _score;
            }
        }
    }
    static int _score;
}
