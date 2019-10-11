﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Transcribed significantly from this Youtube tutorial: https://www.youtube.com/watch?v=GqHFGMy_51c
// Communication scheme between scripts taken from here: https://www.youtube.com/watch?v=_4xNz23Wlfo

public class Oscillator : MonoBehaviour
{
    public double frequency;
    public double frequency_a;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;
    public float maxVolume = 0.2f; // What should there be? What is the expected normal gainstaging for a Vive user?

    public float gain = 0;
    public float gain_a = 0;

    public GameObject right_controller;

    // public float[] frequencies;
    // public int thisfreq;

    private void Start()
    {
        
    }

    private void Update()
    {
        frequency = (right_controller.transform.position.y - 0.8) * 400 + 440; // To be rewritten...
    }

    public void SetGainFromSqueeze(float squeeze_single)
    {
        gain = Mathf.Pow(squeeze_single, 3) * maxVolume;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        // Should only be doing these calculations when absolutely necessary

        // Gain must change at "OnAudioFilterRead" rate to avoid clicks... except there are still clicks
        // Should be incrementing every SAMPLE, not each time OnAudioFilterRead is called! That's only every 20ms or so.
        if (gain_a < gain)
        {
            gain_a += (float)0.01;
            if (gain_a > gain)
            {
                gain_a = gain;
            }
        }
        else if (gain_a > gain)
        {
            gain_a -= (float)0.01;
            if (gain_a < gain)
            {
                gain_a = gain;
            }
        }

        // frequency too
        if (frequency_a < frequency)
        {
            frequency_a += (float)0.1;
            if (frequency_a > frequency)
            {
                frequency_a = frequency;
            }
        }
        else if (frequency_a > frequency)
        {
            frequency_a -= (float)0.1;
            if (frequency_a < frequency)
            {
                frequency_a = frequency;
            }
        }

        increment = frequency * 2.0 * Mathf.PI / sampling_frequency; // Should be frequency_a

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            if (phase > Mathf.PI * 2)
            {
                phase = phase - Mathf.PI * 2;
            }

            data[i] = (float)(gain_a * Mathf.Sin((float)phase));

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
        }
    }

    IEnumerator NoteOn()
    {
        float SecondsToFade = 0.2f;
        float startVol = gain;
        float targetVol = 0.1f;

        for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime / SecondsToFade)
        {
            print("hm");
            gain = Mathf.Lerp(startVol, targetVol, x);
            yield return null;// new WaitForEndOfFrame();
        }

        gain = targetVol;
    }


    IEnumerator NoteOff()
    {
        float SecondsToFade = 0.2f;
        float startVol = gain;
        float targetVol = 0.0f;

        for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime / SecondsToFade)
        {
            gain = Mathf.Lerp(startVol, targetVol, x);
            yield return null;// new WaitForEndOfFrame();
        }

        gain = targetVol;
    }
}
