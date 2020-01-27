using System.Collections;
using UnityEngine;

// Transcribed significantly from this Youtube tutorial: https://www.youtube.com/watch?v=GqHFGMy_51c
// Communication scheme between scripts taken from here: https://www.youtube.com/watch?v=_4xNz23Wlfo

public class Oscillator : MonoBehaviour
{
    public double frequency;
    public float gain;

    private double phase = 0;
    private double sampling_frequency;
    public float max_volume = 0.2f; // What should there be? What is the expected normal gainstaging for a Vive user?

    // OnAudioFilterRead variables
    private int num_frames;
    private float delta_gain;
    private double delta_freq;
    public float gain_old;
    private float gain_new;
    public double freq_old;
    private double freq_new;

    private bool is_on;


    private void Start()
    {
        AudioConfiguration config = AudioSettings.GetConfiguration();
        sampling_frequency = config.sampleRate;

        gain = 0;
        gain_old = 0;
    }

    public void TurnOn()
    {
        is_on = true;
    }

    public void TurnOff()
    {
        is_on = false;
    }

    public void SetVolumeAndFrequency(float volume, float freq)
    {
        gain = volume;
        frequency = freq;
    }

    // This function is called on the audio thread so we don't have access to many Unity functions unfortunately
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (is_on)
        {
            num_frames = data.Length / channels;

            gain_new = gain;
            freq_new = frequency;

            delta_gain = (gain_new - gain_old) / num_frames;
            delta_freq = (freq_new - freq_old) / num_frames;

            for (int i = 0; i < data.Length; i += channels)
            {
                phase += (freq_old + (delta_freq * i / channels)) * 2.0 * Mathf.PI / sampling_frequency;

                if (phase > Mathf.PI * 2)
                {
                    phase = phase - Mathf.PI * 2;
                }

                data[i] = (gain_old + (delta_gain * i / channels)) * Mathf.Sin((float)phase);

                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }
            }

            gain_old = gain_new;
            freq_old = freq_new;
        }
    }
}
