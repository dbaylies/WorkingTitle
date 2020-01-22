using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Communication scheme between scripts taken from here: https://www.youtube.com/watch?v=_4xNz23Wlfo

public class HandInstrumentLeft : MonoBehaviour
{

    public SteamVR_Action_Boolean m_GrabAction = null;
    public SteamVR_Action_Single m_SqueezeAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
    }

    public float GetSqueeze()
    {
        return m_SqueezeAction.GetAxis(m_Pose.inputSource);
    }

    public bool GetTriggerStateDown()
    {
        return m_GrabAction.GetStateDown(m_Pose.inputSource);
    }

    public bool GetTriggerStateUp()
    {
        return m_GrabAction.GetStateUp(m_Pose.inputSource);
    }

    public float GetXPosition()
    {
        return transform.position.x;
    }

}
