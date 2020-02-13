using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller (left)")
        {
            SoundManager.Instance.TriggerHit();
            // GameManager.Instance.VibrateLeftController();
        }
    }
}
