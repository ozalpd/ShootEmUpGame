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
        }
    }
    static float _damage;
}
