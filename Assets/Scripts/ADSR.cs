using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSR : MonoBehaviour
{
    public float attack;
    public float decay;
    public float sustain;
    public float release;

    public float currentAmplitude;
    public float maxAmplitude;

    void Start()
    {
        attack = 0.2f;  // Seconds
        decay = 0.3f;  // Seconds
        //sustain = 0.6f;  // Amplitude
        release = 0.5f;  // Seconds

        maxAmplitude = 1.0f;
        currentAmplitude = 0.0f;
    }

    public float GetCurrentAmplitude()
    {
        return currentAmplitude;
    }

    void HoldAmplitude()
    {

    }

    // Give parameters as arguments
    public IEnumerator NoteOn(float peakAmplitude)
    {
        float startAmplitude = currentAmplitude;
        sustain = peakAmplitude * 0.9f;
        // ATTACK
        // Attack faster if we aren't starting from 0
        float attack_temp = attack * Mathf.Abs((1.0f - (startAmplitude / peakAmplitude)));
        if (startAmplitude < peakAmplitude)
            for (float x = startAmplitude; currentAmplitude < peakAmplitude; x += (Time.deltaTime / attack_temp) * (peakAmplitude - startAmplitude))
            {
                currentAmplitude = Mathf.Min(x, peakAmplitude);
                yield return null;
            }
        else
        {
            for (float x = startAmplitude; currentAmplitude > peakAmplitude; x -= (Time.deltaTime / attack_temp) * (startAmplitude - peakAmplitude))
            {
                currentAmplitude = Mathf.Max(x, peakAmplitude);
                yield return null;
            }
        }

        // DECAY
        for (float x = currentAmplitude; currentAmplitude > sustain; x -= (Time.deltaTime / decay) * (peakAmplitude - sustain))
        {
            currentAmplitude = Mathf.Max(x, sustain);
            yield return null;
        }

        // SUSTAIN (to be implemented?)
        // yield return new WaitForSeconds(2.0f);

        // RELEASE (should end up in NoteOff()))
        for (float x = sustain; currentAmplitude > 0; x -= (Time.deltaTime / release) * (sustain))
        {
            currentAmplitude = Mathf.Max(x, 0.0f);
            yield return null;
        }
    }


    //IEnumerator NoteOff()
    //{
    //    float SecondsToFade = 0.2f;
    //    float startVol = gain;
    //    float targetVol = 0.0f;

    //    for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime / SecondsToFade)
    //    {
    //        gain = Mathf.Lerp(startVol, targetVol, x);
    //        yield return null;// new WaitForEndOfFrame();
    //    }

    //    gain = targetVol;
    //}
}
