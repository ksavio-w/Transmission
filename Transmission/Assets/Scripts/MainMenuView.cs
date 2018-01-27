using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private MatchInfoButton matchPrefab;
    private List<MatchInfoButton> existingMatches = new List<MatchInfoButton>();
    public event Action<MatchInfoSnapshot> MatchSelected = delegate { };
    [SerializeField] private Transform gridLayout;
 
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide ()
    {
        gameObject.SetActive(false);
    }


    public void RecreateList( List<MatchInfoSnapshot> matches)
    {
        List<MatchInfoButton> buttonsToDelete = new List<MatchInfoButton>(existingMatches);
        for (int i = 0; i < buttonsToDelete.Count; i++)
        {
            buttonsToDelete[i].Clicked -= OnMatchButtonClick;
            Destroy(buttonsToDelete[i].gameObject);
        }

        buttonsToDelete.Clear();
        existingMatches.Clear();

        foreach(MatchInfoSnapshot matchInfo in matches)
        {
            MatchInfoButton newButton = Instantiate<MatchInfoButton>(matchPrefab);
            newButton.transform.SetParent(gridLayout);
            newButton.transform.localScale = Vector3.one;
            existingMatches.Add(newButton);
            newButton.MatchInfo = matchInfo;
            newButton.Text = matchInfo.name;
            newButton.Clicked += OnMatchButtonClick;
        }
    }

    public void OnMatchButtonClick (MatchInfoSnapshot matchInfo)
    {
        MatchSelected(matchInfo);
    }

}
