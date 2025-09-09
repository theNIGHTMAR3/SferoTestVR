using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;

/// <summary>
/// When tracking is lost, this script will rotate the camera in player
/// moving direction. It is supposed to be attached to the camera script
/// </summary>
public class CameraLostTrackingOverride : MonoBehaviour
{
    [SerializeField]
    Rigidbody playerRB;

    /// <summary>
    /// Direction to use when player velocity would be 0
    /// </summary>
    Vector3 lastSavedDirection;
    bool trackingLost = false;
        

    // Update is called once per frame
    void Update()
    {
        if(playerRB.velocity != Vector3.zero)
        {
            lastSavedDirection = playerRB.velocity;
        }

        trackingLost = !HeadsetProperlyTracked();

        if (trackingLost)
        {
            OverrideCamera();
        }
    }


    bool HeadsetProperlyTracked()
    {
        // Get headset device
        InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

        if (!headDevice.isValid)
        {
            Debug.Log("Headset disconnected");
            return false;
        }

        // Check if device reports being tracked
        if (headDevice.TryGetFeatureValue(CommonUsages.isTracked, out bool isTracked))
        {
            if (!isTracked)            
            {
                Debug.Log("Headset not tracked");
                return false;
            }
        }


        if(headDevice.TryGetFeatureValue(CommonUsages.trackingState, out InputTrackingState trackingState))
        {
            // trackingState is a bitmask
            bool hasPositiona = (trackingState & InputTrackingState.Position) != 0;
            bool hasRotationa = (trackingState & InputTrackingState.Rotation) != 0;
            var a = 2;
        }             


        // other checks

        // Try to get rotation
        bool hasRotation = headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        // Try to get position
        bool hasPosition = headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);

        if (hasRotation && hasPosition)
        {
            Debug.Log("Headset has position + rotation tracking");
            return true;
        }
        else if (hasRotation && !hasPosition)
        {
            Debug.Log("Headset has rotation tracking, but NO position tracking (orientation-only mode)");
            return false;
        }
        else
        {
            Debug.Log("No rotation or position tracking");
            return false;
        }        
    }

    void OverrideCamera()
    {
        // set camera position
        transform.position = playerRB.transform.position;

        /// rotate camera
        transform.LookAt(transform.position + lastSavedDirection);
    }

}
