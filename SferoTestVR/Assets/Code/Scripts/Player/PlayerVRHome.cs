using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerVRHome : Player
{
    //vr variables
    InputDevice rightController, leftController;
    bool gotDevices = false;
    [SerializeField]GameObject rightHand;


    protected override void Start()
    {
        base.Start();

        //vr setup
        StartCoroutine(setupVR());
    }

    /// <summary>
    /// gets input from controllers
    /// at the moment check only right controller
    /// 
    /// should it allow 2 controller input?
    /// </summary>
    protected override void GetInput()
    {
        Vector2 walkInput;
        //check knob value
        if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondary2DAxis, out walkInput))
        {
            if (walkInput.magnitude > 0.2)
            {
                Vector3 direction = rightHand.transform.forward * walkInput.y + rightHand.transform.right * walkInput.x;
                float length = direction.magnitude;
                direction.y = 0;
                direction *= length/direction.magnitude;
                Move(direction * moveSpeed);
            }                        
        }        
    }


    IEnumerator setupVR()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();

        //loop for waiting to have all 3 devices (head, and 2 controllers)
        while (!gotDevices)
        {

            UnityEngine.XR.InputDevices.GetDevices(inputDevices);
            if (inputDevices.Count < 3)
            {
                yield return null;
            }
            else
            {
                gotDevices = true;
            }
        }

        //if got them all
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        //get right controller
        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandedControllers);
        rightController = rightHandedControllers[0];

        //get left controller
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);
        leftController = leftHandedControllers[0];

    }
}
