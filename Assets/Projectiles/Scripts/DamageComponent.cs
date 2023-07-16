using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageComponent: MonoBehaviour
{
    [SerializeField] public int damage = 50;
    [SerializeField] public int ammo = 10;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] public float radius = 1f;
    [SerializeField] private GameObject explosion;

    private void Update()
    {
        if(ammo <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageToEnemy(collision.gameObject, damage);
    }

    public void DealDamageToEnemy(GameObject enemy, int damage)
    {
        if (enemy.tag == enemyTag)
        {
            if (explosion != null)
            {
                Explode();
            }
            var enemyHealth = enemy.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.DealDamage(damage);
                ammo--;
            }
        }
    }

    private void Explode()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag(enemyTag))
        {
            if (Vector3.Distance(enemy.transform.position, gameObject.transform.position) < radius)
            {
                GetComponent<DamageComponent>().DealDamageToEnemy(enemy, damage);
            }
        }
        if (explosion != null)
        {
            var explosionObj = Instantiate(explosion);
            explosionObj.transform.position = transform.position;
            explosionObj.transform.SetParent(transform.parent);
            Destroy(explosionObj, explosionObj.GetComponent<ParticleSystem>().startLifetime);
        }
    }
}
