using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageComponent: MonoBehaviour
{
    [SerializeField] public int damage = 50;
    [SerializeField] public int ammo = 10;
    [SerializeField] private string enemyTag = "Enemy";

    private void Update()
    {
        if(ammo <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageToEnemy(collision.gameObject);
    }

    void DealDamageToEnemy(GameObject enemy)
    {
        if (enemy.tag == enemyTag)
        {
            var enemyHealth = enemy.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.DealDamage(damage);
                ammo--;
            }
        }
    }
}
