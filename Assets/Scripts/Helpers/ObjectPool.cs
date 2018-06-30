using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    List<GameObject> _objectList;
    GameObject _originalGO;
    int _poolSize;
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

    public static GameObject GetInstance(int originalId, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        return _pools[originalId].GetOnePoolInstance(position, rotation);
    }

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