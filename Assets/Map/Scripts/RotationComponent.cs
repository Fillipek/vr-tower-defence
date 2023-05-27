using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RotationComponent : MonoBehaviour
{
    [SerializeField] float speed_of_rotation = 60f;

    [SerializeField]
    [Tooltip("A list of controllers that allow Snap Turn.  If an XRController is not enabled, or does not have input actions enabled, snap turn will not work.")]
    List<XRBaseController> m_Controllers = new List<XRBaseController>();
    public List<XRBaseController> controllers
    {
        get => m_Controllers;
        set => m_Controllers = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (m_Controllers.Count == 0)
        {

        }
        else
        {
            Vector2 vec = Vector2.zero;
            var controller = m_Controllers[0] as UnityEngine.XR.Interaction.Toolkit.XRController;
            controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out vec);
            if (math.abs(vec.x)>0.2)
                RotateY(vec.x * speed_of_rotation);
        }
    }

    public void RotateY(float angle)
    {
        transform.Rotate(new Vector3(0, angle, 0) * Time.deltaTime);
    }
}
