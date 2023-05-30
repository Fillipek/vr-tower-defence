using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(LineRenderer))]
public class Shooting : MonoBehaviour
{
    [SerializeField]
    private string enemyTag = "Enemy";

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private GameObject shootingOrigin;

    [SerializeField]
    private float velocity = 1.0f;

    [SerializeField]
    private float frequency = 1.0f;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject currentTarget = null;
    private LineRenderer lineRenderer;
    private float cooldown = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (shootingOrigin is null)
        {
            Debug.LogError(this + ": shootingOrigin not set.");
            enabled = false;
        }
        if (projectile is null)
        {
            Debug.LogError(this + ": projectile not set.");
            enabled = false;
        }
    }

    void Update()
    {
        TargetTheNearestEnemy();
        UpdateShooting();
        UpdateLineVisual();

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
        GameObject closestEnemy = null;
        float minDistance = 10000000f;
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                if (closestEnemy == null)
                {

                    closestEnemy = enemy;
                    minDistance = Vector3.Distance(closestEnemy.transform.position, MapSingleton.Instance.Map.transform.position);
                }
                else
                {
                    float distance = Vector3.Distance(enemy.transform.position, MapSingleton.Instance.Map.transform.position);
                    if (distance < minDistance)
                    {
                        closestEnemy = enemy;
                        minDistance = distance;
                    }
                }
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

    void UpdateShooting()
    {
        if (cooldown < 0f)
        {
            if (currentTarget != null)
            {
                Shoot();
                cooldown = 1f / frequency;
            }
        }
        else
        {
            cooldown -= Time.deltaTime;

        }
    }

    void UpdateLineVisual()
    {
        if (currentTarget != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, shootingOrigin.transform.position);
            lineRenderer.SetPosition(1, currentTarget.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void Shoot()
    {
        Vector3 targetVector = Vector3.Normalize(currentTarget.transform.position - shootingOrigin.transform.position);

        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = shootingOrigin.transform.position;
        newProjectile.transform.rotation = Quaternion.LookRotation(targetVector, Vector3.up);
        //newProjectile.GetComponent<Rigidbody>().velocity = targetVector * velocity;
        newProjectile.transform.SetParent(MapSingleton.Instance.Map.transform);
        newProjectile.GetComponent<ProjectileComponent>().Target = currentTarget;

    }
}
