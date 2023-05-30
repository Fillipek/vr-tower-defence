using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlaceOnTheMapComponent : MonoBehaviour
{

    public enum State
    {
        Bought,
        Active,
        Disable
    }

    public State state = State.Bought;

    public void onRaise()
    {
        if (state == State.Active)
        {
            state = State.Disable;
        }
    }
    public void onDrop()
    {
        
        Vector3 pos = new Vector3(transform.position.x, MapSingleton.Instance.Map.transform.position.y, transform.position.z);
        
        transform.SetPositionAndRotation(pos, new Quaternion());
        transform.SetParent(MapSingleton.Instance.Towers.transform);


        if (transform.localPosition.magnitude > 1f)
        {
            if (state == State.Bought)
            {
                this.GetComponent<PriceComponent>().Return();
            }
            else
            {
                this.GetComponent<PriceComponent>().Sell();
            }
            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, .7f);

        
        state = State.Active;
    }

    public bool isActive()
    {
        return state == State.Active;
    }
   
}
