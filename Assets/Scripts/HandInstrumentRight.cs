using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandInstrumentRight : MonoBehaviour
{
    public enum PadDirection
    {
        PadDown = 0,
        PadLeft,
        PadUp,
        PadRight,
        NumDirections
    }

    public SteamVR_Action_Boolean m_GrabAction = null;
    public SteamVR_Action_Vector2 m_PadPosition = null;

    private SteamVR_Behaviour_Pose m_Pose = null;


    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Determine note being played
    }

    // Can probably optimize to only do the math if the direction has changed
    public PadDirection GetPadDirection()
    {
        Vector2 pad_pos = m_PadPosition.GetAxis(m_Pose.inputSource);

        float x = pad_pos.x;
        float y = pad_pos.y;

        PadDirection pad_dir;

        if (x >= y)
            if (x >= -y)
            {
                pad_dir = PadDirection.PadRight;
            }
            else
            {
                pad_dir = PadDirection.PadDown;
            }
        else
        {
            if (x >= -y)
            {
                pad_dir = PadDirection.PadUp;
            }
            else
            {
                pad_dir = PadDirection.PadLeft;
            }
        }

        return pad_dir;
    }

    public float GetHeight()
    {
        return transform.position.y;
    }
}
