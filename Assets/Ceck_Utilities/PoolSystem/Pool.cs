using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private GameObject _prefab;
    private Transform _poolParent;
    private Queue<GameObject> _queue = new Queue<GameObject>();

    public Pool(GameObject prefabObject, Transform poolParent = null)
    {
        _prefab = prefabObject;
        _poolParent = poolParent;
    }

    public GameObject Spawn(Transform parent)
    {
        if (_queue.Count > 0)
        {
            var go = _queue.Dequeue();
            go.transform.SetParent(parent);
            go.SetActive(true);

            return go;
        }
        else
        {
            return GameObject.Instantiate(_prefab, parent);
        }
    }

    public void Despawn(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(_poolParent);
        _queue.Enqueue(go);
    }

    public void Warm(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var go = GameObject.Instantiate(_prefab, _poolParent);
            Despawn(go);
        }
    }
}
