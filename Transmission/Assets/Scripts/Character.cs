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



    public void Update ()
    {
        if (!isServer) return;

        if (leftHorse == null || rightHorse == null) return;

        position = Vector3.Lerp(leftHorse.transform.position, rightHorse.transform.position, 0.5f) + Vector3.up * 0.5f;

        if (isServer)
        {
            RpcSetPosition(position);
        }
    }


    [ClientRpc]
    public void RpcSetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void OnTriggerEnter()
    {
        Debug.Log("On collision enter");
    }

}
