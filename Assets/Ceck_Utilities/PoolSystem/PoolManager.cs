using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<GameObject, Pool> _prefabDictionary = new Dictionary<GameObject, Pool>();
    private Dictionary<GameObject, Pool> _instanceDictionary = new Dictionary<GameObject, Pool>();

    public GameObject Spawn(GameObject prefab, Transform parent = null)
    {
        if (!_prefabDictionary.ContainsKey(prefab))
        {
            _prefabDictionary[prefab] = new Pool(prefab, transform);
        }

        var pool = _prefabDictionary[prefab];
        var instance = _prefabDictionary[prefab].Spawn(parent);
        _instanceDictionary.Add(instance, pool);

        return instance;
    }

    public void Despawn(GameObject instance)
    {
        if (_instanceDictionary.ContainsKey(instance))
        {
            _instanceDictionary[instance].Despawn(instance);
            _instanceDictionary.Remove(instance);
        }
        else if (instance != null)
        {
            Debug.LogWarning(instance.name + " not found in pool... destroying");
            Destroy(instance);
        }
    }

    public void Warm(GameObject prefab, int amount)
    {
        if (!_prefabDictionary.ContainsKey(prefab))
        {
            _prefabDictionary[prefab] = new Pool(prefab, transform);
        }

        _prefabDictionary[prefab].Warm(amount);
    }
}
