using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MatchResult : MonoBehaviour
{
	public TMP_Text winnerText;
    public UnityEvent<bool> matchResultEvent;
	
    // Start is called before the first frame update
    void Start()
    {
		string stageName = PlayerPrefs.GetString( "LatestStage" );
        int winnerIndex = PlayerPrefs.GetInt(String.Format("MatchResultWinner_{0}", stageName), 0);
		UpdateWinner(winnerIndex);
		
		matchResultEvent.Invoke(winnerIndex == 0);
    }

    public void UpdateWinner(int winnerIndex)
    {
        winnerText.text = PlayerPrefs.GetString(String.Format("MatchPlayerName_{0}", winnerIndex), winnerIndex == 0? "紅隊" : "藍隊");
    }
}
