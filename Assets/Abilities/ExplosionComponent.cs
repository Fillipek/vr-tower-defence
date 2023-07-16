using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionComponent: MonoBehaviour
{
    [SerializeField] public float radius = 1f;
    [SerializeField] public int damage = 50;
    [SerializeField] private string enemyTag = "Enemy";
    public List<GameObject> objs;

    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject.FindGameObjectsWithTag("tag");
        Destroy(gameObject);
    }

    void DealDamageToEnemiesInRange(GameObject enemy)
    {
        if (enemy.tag == enemyTag)
        {
            var enemyHealth = enemy.GetComponent<HealthComponent>();
            if (enemyHealth != null)
            {
                enemyHealth.DealDamage(damage);
            }
        }
    }
}
