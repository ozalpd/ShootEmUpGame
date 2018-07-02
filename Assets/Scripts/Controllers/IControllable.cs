using UnityEngine;

public interface IControllable
{
    bool Firing { get; set; }
    void Move(Vector2 movement);
}