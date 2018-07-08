using UnityEngine;

public class SaveTransform : MonoBehaviour
{
    public bool autoSave = true;

    Vector3 _position;
    Quaternion _rotation;

    public bool IsSaved { get { return _isSaved; } }
    bool _isSaved;


    public void Save()
    {
        _position = transform.position;
        _rotation = transform.rotation;
        _isSaved = true;
    }

    public void RestoreTransform()
    {
        transform.position = _position;
        transform.rotation = _rotation;
    }

    private void Start()
    {
        if (autoSave)
            Save();
    }
}