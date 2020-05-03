using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSR : MonoBehaviour
{
    public float maxAttack;
    public float maxDecay;
    public float sustain;
    public float maxRelease;

    public float currentAmplitude;
    public float maxAmplitude;
    public float damp;

    void Start()
    {
        maxAttack = 0.2f;  // Seconds
        maxDecay = 0.2f;  // Seconds
        maxRelease = 0.5f;  // Seconds

        maxAmplitude = 1.0f;

        currentAmplitude = 0.0f;
        damp = 0.0f;
    }

    public float GetCurrentAmplitude()
    {
        return currentAmplitude;
    }

    public void SetDamp(float newDamp)
    {
        damp = Mathf.Clamp(newDamp, 0.05f, 1.0f);
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
        float attack_temp = (maxAttack * damp) * Mathf.Abs((1.0f - (startAmplitude / peakAmplitude)));
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
        for (float x = currentAmplitude; currentAmplitude > sustain; x -= (Time.deltaTime / (maxDecay * damp)) * (peakAmplitude - sustain))
        {
            currentAmplitude = Mathf.Max(x, sustain);
            yield return null;
        }

        // SUSTAIN (to be implemented?)
        // yield return new WaitForSeconds(2.0f);

        // RELEASE (should end up in NoteOff()))
        for (float x = sustain; currentAmplitude > 0; x -= (Time.deltaTime / (maxRelease * damp)) * sustain)
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
