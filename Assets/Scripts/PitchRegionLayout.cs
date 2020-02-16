using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class PitchRegionLayout : MonoBehaviour
{
    public float base_height;
    public float height_increment;

    // Start is called before the first frame update
    void Start()
    {
        base_height = 1.2f;
        height_increment = 0.06f;
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            Vector3 childPos = child.transform.position;
            childPos.y = (base_height - height_increment / 2) + height_increment * i;
            child.transform.position = childPos;
            ++i;
        }
    }
}
