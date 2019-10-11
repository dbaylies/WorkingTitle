using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// From Andrew VR tutorial "SteamVR 2.0 Input using Actions": https://www.youtube.com/watch?v=bn8eMxBcI70
// Note that the tutorial uses SteamVR 2.0 which is substantially different from 2.2. You had to change some code.

public class ViveInput : MonoBehaviour
{
    //public SteamVR_Action_Single squeezeAction;
    public SteamVR_Action_Boolean grabPinchAction;

    //public SteamVR_Action_Vector2 touchPadAction;

    void Update()
    {
        // Several different ways to get button states

        //if (SteamVR_Actions._default.Teleport.GetStateDown(SteamVR_Input_Sources.Any))
        //{
        //    print("Teleport Down");
        //}

        // Obtuse way
        //if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.Any))
        //{
        //    print("Grab Pinch Up");
        //}

        // Simpler way, where you set the object from the Unity UI, rather than accessing it directly
        if (grabPinchAction.stateDown)
        {
            print("Grab Pinch Down");
        }

        //// Yet another way, being a bit more explicit about the input source
        //if (grabPinchAction.GetStateDown(SteamVR_Input_Sources.Any))
        //{
        //    print("Grab Pinch Down");
        //}


        // Getting other values

        //float triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        //if (triggerValue > 0.0f)
        //{
        //    print(triggerValue);
        //}

        //Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        //if (touchpadValue != Vector2.zero)
        //{
        //    print(touchpadValue);
        //}
    }


}
