using UnityEngine;

namespace Shortcat
{
    public class PoolSystemExample_TestCube : MonoBehaviour
    {
        [SerializeField] private float _lifeSpanInSeconds = 1f;
        [SerializeField] private float _speed = 10f;

        private float _t = 0;

        void Update()
        {
            transform.position += Vector3.down * _speed * Time.deltaTime;

            if (_t >= _lifeSpanInSeconds)
            {
                PoolManager.Instance.Despawn(gameObject);
                _t = 0;
            }
            else
            {
                _t += Time.deltaTime;
            }
        }
    }
}
