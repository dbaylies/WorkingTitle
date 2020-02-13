using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRegion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller (right)")
        {
            // SoundManager.Instance.PitchChanged(); ?
            GameManager.Instance.VibrateRightController();
        }
    }
}
