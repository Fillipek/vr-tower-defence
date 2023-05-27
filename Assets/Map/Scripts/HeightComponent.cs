using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.DeviceBasedSnapTurnProvider;

public class HeightComponent : MonoBehaviour
{
    [SerializeField] float speed_of_lifting = 5f;


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
            if (math.abs(vec.y) > 0.5)
                ChangeHeight(vec.y * speed_of_lifting / 10f);
        }
    }

    public void ChangeHeight(float distance)
    {
        var pos = transform.position;
        var moveY = distance * Time.deltaTime;
        if (pos.y + moveY > 0.4 && pos.y + moveY < 1.5)
        {
            transform.Translate(new Vector3(0, moveY, 0) );
        }
    }
}
