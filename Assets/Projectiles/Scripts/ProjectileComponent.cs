using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    [SerializeField] public float maxVelocity = 1f;

    [SerializeField] public int damage = 10;

    [SerializeField] private string enemyTag = "Enemy";

    [Tooltip("The amount of seconds object is kept alive after colliding with another object.")]
    [SerializeField] private float clearTime = 1f;

    [Tooltip("Set whether stick to collided object.")]
    [SerializeField] bool stickUponCollision = true;

    private float absoluteLifeTime = 30f;

    private Vector3 spawnLoc;

    private bool canDealDamage = true;

    void Start()
    {
        spawnLoc = transform.position;
        //GetComponent<Rigidbody>().velocity = transform.localToWorldMatrix * Vector3.forward * velocity;

        Destroy(gameObject, absoluteLifeTime);
    }

    void Update()
    {
        var velocity = GetComponent<Rigidbody>().velocity;
        if (canDealDamage && velocity.magnitude > .1f)
        {
            transform.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageToEnemy(collision.gameObject);
        if (stickUponCollision)
        {
            StickTo(collision.gameObject);
        }
        Destroy(gameObject, clearTime);
        canDealDamage = false;
    }

    void DealDamageToEnemy(GameObject enemy)
    {
        if (enemy.tag == enemyTag && canDealDamage)
        {
            var enemyHealth = enemy.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.DealDamage(damage);
            }
        }
    }

    void StickTo(GameObject obj)
    {
        transform.SetParent(obj.transform);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
