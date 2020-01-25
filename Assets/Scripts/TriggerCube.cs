using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCube : MonoBehaviour
{
    public GameObject adsr_holder;

    public ADSR adsr_script;

    // Start is called before the first frame update
    void Start()
    {
        adsr_script = adsr_holder.GetComponent<ADSR>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.GetType());
        if (other.gameObject.tag == "controller")
        {
            adsr_script.TriggerHit();
        }
    }
}
