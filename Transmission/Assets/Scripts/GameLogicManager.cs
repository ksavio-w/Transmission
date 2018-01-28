using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    [SerializeField] Sounds sounds;
    [SerializeField] Network net;
    [SerializeField]
    private Horse localPlayer; // will be always left
    [SerializeField]
    private Horse remotePlayer;

    [SerializeField]
    private List<GameObject> Blocks;

    private void Awake()
    {
        if (net == null)
            net = FindObjectOfType<Network>();

        net.NewPlayerConnected += OnSecondPlayerConnected;
         
    }
    
    private void OnSecondPlayerConnected ()
    {
        StartGame();
    }

    private void StartGame ()
    {

    }


}
