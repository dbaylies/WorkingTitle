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
        sustain = 0.6f;  // Amplitude
        release = 0.5f;  // Seconds

        maxAmplitude = 1.0f;
    }

    public float GetCurrentAmplitude()
    {
        return currentAmplitude;
    }

    void HoldAmplitude()
    {

    }

    // Give parameters as arguments
    IEnumerator NoteOn(float startAmplitude)
    {
        // ATTACK
        // Attack faster if we aren't starting from 0
        float attack_temp = attack * (1.0f - (startAmplitude / maxAmplitude));
        // Peak amplitude should be determined by some kind of velocity parameter
        for (float x = startAmplitude; currentAmplitude < maxAmplitude; x += (Time.deltaTime / attack_temp) * (maxAmplitude - startAmplitude))
        {
            currentAmplitude = Mathf.Min(x, maxAmplitude);
            yield return null;
        }

        // DECAY
        for (float x = currentAmplitude; currentAmplitude > sustain; x -= (Time.deltaTime / decay) * (maxAmplitude - sustain))
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
