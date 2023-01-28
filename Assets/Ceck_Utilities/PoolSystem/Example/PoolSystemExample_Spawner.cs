using UnityEngine;

namespace Shortcat
{
    public class PoolSystemExample_Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab = null;
        [SerializeField] private float _cooldown = 0.1f;

        private float _t = 0;

        void Update()
        {
            if (_t >= _cooldown)
            {
                var obj = PoolManager.Instance.Spawn(_prefab);
                obj.transform.position = transform.position;
                _t = 0;
            }
            else
            {
                _t += Time.deltaTime;
            }
        }
    }
}
