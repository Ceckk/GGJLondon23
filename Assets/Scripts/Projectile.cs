using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _lifeTime = 10f;

    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * _movementSpeed * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= 0.5f)
        {
            PlayerManager.Instance.Die();
        }
    }
}