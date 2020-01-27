using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    public void TriggerHit()
    {
        // TODO: Consider using InvokeRepeating here instead, you can poll the controller status faster than 
        // with coroutines (I think!)
        StopCoroutine("NoteOn");
        StartCoroutine("NoteOn");
    }

    IEnumerator NoteOn()
    {
        ADSR adsr = GameObject.Find("Sphere").GetComponent<ADSR>();
        Oscillator oscillator = GameObject.Find("Sphere").GetComponent<Oscillator>();

        adsr.StartCoroutine("NoteOn");

        oscillator.TurnOn();
        do
        {
            oscillator.SetVolumeAndFrequency(adsr.GetCurrentAmplitude(), 440f);
            yield return null;
        }
        while ( adsr.GetCurrentAmplitude() > 0 );
        oscillator.TurnOff();
    }


}
