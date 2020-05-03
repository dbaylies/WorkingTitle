using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // Should these be doubles instead?
    public float[] frequencies;

    public PitchRegionLayout pitchLevels;

    private ADSR adsr;
    private Oscillator oscillator;
    private HandInstrumentRight rightController;
    private HandInstrumentLeft leftController;
    private Hand rightControllerHand;

    private void Start()
    {
        frequencies = new float[] { 220.0f, 233.1f, 246.9f, 261.6f, 277.2f, 293.7f, 311.1f, 329.6f, 349.2f, 370.0f, 392.0f, 415.3f,
        440.0f, 466.2f, 493.9f, 523.3f, 554.4f, 587.3f, 622.3f, 659.3f, 698.5f, 740.0f, 784.0f, 830.6f, 880.0f,
        932.3f, 987.8f, 1047f, 1109f, 1175f, 1245f, 1319f, 1397f, 1480f, 1568f, 1661f, 1760f };

        adsr = GetComponent<ADSR>();
        oscillator = GetComponent<Oscillator>();
        rightController = GameObject.Find("Controller (right)").GetComponent<HandInstrumentRight>();
        leftController = GameObject.Find("Controller (left)").GetComponent<HandInstrumentLeft>();
        rightControllerHand = GameObject.Find("Controller (right)").GetComponent<Hand>();
        pitchLevels = GameObject.Find("Pitch Levels").GetComponent<PitchRegionLayout>();

    }

    // TODO: Consider using InvokeRepeating here instead, you can poll the controller status faster than 
    // with coroutines (I think!)
    public void TriggerHit()
    {
        adsr.StopCoroutine("NoteOn");

        float leftVelocity = leftController.GetXSpeed();
        // Measured approx min 0.01, max 5.5

        // Arctangent is a good mapping function here
        float peakAmplitude = Mathf.Atan(leftVelocity * (10f/5.5f)) / (Mathf.PI / 2);

        adsr.StartCoroutine("NoteOn", peakAmplitude);

        StopCoroutine("NoteOn");
        StartCoroutine("NoteOn");
    }

    IEnumerator NoteOn()
    {
        float freq = DetermineFrequency();

        oscillator.TurnOn();
        do
        {
            //Trigger maxes at whole tone down
            oscillator.SetVolumeAndFrequency(adsr.GetCurrentAmplitude(), Mathf.Lerp(freq, (float)(freq * 0.89), rightController.GetSqueeze()));
            yield return null;
        }
        while ( adsr.GetCurrentAmplitude() > 0 );
        oscillator.TurnOff();
    }

    private float DetermineFrequency()
    {
        float base_height = pitchLevels.base_height;
        float height_increment = pitchLevels.height_increment;
        int num_levels = pitchLevels.gameObject.transform.childCount;
        int pitches_per_level = 4;

        HandInstrumentRight.PadDirection direction = rightController.GetPadDirection();
        float right_height = rightController.GetHeight();

        int right_level = Mathf.CeilToInt((right_height - base_height) / height_increment);

        return frequencies[Mathf.Clamp(right_level, 0, num_levels - 1) * pitches_per_level + (int)direction];
    }

    public void SetPitchLevelHeight(float baseHeight)
    {
        pitchLevels.base_height = baseHeight;
    }

    public bool IsRightTriggerDown()
    {
        return rightControllerHand.triggerDown;
    }

}
