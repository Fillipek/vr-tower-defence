using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static Vector3 SphericalToCartesian(float radius, float polar, float elevation)
    {
        polar *= Mathf.Deg2Rad;
        elevation *= Mathf.Deg2Rad;

        float a = radius * Mathf.Cos(elevation);
        Vector3 outCart;
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
        return outCart;
    }
}