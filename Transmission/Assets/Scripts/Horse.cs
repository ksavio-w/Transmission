using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Horse : NetworkBehaviour
{
    [SerializeField]
    private AccelerometerManager accelerometer;
    [SerializeField]
    private float borderDistance = 10;

    [SerializeField]
    private NetworkIdentity identity;
    
    Vector3 position;

    private void Awake()
    {
        position = transform.position;
        accelerometer = FindObjectOfType<AccelerometerManager>();
        if (identity == null) identity = GetComponent<NetworkIdentity>();
        
    }

    public void Update()
    {
        position.x += accelerometer.Acceleration;
        position.x = Mathf.Clamp(position.x, -borderDistance, borderDistance);

        if (identity.isClient && identity.hasAuthority)
        {
            CmdSetPosition(position);
            transform.position = position;
        }
        
        if (identity.isServer && identity.hasAuthority)
        {
            RpcSetPosition(position);

        }
        
    }

    [Command(channel = 1)]
    void CmdSetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [ClientRpc]
    void RpcSetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }


}
