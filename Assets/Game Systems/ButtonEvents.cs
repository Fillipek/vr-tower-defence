using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonEvents : MonoBehaviour
{
   

    

    XRController controller = null;

    [Serializable]
    public class ButtonEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onPressPrimary")]
    [SerializeField]
    private ButtonEvent m_OnPressPrimary = new ButtonEvent();

    [FormerlySerializedAs("onReleasePrimary")]
    [SerializeField]
    private ButtonEvent m_OnReleasePrimary = new ButtonEvent();

    [FormerlySerializedAs("onPressSecondary")]
    [SerializeField]
    private ButtonEvent m_OnPressSecondary = new ButtonEvent();

    [FormerlySerializedAs("onReleaseSecondary")]
    [SerializeField]
    private ButtonEvent m_OnReleaseSecondary = new ButtonEvent();

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<XRController>();
    }
    // Update is called once per frame
    bool lastStatePrimary = false;
    bool lastStateSecondary = false;
    void Update()
    {
        if (controller == null)
        {
            controller = this.GetComponent<XRController>();
        }
        bool pressed = false;
        controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out pressed);
        if(pressed!= lastStatePrimary)
        {
            if(pressed)
            {
                m_OnPressPrimary.Invoke();
            }
            else
            {
                m_OnReleasePrimary.Invoke();
            }
        }
        lastStatePrimary = pressed;

        pressed = false;
        controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed);
        if (pressed != lastStateSecondary)
        {
            if (pressed)
            {
                m_OnPressSecondary.Invoke();
            }
            else
            {
                m_OnReleaseSecondary.Invoke();
            }
        }
        lastStateSecondary= pressed;
    }

}
