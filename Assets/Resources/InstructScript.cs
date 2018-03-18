using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructScript : MonoBehaviour
{

    public Transform[] starts;
	public Transform[] ends;
    public Transform[] current;

	public float rotatespeed = 50;
	public float movespeed= .1f;
    private int step = 0;
    private bool paused, moving, reverse;
    //public UWBNetworkingPackage.NetworkManager network;
    // Use this for initialization
    void Start()
    {
        paused = moving = reverse = false;
        rotatespeed = 50;
        movespeed = .1f;
    }
    public void Next()
    {
        current[step].GetComponent<PhotonView>().RPC("Grab", PhotonTargets.Others);
        if (!moving)
        {
            reverse = false;
            if (!Same(current[step], ends[step]))
                moving = true;
            else if (step < current.Length - 1)
            {
                step++;
                moving = true;
            }
        }
    }
    public void Prev()
    {
        current[step].GetComponent<PhotonView>().RPC("Grab", PhotonTargets.Others);
        if (!moving)
        {
            reverse = true;
            if (!Same(current[step], starts[step]))
                moving = true;
            else if ( step > 0)
            {
                step--;
                moving = true;
            }
        }
    }
    public void Pause()
    {
        //current[step].GetComponent<UWBNetworkingPackage.OwnableObject>().Grab()
        current[step].GetComponent<PhotonView>().RPC("Grab", PhotonTargets.Others);

        if (moving)
            paused = !paused;
    }

    bool Same(Transform a, Transform b)
    {
        if (a.localPosition == b.localPosition && a.localRotation == b.localRotation)
            return true;
        else
            return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!paused && moving)
        {
            if (!reverse)
            {
                current[step].localPosition = Vector3.MoveTowards(current[step].localPosition, ends[step].localPosition, movespeed * Time.deltaTime);
                current[step].localRotation = Quaternion.RotateTowards(current[step].localRotation, ends[step].localRotation, rotatespeed * Time.deltaTime);
                if (Same(current[step], ends[step]))
                    moving = false;
            }
            else
            {
                current[step].localPosition = Vector3.MoveTowards(current[step].localPosition, starts[step].localPosition, movespeed * Time.deltaTime);
                current[step].localRotation = Quaternion.RotateTowards(current[step].localRotation, starts[step].localRotation, rotatespeed * Time.deltaTime);
                if (Same(current[step], starts[step]))
                    moving = false;
            }
        }
    }
}