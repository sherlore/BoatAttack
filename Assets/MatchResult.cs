using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchResult : MonoBehaviour
{
	public string stageName;
	public TMP_Text winnerText;
	
    // Start is called before the first frame update
    void Start()
    {
        int winnerIndex = PlayerPrefs.GetInt(String.Format("MatchResultWinner_{0}", stageName), 0);
    }

    public void UpdateWinner(int winnerIndex)
    {
        winnerText.text = PlayerPrefs.GetString(String.Format("MatchPlayerName_{0}", winnerIndex), winnerIndex == 0? "藍隊" : "紅隊");
    }
}
