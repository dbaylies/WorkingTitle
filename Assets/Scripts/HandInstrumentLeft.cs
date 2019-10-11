using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Communication scheme between scripts taken from here: https://www.youtube.com/watch?v=_4xNz23Wlfo

public class HandInstrumentLeft : MonoBehaviour
{
    private GameManager game_manager;

    public SteamVR_Action_Boolean m_GrabAction = null;
    public SteamVR_Action_Single m_SqueezeAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;

    public GameObject oscillator;
    private Oscillator oscillator_script;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();

        oscillator_script = oscillator.GetComponent<Oscillator>();

        game_manager = GameManager.Instance;

    }

    private void Update()
    {
        if (game_manager.play_mode == TriggerPlayMode.Continuous)
        {
            oscillator_script.SetGainFromSqueeze(m_SqueezeAction.GetAxis(m_Pose.inputSource));
        }
        else if (game_manager.play_mode == TriggerPlayMode.Pluck)
        {
            //Down
            if (m_GrabAction.GetStateDown(m_Pose.inputSource))
            {
                print(m_Pose.inputSource + " Trigger Down");
                oscillator_script.StartCoroutine("NoteOn");
            }
            //Up
            if (m_GrabAction.GetStateUp(m_Pose.inputSource))
            {
                print(m_Pose.inputSource + " Trigger Up");
                oscillator_script.StartCoroutine("NoteOff");
            }
        }
    }
}
