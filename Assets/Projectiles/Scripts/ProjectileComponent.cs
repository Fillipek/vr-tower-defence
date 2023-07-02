using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    [SerializeField] public float velocity = 1f;

    [SerializeField] public int damage = 10;

    [SerializeField] private string enemyTag = "Enemy";

    [Tooltip("The amount of seconds object is kept alive after colliding with another object.")]
    [SerializeField] private float clearTime = 1f;

    [Tooltip("Set whether stick to collided object.")]
    [SerializeField] bool stickUponCollision = true;

    [Tooltip("Minimum travel distance to start sticking.")]
    [SerializeField] private float minTravelDistance = 0.1f;

    private float absoluteLifeTime = 30f;

    private Vector3 spawnLoc;
    public Vector3 TargetLoc { get; set; }

    private bool canDealDamage = true;

    void Start()
    {
        spawnLoc = transform.position;

        GetComponent<Rigidbody>().velocity = Vector3.Normalize(TargetLoc-spawnLoc) * velocity;

        Destroy(gameObject, absoluteLifeTime);
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageToEnemy(collision.gameObject);
        if (stickUponCollision)
        {
            StickTo(collision.gameObject);
        }
        Destroy(gameObject, clearTime);
    }

    void DealDamageToEnemy(GameObject enemy)
    {
        if (enemy.tag == enemyTag && canDealDamage)
        {
            var enemyHealth = enemy.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.DealDamage(damage);
                canDealDamage = false;
            }
        }
    }

    void StickTo(GameObject obj)
    {
        if (Vector3.Magnitude(transform.position - spawnLoc) >= minTravelDistance)
        {
            transform.SetParent(obj.transform);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
