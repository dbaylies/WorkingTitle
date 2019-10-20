using System.Collections;
using UnityEngine;

// Transcribed significantly from this Youtube tutorial: https://www.youtube.com/watch?v=GqHFGMy_51c
// Communication scheme between scripts taken from here: https://www.youtube.com/watch?v=_4xNz23Wlfo

public class Oscillator : MonoBehaviour
{
    public double frequency;
    public double freq_old;
    public float gain;
    public float gain_old;

    private double phase = 0;
    private double sampling_frequency;
    public float maxVolume = 0.2f; // What should there be? What is the expected normal gainstaging for a Vive user?

    public GameObject right_controller;

    // public float[] frequencies;
    // public int thisfreq;

    private void Start()
    {
        AudioConfiguration config = AudioSettings.GetConfiguration();
        sampling_frequency = config.sampleRate;

        frequency = GetFrequency();
        freq_old = frequency;

        gain = 0;
        gain_old = 0;

    }

    private void Update()
    {
        frequency = GetFrequency();
    }

    private double GetFrequency()
    {
        return (right_controller.transform.position.y - 0.8) * 400 + 440; // To be rewritten...
    }

    public void SetGainFromSqueeze(float squeeze_single)
    {
        gain = Mathf.Pow(squeeze_single, 3) * maxVolume;
    }

    // This function is called on the audio thread so we don't have access to many Unity functions unfortunately
    private void OnAudioFilterRead(float[] data, int channels)
    {
        int num_frames = data.Length/channels;

        float delta_gain;
        double delta_freq;

        float gain_new = gain;
        double freq_new = frequency;

        delta_gain = (gain_new - gain_old) / num_frames;
        delta_freq = (freq_new - freq_old) / num_frames;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += (freq_old + (delta_freq * i/channels)) * 2.0 * Mathf.PI / sampling_frequency;

            if (phase > Mathf.PI * 2)
            {
                phase = phase - Mathf.PI * 2;
            }

            data[i] = (gain_old + (delta_gain * i/channels)) * Mathf.Sin((float)phase);

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
        }

        gain_old = gain_new;
        freq_old = freq_new;
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
