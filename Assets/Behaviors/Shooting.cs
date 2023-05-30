using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PlaceOnTheMapComponent))]
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

    private PlaceOnTheMapComponent placeOnTheMapComponent;

    void Start()
    {
        placeOnTheMapComponent = GetComponent<PlaceOnTheMapComponent>();
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
        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Add(other.gameObject);
            if (currentTarget is null)
            {
                TargetTheNearestEnemy();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Remove(other.gameObject);
            TargetTheNearestEnemy();
        }
    }

    private void TargetTheNearestEnemy()
    {
        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }
        GameObject closestEnemy = null;
        float minDistance = 10000000f;
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null && enemy.GetComponent<EnemyBaseComponent>().isActive())
            {
                if (closestEnemy == null)
                {

                    closestEnemy = enemy;
                    minDistance = closestEnemy.transform.localPosition.sqrMagnitude;
                }
                else
                {
                    float distance = enemy.transform.localPosition.sqrMagnitude;
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
            currentTarget = closestEnemy;
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
        if (placeOnTheMapComponent.isActive())
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
}
