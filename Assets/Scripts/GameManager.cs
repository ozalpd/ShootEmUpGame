using UnityEngine;

public static class GameManager
{
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
    public const float maxDamage = 100;

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
}
