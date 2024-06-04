using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchConsole : MonoBehaviour
{
	public string stageName;
	
    public void SetMatchWinner(int playerIndex)
    {
		PlayerPrefs.SetString( "LatestStage", stageName );
        PlayerPrefs.SetInt(String.Format("MatchResultWinner_{0}", stageName), playerIndex);
    }
}
