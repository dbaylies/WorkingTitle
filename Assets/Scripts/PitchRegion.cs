using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRegion : MonoBehaviour
{
    private void Awake()
    {
        SetAlpha(0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Controller (right)")
        {
            GameManager.Instance.VibrateRightController();
            SetAlpha(0.8f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SetAlpha(0.1f);
    }

    private void SetAlpha(float alpha)
    {
        Color regionColor = GetComponent<Renderer>().material.color;
        regionColor.a = alpha;
        GetComponent<Renderer>().material.color = regionColor;
    }
}
