using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlaceOnTheMapComponent : MonoBehaviour
{
    public void onDrop()
    {

        Vector3 pos = new Vector3(transform.position.x, MapSingleton.Instance.Map.transform.position.y, transform.position.z);
        
        transform.SetPositionAndRotation(pos, new Quaternion());
        transform.SetParent(MapSingleton.Instance.Towers.transform);

        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, .7f);
    }

  
   
}
