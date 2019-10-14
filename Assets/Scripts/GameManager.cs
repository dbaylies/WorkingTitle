using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerPlayMode
{
    Continuous = 0,
    Pluck = 1
}

public class GameManager : Singleton<GameManager>
{
    // Define and deal with game-wide variables here, like settings
    public TriggerPlayMode play_mode;

    // Start is called before the first frame update
    void Start()
    {
        play_mode = TriggerPlayMode.Pluck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
