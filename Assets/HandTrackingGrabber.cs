using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using System;

public class HandTrackingGrabber : OVRGrabber
{
    private OVRHand hand;
    private OVRSkeleton ske;
    public float treshold = .9f;

    public bool isAdjustedHandGrip = true;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hand = GetComponent<OVRHand>();
        ske = GetComponent<OVRSkeleton>();
        //ske.Freeze = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CheckPinch();
    }

     void CheckPinch()
    {

        List<OVRBoneCapsule> capsules = ske._capsules;
        List<float> distances = new List<float>();

        //Set default pinch strength
        float index = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        float middle = hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
        float ring = hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
        float pinky = hand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky);

        bool isIndex = index > treshold;
        bool isMiddle = middle > treshold;
        bool isRing = ring > treshold;
        bool isPinky = pinky > treshold;


        foreach (var capsule in capsules)
        {
            

            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(capsule.CapsuleCollider.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                
                distances.Add(hit.distance);
                if(hit.distance <= 0.01f)
                {
                    //Freeze index
                    if(capsule == capsules [7]  || capsule == capsules[8] || capsule == capsules[9])
                    {
                        isIndex = true;
                    }
                    //Freeze middle
                    if (capsule == capsules[10] || capsule == capsules[11] || capsule == capsules[12])
                    {
                        isMiddle = true;
                    }
                    //Freeze ring
                    if (capsule == capsules[13] || capsule == capsules[14] || capsule == capsules[15])
                    {
                        isMiddle = true;
                    }

                    
                }
            }
        }


        
        if (!m_grabbedObj && (isIndex || isMiddle || isRing) && m_grabCandidates.Count >0)
        {
            GrabBegin();
            //Set the frozen finger
            if(isAdjustedHandGrip)
            {
                ske.FreezeIndex = true;
                if (isMiddle)
                {
                    ske.FreezeMiddle = true;
                }
                //else
                //{
                //    ske.FreezeMiddle = false;
                //}

                if (isRing)
                {
                    ske.FreezeRing = true;
                }
                //else
                //{
                //    ske.FreezeRing = false;
                //}

                if (isPinky)
                {
                    ske.FreezePinky = true;
                }
                //else
                //{
                //    ske.FreezePinky = false;
                //}
            }



        }

        else if (m_grabbedObj && !(isIndex || isMiddle || isRing))
        {
            GrabEnd();
            ske.FreezeIndex = false;
            ske.FreezeMiddle = false;
            ske.FreezeRing = false;
            ske.FreezePinky = false;
        }
            
    }
}
