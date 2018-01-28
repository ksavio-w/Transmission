using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameLogicManager : NetworkBehaviour
{
    [SerializeField] private float blocksMoveSpeed = 10;
    [SerializeField]
    private float minRoadDistance = 50;
    [SerializeField] private Transform initialBlockPosition;

    [SerializeField] Network net;

    [SerializeField]
    private Horse localPlayer; // will be always left
    [SerializeField]
    private Horse remotePlayer;


    [SerializeField]
    private Transform leftStartPosition;
    
    [SerializeField]
    private Transform rightStartPosition;

    [SerializeField]
    private Character _character;

    [SerializeField] private float maxDistance = 4;
    [SerializeField]
    private float minDistance = 0.75f;

    [SerializeField] private GameObject tooCloseBillboard;
    [SerializeField]
    private GameObject tooFarBillboard;
    private int playersCount = 0;

    private void Awake()
    {

        tooFarBillboard.SetActive(false);
        tooCloseBillboard.SetActive(false);

        //RpcHideBillboards();

        if (net == null)
            net = FindObjectOfType<Network>();
        
        net.NewPlayerConnected += OnSecondPlayerConnected;
        _character.CharHit += _character_CharHit;
        
    }

    private void _character_CharHit()
    {
        
    }

    
    
    private void OnSecondPlayerConnected ()
    {
        Debug.Log("Player connected");
        playersCount++;

        if (playersCount > 1)
            StartGame();
    }

    private void StartGame ()
    {
        

        StartCoroutine(GameRoutine());
    }
    
    private IEnumerator GameRoutine()
    {
        yield return new WaitForSeconds(1);
        RpcHideBillboards();

        Horse[] horses = FindObjectsOfType<Horse>();
        foreach (Horse horse in horses)
        {
            if (horse.hasAuthority)
                localPlayer = horse;
            if (!horse.hasAuthority)
                remotePlayer = horse;
        }
        
        localPlayer.RpcSetPosition(leftStartPosition.position);
        remotePlayer.RpcSetPosition(rightStartPosition.position);
        
        _character.leftHorse = localPlayer;
        _character.rightHorse = remotePlayer;
        
        yield return new WaitForSeconds(2);

        localPlayer.RpcStartWalking();
        remotePlayer.RpcStartWalking();

        while (true)
        {
            yield return null;

            if (Vector3.Distance(localPlayer.transform.position, remotePlayer.transform.position) > maxDistance)
            {
                RpcShowTooFarBillboard();
                //tooFarBillboard.SetActive(true);
                localPlayer.RpcStopWalking();
                remotePlayer.RpcStopWalking();
                yield return new WaitForSeconds(3);
                StartGame();
                break;
            }

            if (Vector3.Distance(localPlayer.transform.position, remotePlayer.transform.position) < minDistance)
            {
                RpcShowTooCloseBillboard();
                localPlayer.RpcStopWalking();
                remotePlayer.RpcStopWalking();
                yield return new WaitForSeconds(3);
                StartGame();
                break;
            }
        }

        
    }


    [ClientRpc]
    public void RpcShowTooCloseBillboard()
    {
        tooCloseBillboard.SetActive(true);
    }

    [ClientRpc]
    public void RpcShowTooFarBillboard()
    {
        tooFarBillboard.SetActive(true);
    }

    [ClientRpc]
    public void RpcHideBillboards()
    {
        tooFarBillboard.SetActive(false);
        tooCloseBillboard.SetActive(false);
    }



    private IEnumerator BlocksMoveRoutine ()
    {

            yield return null;

    }
    
}
