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
    public GameObject left_controller;

    private GameManager game_manager;

    private HandInstrumentRight right_controller_script;
    private HandInstrumentLeft left_controller_script;

    private ADSR adsr_script;

    // Should these be doubles instead?
    public float[] frequencies;

    private void Awake()
    {
        game_manager = GameManager.Instance;

        right_controller_script = right_controller.GetComponent<HandInstrumentRight>();
        left_controller_script = left_controller.GetComponent<HandInstrumentLeft>();

        // ADSR script must be on the same game object as this oscillator script
        adsr_script = GetComponent<ADSR>();

        frequencies = new float[] { 220.0f, 233.1f, 246.9f, 261.6f, 277.2f, 293.7f, 311.1f, 329.6f, 349.2f, 370.0f, 392.0f, 415.3f,
        440.0f, 466.2f, 493.9f, 523.3f, 554.4f, 587.3f, 622.3f, 659.3f, 698.5f, 740.0f, 784.0f, 830.6f, 880.0f,
        932.3f, 987.8f, 1047f, 1109f, 1175f, 1245f, 1319f, 1397f, 1480f, 1568f, 1661f, 1760f };
    }

    private void Start()
    {
        AudioConfiguration config = AudioSettings.GetConfiguration();
        sampling_frequency = config.sampleRate;

        frequency = DetermineAndSetFrequency();
        freq_old = frequency;

        gain = 0;
        gain_old = 0;
    }

    // TODO: Consider using InvokeRepeating here instead, you can poll the controller status faster than 
    // with Update (I think!)
    private void Update()
    {
        frequency = DetermineAndSetFrequency();

        gain = maxVolume * GetGainFromADSR();

    }

    public float GetGainFromADSR()
    {
        return adsr_script.GetCurrentAmplitude();
    }

    private double DetermineAndSetFrequency()
    {
        float base_height = 1.0f;
        float height_increment = 0.1f;
        int num_levels = 9;
        int pitches_per_level = 4;

        HandInstrumentRight.PadDirection direction = right_controller_script.GetPadDirection();
        float right_height = right_controller_script.GetHeight();
 
        int right_level = Mathf.CeilToInt((right_height - base_height) / height_increment);

        return frequencies[Mathf.Clamp(right_level, 0, num_levels - 1) * pitches_per_level + (int)direction];
    }

    //public void SetGainFromSqueeze(float squeeze_single)
    //{
    //    gain = Mathf.Pow(squeeze_single, 3) * maxVolume;
    //}

    //public void GetGainFromXPosition(float XPosition)
    //{
    //    if (XPosition < -0.44 || XPosition > -0.4)
    //        gain = 0.2f;
    //    else
    //        gain = 0f;
    //}


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
}
