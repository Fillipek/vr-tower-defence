using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlaceOnTheMapComponent : MonoBehaviour
{
    GameObject Map;
    GameObject TowersGameObject;
    // Start is called before the first frame update
    void Start()
    {
        Map = GameObject.Find("Map");
        TowersGameObject = Map.GetNamedChild("Towers");
    }

    

    public void onDrop()
    {

        Vector3 pos = new Vector3(transform.position.x, Map.transform.position.y, transform.position.z);
        
        transform.SetPositionAndRotation(pos, new Quaternion());
        transform.SetParent(TowersGameObject.transform);

        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, .7f);
    }

  
   
}
