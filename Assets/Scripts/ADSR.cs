using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSR : MonoBehaviour
{

    public float attack;  // Seconds
    public float decay;  // Seconds
    public float sustain;  // Amplitude
    public float release;  // Seconds

    public float currentAmplitude;

    private bool triggerDown;

    // Start is called before the first frame update
    void Start()
    {
        attack = 0.1f;
        decay = 0.2f;
        sustain = 0.6f;
        release = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetCurrentAmplitude()
    {
        return currentAmplitude;
    }

    void HoldAmplitude()
    {

    }

    public void TriggerHit()
    {
        StartCoroutine("NoteOn");
    }

    IEnumerator NoteOn()
    {
        for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime / decay)
        {
            currentAmplitude = x;
            yield return null;// new WaitForEndOfFrame();
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
