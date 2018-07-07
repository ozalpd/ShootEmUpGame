using UnityEngine;
using System.Collections;

public class PlayerItem : MonoBehaviour
{
    public enum ItemType
    {
        RepairKit = 1,
        ExtraLife = 2,
        Weapon = 3
    }

    public ItemType itemType;

    AudioSource _audioSrc;
    Collider2D _collider;
    Renderer _renderer;

    public Weapon weapon;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
            return;
        switch (itemType)
        {
            case ItemType.ExtraLife:
                GameManager.Lives++;
                break;

            case ItemType.RepairKit:
                GameManager.Damage = 0;
                break;

            case ItemType.Weapon:
                if (weapon != null)
                {
                    var player = collision.GetComponent<AbstractPlayerController>();
                    if (player != null)
                    {
                        player.SwitchWeapon(weapon);
                    }
                }
                break;

            default:
                break;
        }

        StartCoroutine(ReleaseObject());
    }

    IEnumerator ReleaseObject()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        if (_audioSrc != null)
        {
            _audioSrc.Play();
            yield return new WaitForSeconds(_audioSrc.clip.length + 0.025f);
        }

        Destroy(gameObject);
    }
}