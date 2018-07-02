using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a number of instantiated game objects ready to use in memory and recyles them to keep memory unfragmanted.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// The pool of objects
    /// </summary>
    List<GameObject> _objectList;

    /// <summary>
    /// The original GameObject
    /// </summary>
    GameObject _originalGO;

    /// <summary>
    /// Number of the instantiated game objects
    /// </summary>
    int _poolSize;

    /// <summary>
    /// Keeps the object pool instances
    /// </summary>
    static Dictionary<int, ObjectPool> _pools = new Dictionary<int, ObjectPool>();

    void Init()
    {
        _objectList = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject go = Instantiate(_originalGO);
            go.SetActive(false);
            go.transform.SetParent(transform);
            _objectList.Add(go);
        }

        int id = _originalGO.GetInstanceID();
        if (_pools.ContainsKey(id))
            _pools[id] = this;
        else
            _pools.Add(id, this);
    }

    private GameObject GetOnePoolInstance(Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        var go = _objectList.FirstOrDefault(o => !o.activeSelf);
        if (go == null)
        {
            go = Instantiate(_originalGO, position, rotation);
            go.transform.SetParent(transform);
            _objectList.Add(go);
        }
        else
        {
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);
        }

        return go;
    }

    /// <summary>
    /// Finds an ObjectPool instance from a static dictionary. If no instance found initiliazes a new instance.
    /// </summary>
    /// <param name="original">The original GameObject that is needed to be in ObjectPool instance</param>
    /// <param name="poolSize">Number of the instantiated game objects in the new instance. If there is already an instance of ObjectPool this param has no effect.</param>
    /// <returns>ObjectPool that initiliazed or found in Dictionary</returns>
    public static ObjectPool GetOrInitPool(GameObject original, int poolSize = 200)
    {
        int id = original.GetInstanceID();
        ObjectPool pool = _pools.ContainsKey(id) ? _pools[id] : null;
        if (pool == null)
        {
            var poolGO = new GameObject("ObjectPool: " + original.name);
            pool = poolGO.AddComponent<ObjectPool>();
            pool._originalGO = original;
            pool._poolSize = poolSize;
            pool.Init();
        }

        return pool;
    }

    /// <summary>
    /// Intended to be used like Object.Instantiate method.
    /// </summary>
    /// <param name="originalId">Hash ID of an existing object that you want to activate a copy of it</param>
    /// <param name="position">Position for the activated object.</param>
    /// <param name="rotation">Orientation for the activated object.</param>
    /// <returns></returns>
    public static GameObject GetInstance(int originalId, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        return _pools[originalId].GetOnePoolInstance(position, rotation);
    }

    /// <summary>
    /// Intended to be used like Object.Instantiate method.
    /// </summary>
    /// <param name="original">An existing object that you want to activate a copy of it.</param>
    /// <param name="position">Position for the activated object.</param>
    /// <param name="rotation">Orientation for the activated object.</param>
    /// <param name="poolSize">If an ObjectPool instance is not found, this will be poolSize of the new instance</param>
    /// <returns></returns>
    public static GameObject GetInstance(GameObject original, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), int poolSize = 200)
    {
        ObjectPool pool = GetOrInitPool(original);
        return pool.GetOnePoolInstance(position, rotation);
    }

    public static void Release(GameObject obj)
    {
        obj.SetActive(false);
    }
}