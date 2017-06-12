using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class CameraController : MonoBehaviour
{

    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreGuiFingers = true;

    [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
    public int RequiredFingerCount;

    [Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
    [Range(-1.0f, 1.0f)]
    public float WheelSensitivity;

    [Tooltip("The current FOV/Size")]
    public float Zoom;

    [Tooltip("Limit the FOV/Size?")]
    public bool ZoomClamp;

    [Tooltip("The minimum FOV/Size we want to zoom to")]
    public float ZoomMin = 4.0f;

    [Tooltip("The maximum FOV/Size we want to zoom to")]
    public float ZoomMax = 15.0f;

    [Tooltip("How quickly the zoom reaches the target value")]
    public float Dampening = 10.0f;

    private float currentZoom;

    protected virtual void OnEnable()
    {
        Zoom = transform.localPosition.magnitude;
        currentZoom = Zoom;
    }

    protected virtual void LateUpdate()
    {
        // Get the fingers we want to use
        var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

        // Get the pinch ratio of these fingers
        var pinchRatio = LeanGesture.GetPinchRatio(fingers, WheelSensitivity);

        // Modify the zoom value
        Zoom *= pinchRatio;

        if (ZoomClamp == true)
        {
            Zoom = Mathf.Clamp(Zoom, ZoomMin, ZoomMax);
        }

        // Set the new zoom
        //SetZoom(Zoom);

        // Get t value
        var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);

        // Lerp the current values to the target ones
        currentZoom = Mathf.Lerp(currentZoom, Zoom, factor);

        // Set the new zoom
        SetZoom(currentZoom);
    }

    protected void SetZoom(float current)
    {
        //Vector3 dir = transform.forward;
        transform.localPosition  = new Vector3(0,0,-1) * current;
    }
}
