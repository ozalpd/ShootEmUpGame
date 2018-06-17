using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject reference;

    [Header("Spawning")]
    [Range(0.001f, 100)]
    public float minRate = 0.75f;
    [Range(0.001f, 100)]
    public float maxRate = 2.75f;

    [Range(3, 120)]
    public int number = 5;
    int _remain;

    [Header("Locations")]
    public GameArea gameArea;

    [Range(0, 6)]// When set to zero, it's disabled that checking for
    public float minDistToPlayer; // minimum distance to player's position 

    Transform player;

    void Start()
    {
        _remain = number;
        StartCoroutine(Spawn());
    }


    IEnumerator Spawn()
    {
        if (minDistToPlayer > 0)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
            else
                Debug.LogWarning("No player found! Please assign a GameObject with a \"Player\" tag.");
        }

        while (_remain > 0)
        {
            var randPos = gameArea.GetRandomPosition(player, minDistToPlayer);

            //I'm keeping below statement as a visual debug sample
            if (player != null && Vector2.Distance(randPos, player.position) < minDistToPlayer)
            {
                Debug.DrawLine(transform.position, randPos);
                Vector3 fixedPosition = minDistToPlayer * (randPos - player.position).normalized;
                Debug.DrawLine(randPos, fixedPosition);
                randPos = fixedPosition;
                Debug.Break();
            }

            Instantiate(reference, randPos, new Quaternion(0, 0, 0, 0));
            _remain -= 1;

            yield return new WaitForSeconds(1.0f / Random.Range(minRate, maxRate));
        }
    }
}