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

    private bool isWalking = false;

    [SerializeField]
    private float cameraDistance = 8;

    [SerializeField]
    private float speed = 10;

    private float _acceleration;

    private void Awake()
    {
        
        accelerometer = FindObjectOfType<AccelerometerManager>();
        if (identity == null) identity = GetComponent<NetworkIdentity>();
    
    }

    public void Update()
    {
        if (!isWalking) return;

        if (hasAuthority)
        {
            _acceleration = accelerometer.Acceleration;
            
        }
        

        if (identity.isClient && identity.hasAuthority)
        {
            CmdSetAcceleration(accelerometer.Acceleration);
        }
        
        if (identity.isServer/* && identity.hasAuthority*/)
        {
            position.x += _acceleration;
            position.z += speed * Time.deltaTime;

            RpcSetPosition(position);
        }
        
    }

    public void LateUpdate()
    {
        if (hasAuthority)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            cameraPos.z = transform.position.z - cameraDistance;
            Camera.main.transform.position = cameraPos;
        }
    }

    [Command(channel = 1)]
    public void CmdSetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [Command(channel = 1)]
    public void CmdSetAcceleration(float newAcceleration)
    {
        _acceleration = newAcceleration;
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }


    [ClientRpc]
    public void RpcStartWalking()
    {
        position = transform.position;
        isWalking = true;
    }

    [ClientRpc]
    public void RpcStopWalking()
    {
        isWalking = false;
    }

    private void OnTriggerEnter()
    {
        Debug.Log("On collision enter");
        speed = 0;
    }


}
