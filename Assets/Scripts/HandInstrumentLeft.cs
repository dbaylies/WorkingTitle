using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandInstrumentLeft : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;
    public SteamVR_Action_Single m_SqueezeAction = null;
    public SteamVR_Action_Vibration m_Vibration = null;

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

    public float GetXSpeed()
    {
        return Mathf.Abs(m_Pose.GetVelocity().x);
    }

    public void Vibrate()
    {
        m_Vibration.Execute(0.0f, 0.01f, 100.0f, 0.5f, Valve.VR.SteamVR_Input_Sources.LeftHand);
    }
}
