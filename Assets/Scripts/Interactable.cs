using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Transcribed from Andrew VR tutorial: https://www.youtube.com/watch?v=HnzmnSqE-Bc&t=72s

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public Hand m_ActiveHand = null;
}
