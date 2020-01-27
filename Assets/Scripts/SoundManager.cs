using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TriggerHit()
    {
        NoteOn();
    }

    private void NoteOn()
    {
        GameObject.Find("Sphere").GetComponent<ADSR>().TriggerHit();
    }
}
