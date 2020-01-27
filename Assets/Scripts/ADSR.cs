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

    private bool triggerDown;

    // Start is called before the first frame update
    void Start()
    {
        attack = 0.2f;  // Seconds
        decay = 0.3f;  // Seconds
        sustain = 0.6f;  // Amplitude
        release = 0.5f;  // Seconds
    }

    public float GetCurrentAmplitude()
    {
        return currentAmplitude;
    }

    void HoldAmplitude()
    {

    }

    IEnumerator NoteOn()
    {
        Debug.Log("Test");
        // Peak amplitude should be determined by some kind of velocity parameter
        for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime / attack)
        {
            currentAmplitude = x;
            yield return null;
        }

        currentAmplitude = 1.0f;

        for (float x = currentAmplitude; x >= sustain; x -= (Time.deltaTime / decay) * (1.0f - sustain))
        {
            currentAmplitude = x;
            yield return null;
        }

        currentAmplitude = sustain;

        // Amt of time to sustain
        // yield return new WaitForSeconds(2.0f);

        // The following should end up in NoteOff()

        for (float x = sustain; x >= 0; x -= (Time.deltaTime / release) * (sustain))
        {
            currentAmplitude = x;
            yield return null;
        }

        // Should be a way to more smoothly end up at zero from the above code than to just force it here
        // Better math likely. Same for getting to sustain level and peak amplitude level
        currentAmplitude = 0;
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
