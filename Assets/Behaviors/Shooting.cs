using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        float minDistance = float.MaxValue;
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
            float points = 20;
            float timeStep = 0.03f;

            lineRenderer.positionCount = Mathf.CeilToInt(points + 1);

            Vector3 startPos = shootingOrigin.transform.position;
            Vector3 targetPos = currentTarget.transform.position + currentTarget.GetComponent<Rigidbody>().velocity;
            Vector3 velocity = ThrowCalculator.CalculateThrowingVelocity(startPos, targetPos, 0f);

            for (int i = 0; i <= points; i++)
            {
                float time = i * timeStep;
                Vector3 point = startPos + velocity * time + 0.5f * time * time * Physics.gravity;
                lineRenderer.SetPosition(i, point);
            }
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
            GameObject newProjectile = Instantiate(projectile);

            Vector3 startPos = shootingOrigin.transform.position;
            Vector3 targetPos = currentTarget.transform.position;
            Vector3 targetVector = (currentTarget.transform.position - shootingOrigin.transform.position).normalized;
            float projectileSpeed = newProjectile.GetComponent<ProjectileComponent>().velocity;

            //targetVector = PredictTrajectory(shootingOrigin.transform.position, currentTarget.transform.position, projectileSpeed);
            targetVector = ThrowCalculator.CalculateThrowingVelocity(startPos, targetPos, 0f);

            newProjectile.transform.position = shootingOrigin.transform.position;
            newProjectile.transform.rotation = Quaternion.LookRotation(targetVector, Vector3.up);
            newProjectile.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(targetVector, projectileSpeed);
            newProjectile.transform.SetParent(MapSingleton.Instance.Map.transform);
        }
    }

    private Vector3 PredictTrajectory(Vector3 source, Vector3 target, float speed)
    {
        float yaw, pitch;
        var straight = (currentTarget.transform.position - shootingOrigin.transform.position).normalized;

        pitch = Mathf.Asin((target - source).magnitude * Physics.gravity.magnitude / (speed * speed)) / 2f;
        pitch *= Mathf.Rad2Deg;

        yaw = Mathf.Atan2(straight.z, straight.x);
        yaw *= Mathf.Rad2Deg;

        Debug.Log("pitch = " + pitch + ", yaw = " + yaw);
        Debug.Log("Flat aiming = " + straight);
        Debug.Log("After correction = " + SphericalToCartesian(yaw, -pitch));

        return straight;
    }

    private Vector3 SphericalToCartesian(float polar, float elevation)
    {
        Vector3 outCart = new Vector3();
        float a = Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
        return outCart;
    }
}
