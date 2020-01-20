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

    // public float[] frequencies;
    // public int thisfreq;

    private void Awake()
    {
        game_manager = GameManager.Instance;

        right_controller_script = right_controller.GetComponent<HandInstrumentRight>();
        left_controller_script = left_controller.GetComponent<HandInstrumentLeft>();
    }

    private void Start()
    {
        AudioConfiguration config = AudioSettings.GetConfiguration();
        sampling_frequency = config.sampleRate;

        frequency = SetFrequencyFromRightPad();
        freq_old = frequency;

        gain = 0;
        gain_old = 0;

    }

    // TODO: Consider using InvokeRepeating here instead, you can poll the controller status faster than 
    // with Update (I think!)
    private void Update()
    {
        frequency = SetFrequencyFromRightPad();

        if (game_manager.play_mode == TriggerPlayMode.Continuous)
        {
            // Would ideally update this every time OnAudioFilterRead is called, but doesn't seem to be callable in audio thread
            SetGainFromSqueeze(left_controller_script.GetSqueeze());
        }
        else if (game_manager.play_mode == TriggerPlayMode.Pluck)
        {
            if (left_controller_script.GetTriggerStateDown())
            {
                StartCoroutine("NoteOn");
            }
            if (left_controller_script.GetTriggerStateUp())
            {
                StartCoroutine("NoteOff");
            }
        }

    }

    private double SetFrequencyFromRightPad()
    {
        HandInstrumentRight.PadDirection direction = right_controller_script.GetPadDirection();

        // Default if no direction selected
        float pad_freq;

        switch (direction)
        {
            case HandInstrumentRight.PadDirection.PadDown:
                pad_freq = 500;
                break;
            case HandInstrumentRight.PadDirection.PadLeft:
                pad_freq = 600;
                break;
            case HandInstrumentRight.PadDirection.PadUp:
                pad_freq = 700;
                break;
            case HandInstrumentRight.PadDirection.PadRight:
                pad_freq = 800;
                break;
            default:
                pad_freq = 70;
                break;
        }

        return pad_freq;
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
