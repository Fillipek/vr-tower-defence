using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OnlyYAxisDeviceBasedContinuousMoveProvider : ContinuousMoveProviderBase
{
    public enum InputAxes
    {
        Primary2DAxis = 0,
        Secondary2DAxis = 1,
    }

    [SerializeField]
    [Tooltip("The 2D Input Axis on the controller devices that will be used to trigger a move.")]
    InputAxes m_InputBinding = InputAxes.Primary2DAxis;
    public InputAxes inputBinding
    {
        get => m_InputBinding;
        set => m_InputBinding = value;
    }

    [SerializeField]
    [Tooltip("A list of controllers that allow move.  If an XRController is not enabled, or does not have input actions enabled, move will not work.")]
    List<XRBaseController> m_Controllers = new List<XRBaseController>();
    public List<XRBaseController> controllers
    {
        get => m_Controllers;
        set => m_Controllers = value;
    }

    [SerializeField]
    [Tooltip("Value below which input values will be clamped. After clamping, values will be renormalized to [0, 1] between min and max.")]
    float m_DeadzoneMin = 0.125f;
    public float deadzoneMin
    {
        get => m_DeadzoneMin;
        set => m_DeadzoneMin = value;
    }

    [SerializeField]
    [Tooltip("Value above which input values will be clamped. After clamping, values will be renormalized to [0, 1] between min and max.")]
    float m_DeadzoneMax = 0.925f;
    public float deadzoneMax
    {
        get => m_DeadzoneMax;
        set => m_DeadzoneMax = value;
    }

    static readonly InputFeatureUsage<Vector2>[] k_Vec2UsageList =
    {
        CommonUsages.primary2DAxis,
        CommonUsages.secondary2DAxis,
    };

    /// <inheritdoc />
    protected override Vector2 ReadInput()
    {
        if (m_Controllers.Count == 0)
            return Vector2.zero;

        // Accumulate all the controller inputs
        var input = Vector2.zero;
        var feature = k_Vec2UsageList[(int)m_InputBinding];
        for (var i = 0; i < m_Controllers.Count; ++i)
        {
            var controller = m_Controllers[i] as XRController;
            if (controller != null &&
                controller.enableInputActions &&
                controller.inputDevice.TryGetFeatureValue(feature, out var controllerInput))
            {
                input += GetDeadzoneAdjustedValue(controllerInput);
                   
            }
        }
        input.x = 0;
        return input;
    }

    protected Vector2 GetDeadzoneAdjustedValue(Vector2 value)
    {
        var magnitude = value.magnitude;
        var newMagnitude = GetDeadzoneAdjustedValue(magnitude);
        if (Mathf.Approximately(newMagnitude, 0f))
            value = Vector2.zero;
        else
            value *= newMagnitude / magnitude;
        return value;
    }

    protected float GetDeadzoneAdjustedValue(float value)
    {
        var min = m_DeadzoneMin;
        var max = m_DeadzoneMax;

        var absValue = Mathf.Abs(value);
        if (absValue < min)
            return 0f;
        if (absValue > max)
            return Mathf.Sign(value);

        return Mathf.Sign(value) * ((absValue - min) / (max - min));
    }
}
