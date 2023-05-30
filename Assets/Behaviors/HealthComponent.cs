using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] int health = 10;
    public int currentHealth { get;private set; }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
            DestroyImmediate(this.gameObject);
        }
    }

    public void DealDamage(int amount)
    {
        currentHealth -= amount;
        clampCurrentHealth();
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        clampCurrentHealth();
    }
    void clampCurrentHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
    }
}
