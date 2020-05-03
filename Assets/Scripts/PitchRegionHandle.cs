using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRegionHandle : MonoBehaviour
{
    private void Awake()
    {
        SetAlpha(0.3f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller (right)")
        {
            GameManager.Instance.VibrateRightController();
            SetAlpha(0.8f);
            StartCoroutine("TriggerEntered", other);
        }
    }

    IEnumerator TriggerEntered(Collider other)
    {
        while (true)
        {
            if (SoundManager.Instance.IsRightTriggerDown())
            {
                SoundManager.Instance.SetPitchLevelHeight(other.transform.position.y);
                Vector3 handlePos = transform.position;
                handlePos.y = other.transform.position.y;
                transform.position = handlePos;
            }

            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SetAlpha(0.3f);
        StopCoroutine("TriggerEntered");
    }

    private void SetAlpha(float alpha)
    {
        Color regionColor = GetComponent<Renderer>().material.color;
        regionColor.a = alpha;
        GetComponent<Renderer>().material.color = regionColor;
    }
}
