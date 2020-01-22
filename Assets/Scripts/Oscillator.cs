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

    // Should these be doubles instead?
    public float[] frequencies;

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

        frequency = DetermineAndSetFrequency();
        freq_old = frequency;

        gain = 0;
        gain_old = 0;

        frequencies = new float[] { 220.0f, 233.1f, 246.9f, 261.6f, 277.2f, 293.7f, 311.1f, 329.6f, 349.2f, 370.0f, 392.0f, 415.3f,
        440.0f, 466.2f, 493.9f, 523.3f, 554.4f, 587.3f, 622.3f, 659.3f, 698.5f, 740.0f, 784.0f, 830.6f, 880.0f,
        932.3f, 987.8f, 1047f, 1109f, 1175f, 1245f, 1319f, 1397f, 1480f, 1568f, 1661f, 1760f };
    }

    // TODO: Consider using InvokeRepeating here instead, you can poll the controller status faster than 
    // with Update (I think!)
    private void Update()
    {
        frequency = DetermineAndSetFrequency();

        if (game_manager.play_mode == TriggerPlayMode.Continuous)
        {
            // Would ideally update this every time OnAudioFilterRead is called, but doesn't seem to be callable in audio thread
            // SetGainFromSqueeze(left_controller_script.GetSqueeze());
            SetGainFromXPosition(left_controller_script.GetXPosition());

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

    private double DetermineAndSetFrequency()
    {
        HandInstrumentRight.PadDirection direction = right_controller_script.GetPadDirection();

        float right_height = right_controller_script.GetHeight();

        float pad_freq = 70;

        if (right_height <= 1.0)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[0];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[1];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[2];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[3];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.1)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[4];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[5];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[6];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[7];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.2)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[8];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[9];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[10];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[11];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.3)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[12];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[13];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[14];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[15];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.4)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[16];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[17];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[18];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[19];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.5)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[20];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[21];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[22];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[23];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.6)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[24];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[25];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[26];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[27];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else if (right_height <= 1.7)
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[28];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[29];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[30];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[31];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        else
        {
            switch (direction)
            {
                case HandInstrumentRight.PadDirection.PadDown:
                    pad_freq = frequencies[32];
                    break;
                case HandInstrumentRight.PadDirection.PadLeft:
                    pad_freq = frequencies[33];
                    break;
                case HandInstrumentRight.PadDirection.PadUp:
                    pad_freq = frequencies[34];
                    break;
                case HandInstrumentRight.PadDirection.PadRight:
                    pad_freq = frequencies[35];
                    break;
                default:
                    pad_freq = 70;
                    break;
            }
        }

        return pad_freq;
    }

    public void SetGainFromSqueeze(float squeeze_single)
    {
        gain = Mathf.Pow(squeeze_single, 3) * maxVolume;
    }

    public void SetGainFromXPosition(float XPosition)
    {
        if (XPosition < -0.44 || XPosition > -0.4)
            gain = 0.2f;
        else
            gain = 0f;
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
