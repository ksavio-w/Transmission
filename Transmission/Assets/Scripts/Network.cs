using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class Network : NetworkManager
{
    private NetworkMatch networkMatch;
    List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
    [SerializeField] private MainMenuView mainMenu;
    MatchInfo _currentHostedMatch;
    public event Action NewPlayerConnected = delegate { };

    public Text idText;
    private int id;

    private void Awake()
    {
        networkMatch = this.gameObject.AddComponent<NetworkMatch>();
        StartCoroutine(MatchListUpdateRoutine());
        mainMenu.MatchSelected += OnMatchSelected;
        
        id = UnityEngine.Random.Range(1 , 100);
        idText.text = id.ToString();
    }

    private void OnMatchSelected(MatchInfoSnapshot matchInfo)
    {
        Debug.Log("Match selected");
        networkMatch.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoinedDone);
    }

    private IEnumerator MatchListUpdateRoutine()
    {
        while (true)
        {
            RequestMatchList();
            yield return new WaitForSeconds(3);
        }
    }

    private void RequestMatchList()
    {
        networkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchListReceived);
    }

    public void OnMatchListReceived(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        this.matchList = matches;
        if (success && matches != null && matches.Count > 0)
        {
            
            mainMenu.RecreateList(matches);
        }
        else if (!success)
        {
            Debug.LogError("List match failed: " + extendedInfo);
        }
    }

    public void CreateMatch()
    {
        string matchName = "room " + id.ToString();
        uint matchSize = 2;
        bool matchAdvertise = true;
        string matchPassword = "";
        networkMatch.CreateMatch(matchName, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnMatchCreationDone);
    }

    public void OnMatchCreationDone(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Create match succeeded");
            //matchCreated = true;
            NetworkServer.Listen(matchInfo, 1000);
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
            RequestMatchList();
            mainMenu.Hide();
            _currentHostedMatch = matchInfo;
            StartHost(matchInfo);
            
        }
        else
        {
            Debug.LogError("Create match failed: " + extendedInfo);
        }
    }
    
    public void JoinMatch()
    {
        //networkMatch.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    //void OnGUI()
    //{
    //    // You would normally not join a match you created yourself but this is possible here for demonstration purposes.
    //    if (GUILayout.Button("Create Room"))
    //    {
    //        string matchName = "room";
    //        uint matchSize = 4;
    //        bool matchAdvertise = true;
    //        string matchPassword = "";

    //        networkMatch.CreateMatch(matchName, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnMatchCreate);
    //    }

    //    //if (GUILayout.Button("List rooms"))
    //    //{
    //    //    networkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
    //    //}

    //    if (matchList.Count > 0)
    //    {
    //        GUILayout.Label("Current rooms");
    //    }
    //    foreach (var match in matchList)
    //    {
    //        if (GUILayout.Button(match.name))
    //        {
    //            //networkMatch.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
    //        }
    //    }
    //}

    public void OnMatchJoinedDone(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        Debug.Log("Match joined, starting client");
        mainMenu.Hide();
        StartClient(matchInfo);
    }

    public void OnDestroy()
    {
        if (_currentHostedMatch != null)
            networkMatch.DestroyMatch(_currentHostedMatch.networkId, 0, null);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        if (NewPlayerConnected != null)
            NewPlayerConnected();
    }
    
}
