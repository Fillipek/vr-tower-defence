using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Collider))]
public class Shooting : MonoBehaviour
{
    [SerializeField]
    private string enemyTag = "Enemy";

    [SerializeField]
    private GameObject projectile;

    private List<GameObject> enemiesInRange;
    private GameObject currentTarget;

    void Start()
    {
        enemiesInRange = new List<GameObject>();
        currentTarget = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In shooting range: " + other.gameObject);
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Enemy tag detected, adding to enemy list");
            enemiesInRange.Add(other.gameObject);
            if (currentTarget is null)
            {
                TargetTheNearestEnemy();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Left shooting range: " + other.gameObject);
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Enemy tag detected, removing from enemy list");
            enemiesInRange.Remove(other.gameObject);
            TargetTheNearestEnemy();
        }
    }

    private void TargetTheNearestEnemy()
    {
        if (enemiesInRange.Count == 0)
        {
            Debug.Log("No enemies in list, target set to null.");
            currentTarget = null;
            return;
        }
        GameObject closestEnemy = enemiesInRange.First();
        float minDistance = Vector3.Distance(closestEnemy.transform.position, transform.position);
        foreach (var enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < minDistance)
            {
                closestEnemy = enemy;
                minDistance = distance;
            }
        }
        if (currentTarget != closestEnemy)
        {
            Debug.Log("Changing target from " + currentTarget + " to " + closestEnemy);
            currentTarget = closestEnemy;
        }
        else
        {
            Debug.Log("Target uchanged.");
        }
        
    }
}
