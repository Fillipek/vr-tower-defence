using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using System.Drawing;

public class ThrowCalculator : MonoBehaviour
{
    public static Vector3 CalculateThrowingVelocity(Vector3 startLocation, Vector3 targetLocation, float throwAngle)
    {
        Vector3 direction = targetLocation - startLocation;
        float distance = new Vector3(direction.x, 0, direction.z).magnitude;
        float elevation = startLocation.y - targetLocation.y;
        float directionangle = Mathf.Atan2(direction.z, direction.x);
        
        float initialVelocity = CalculateInitialVelocity(elevation, distance, throwAngle);

        Debug.Log("distance: " + distance + ", directionAngle: " + directionangle * Mathf.Rad2Deg + ", throwAngle: " + throwAngle);

        Vector3 throwingVelocity = RadialToCartesian(initialVelocity, directionangle, throwAngle*Mathf.Deg2Rad);

        return throwingVelocity;
    }

    private static float CalculateInitialVelocity(float elevation, float distance, float throwAngle)
    {
        float initialVelocity = Mathf.Sqrt(Physics.gravity.magnitude * distance * distance / (2 * Mathf.Cos(throwAngle * Mathf.Deg2Rad) * (elevation + Mathf.Tan(throwAngle * Mathf.Deg2Rad) * distance)));
        return initialVelocity;
    }

    private static Vector3 RadialToCartesian(float radius, float yaw, float pitch)
    {
        float x = radius * Mathf.Cos(yaw) * Mathf.Cos(pitch);
        float y = radius * Mathf.Sin(pitch);
        float z = radius * Mathf.Sin(yaw) * Mathf.Cos(pitch);
        return new Vector3(x,y,z);
    }
}
