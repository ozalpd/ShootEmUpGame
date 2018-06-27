using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public int score = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Projectile"))
            return;

        GameManager.Score += score;

        Destroy(gameObject);
    }
}
