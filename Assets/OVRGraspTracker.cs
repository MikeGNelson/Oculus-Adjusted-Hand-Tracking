using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OVRSkeleton))]
public class OVRGraspTracker : MonoBehaviour
{
    private OVRSkeleton skeleton;
    private OVRHand hand;
    public List<Transform> interacatable;
    public Transform anchor;
    // Start is called before the first frame update
    void Start()
    {
        skeleton = this.GetComponent<OVRSkeleton>();
        hand = this.GetComponent<OVRHand>();

        foreach(var bone in skeleton.Bones)
        {
            Debug.Log(bone.Id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform i in interacatable)
        {
            if(Vector3.Distance(this.transform.position,i.position) <.1)
            {

                i.parent = hand.PointerPose;// this.anchor;
                //i.position = Vector3.zero;
            }
            else
            {
                i.parent = null;
            }
        }
        
    }

    //Check average of hands to see if object is grasping
    bool IsGrasping()
    {
        return false;

    }

    void SetObject()
    {
        //Get nearest object and add to child
    }

    void RemoveObject()
    {

    }

}
