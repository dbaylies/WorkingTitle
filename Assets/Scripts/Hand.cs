﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Transcribed from Andrew VR tutorial: https://www.youtube.com/watch?v=HnzmnSqE-Bc&t=72s

public class Hand : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_contactInteractables = new List<Interactable>();

    public bool triggerDown;


    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            triggerDown = true;
            PickUp();
        }
        //Up
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            triggerDown = false;
            Drop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;


        m_contactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;


        m_contactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void PickUp()
    {
        // Get nearest Interactable
        m_CurrentInteractable = GetNearestInteractable();

        // Null Check
        if(!m_CurrentInteractable)
            return;

        // Already Held Check
        if (m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();

        // Position
        m_CurrentInteractable.transform.position = transform.position;

        // Attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        // Set Active Active Hand
        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
        // Null Check
        if (!m_CurrentInteractable)
            return;

        // Apply Velocity
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        // Detach
        m_Joint.connectedBody = null;

        // Clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach(Interactable interactable in m_contactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        } 

        return nearest;
    }
}
