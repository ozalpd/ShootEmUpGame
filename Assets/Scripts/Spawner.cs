using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPosition
{
    RandomInTheGameArea = 0,
    TopOfTheGameArea = 1,
    TransformPosition = 3
}
public class Spawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject[] reference;
    int[] _referenceId;
    public SpawnPosition spawnPosition;
    [Tooltip("If this is unchecked spawns sequentally.")]
    public bool spawnRandomly = true;

    [Header("Spawning")]
    [Range(0.001f, 100)]
    public float minRate = 0.75f;
    [Range(0.001f, 100)]
    public float maxRate = 2.75f;

    [Range(3, 120)]
    public int number = 5;
    int _remain;

    [Range(0f, 360f)]
    public float spreadAngle = 30;
    [Range(0f, 360f)]
    public float angle = 180;

    [Range(1f, 10f)]
    public float minSpeed = 5;
    [Range(10f, 100f)]
    public float maxSpeed = 15;


    GameArea gameArea { get { return GameArea.Main; } }

    [Range(0, 6)]// When set to zero, it's disabled that checking for
    public float minDistToPlayer; // minimum distance to player's position 

    [Header("Animator")]
    [Range(1f, 10f)]
    public float delayIn = 3;
    [Range(1f, 10f)]
    public float delayOut = 3;

    Transform player;
    Animator _animator;
    int _animatorSpawningId;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
            _animatorSpawningId = Animator.StringToHash("Spawning");

        _referenceId = new int[reference.Length];
        for (int i = 0; i < reference.Length; i++)
        {
            _referenceId[i] = reference[i].GetInstanceID();
            ObjectPool.GetOrInitPool(reference[i]);
        }
    }


    private int GetNextIndex()
    {
        if (!(reference.Length > 1)) //if reference is null or length is zero let the compiler throws a null reference exception
        {
            return 0;
        }
        else if (spawnRandomly)
        {
            return Random.Range(0, reference.Length - 1);
        }
        else
        {
            index = index.HasValue && index.Value < reference.Length - 1 ? index.Value + 1 : 0;
            return index.Value;
        }
    }
    int? index;

    //Start can be used as a coroutine
    IEnumerator Start()
    {
        _remain = number;

        if (minDistToPlayer > 0)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                player = playerGO.transform;
            else
                Debug.LogWarning("No player found! Please assign a GameObject with a \"Player\" tag.");
        }

        if (_animator != null)
        {
            _animator.SetBool(_animatorSpawningId, true);
            yield return new WaitForSeconds(delayIn);
        }

        while (_remain > 0)
        {
            var randPos = gameArea != null
                && (spawnPosition == SpawnPosition.RandomInTheGameArea || spawnPosition == SpawnPosition.TopOfTheGameArea)
                ? gameArea.GetRandomPosition(player, minDistToPlayer, spawnPosition == SpawnPosition.TopOfTheGameArea)
                          : transform.position;

            //I'm keeping below statement as a visual debug sample
            if (player != null && Vector2.Distance(randPos, player.position) < minDistToPlayer)
            {
                Debug.DrawLine(transform.position, randPos);
                Vector3 fixedPosition = minDistToPlayer * (randPos - player.position).normalized;
                Debug.DrawLine(randPos, fixedPosition);
                randPos = fixedPosition;
                Debug.Break();
            }

            var go = ObjectPool.GetInstance(_referenceId[GetNextIndex()], randPos, new Quaternion(0, 0, 0, 0));

            var rb2d = go.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                float direction = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f) + angle;
                Vector2 velocity = new Vector2(
                    Mathf.Sin(Mathf.Deg2Rad * direction),
                    Mathf.Cos(Mathf.Deg2Rad * direction));
                rb2d.velocity = velocity * Random.Range(minSpeed, maxSpeed);
            }

            _remain -= 1;

            yield return new WaitForSeconds(1.0f / Random.Range(minRate, maxRate));
        }

        if (_animator != null)
        {
            yield return new WaitForSeconds(delayOut);
            _animator.SetBool(_animatorSpawningId, false);
        }

        gameObject.SetActive(false);
    }
}