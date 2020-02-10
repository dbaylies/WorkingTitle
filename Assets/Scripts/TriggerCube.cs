using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "controller")
        {
            SoundManager.Instance.TriggerHit();
            GameManager.Instance.VibrateLeftController();
        }
    }
}
