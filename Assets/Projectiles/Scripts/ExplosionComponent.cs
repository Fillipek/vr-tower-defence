using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionComponent: MonoBehaviour
{
    [SerializeField] public float radius = 1f;
    [SerializeField] public int damage = 50;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject explosion;

    private void Start()
    {
    }

    public void Explode()
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
