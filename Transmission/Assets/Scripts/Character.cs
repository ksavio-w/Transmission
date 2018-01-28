using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character : NetworkBehaviour
{
    public Horse leftHorse;
    public Horse rightHorse;

    Vector3 position;

    public event Action CharHit = delegate { };

    [SerializeField] Transform LeftHand;
    [SerializeField] Transform RightHand;
    [SerializeField] Transform LeftLeg;
    [SerializeField] Transform RightLeg;


    [SerializeField]
    private LineRenderer leftForwardLR;
    [SerializeField]
    private LineRenderer leftBackLR;

    [SerializeField]
    private LineRenderer rightForwardLR;
    [SerializeField]
    private LineRenderer rightBackLR;
    


    private void UpdateLineRenderers ()
    {
        if (leftHorse == null || rightHorse == null) return;

        leftForwardLR.SetPosition(0, LeftHand.transform.position);
        leftForwardLR.SetPosition(1, leftHorse.backRopeSocket.transform.position);

        rightForwardLR.SetPosition(0, RightHand.transform.position);
        rightForwardLR.SetPosition(1, rightHorse.backRopeSocket.transform.position);

        leftBackLR.SetPosition(0, LeftLeg.transform.position);
        leftBackLR.SetPosition(1, leftHorse.forwardRopeSocket.transform.position);

        rightBackLR.SetPosition(0, RightLeg.transform.position);
        rightBackLR.SetPosition(1, rightHorse.forwardRopeSocket.transform.position);

    }

    public void Update ()
    {
        //UpdateLineRenderers();

        if (!isServer) return;

        if (leftHorse == null || rightHorse == null) return;
        

        if (isServer)
        {
            RpcSetPosition(position);
            position = Vector3.Lerp(leftHorse.transform.position, rightHorse.transform.position, 0.5f) + Vector3.up * 0.5f;
        }
    }


    [ClientRpc]
    public void RpcSetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        UpdateLineRenderers();
        if (leftHorse == null || rightHorse == null)
        {
            Horse[] horses = FindObjectsOfType<Horse>();
            foreach (Horse horse in horses)
            {
                if (isServer)
                {

                    if (horse.isServer)
                        leftHorse = horse;
                    if (horse.isClient)
                        rightHorse = horse;
                }
                else
                {
                    if (!horse.isLocalPlayer)
                        leftHorse = horse;
                    if (horse.isLocalPlayer)
                        rightHorse = horse;

                }
            }
        }

    }

    //private void OnTriggerEnter(Collision collision)
    //{
    //    //Destroy(collision.transform.gameObject);
    //}

}
