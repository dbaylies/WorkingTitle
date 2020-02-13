using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{


    // Define and deal with game-wide variables here, like settings

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void VibrateLeftController()
    {
        GameObject.Find("Controller (left)").GetComponent<HandInstrumentLeft>().Vibrate();
    }

    public void VibrateRightController()
    {
        GameObject.Find("Controller (right)").GetComponent<HandInstrumentRight>().Vibrate();
    }
}
