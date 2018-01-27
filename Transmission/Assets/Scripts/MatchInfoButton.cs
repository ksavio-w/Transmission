using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MatchInfoButton : MonoBehaviour
{
    public MatchInfoSnapshot MatchInfo { get; set; }
    public event Action<MatchInfoSnapshot> Clicked = delegate { };
    [SerializeField] private Text buttonText;

    public string Text
    {
        get { return buttonText.text; }
        set { buttonText.text = value; }
    }

    public void OnClicked ()
    {
        if (Clicked != null)
            Clicked(MatchInfo);
    }
}
