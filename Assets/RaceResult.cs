using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TOS.Tool;

public class RaceResult : MonoBehaviour
{
	public GameObject itemPrefab;
	public RectTransform itemParent;
	public RankItem playerItem;
		
	
    // Start is called before the first frame update
    void Start()
    {
        LoadLeaderBoard();
    }

    public void LoadLeaderBoard()
    {
		string stageName = PlayerPrefs.GetString( "LatestStage" );
        string leaderboardKey = String.Format("Leaderboard_{0}", stageName);
		string leaderboardStr = PlayerPrefs.GetString( leaderboardKey, String.Empty);	
		RaceLeaderboard leaderboard = RaceLeaderboard.CreateFromJSON(leaderboardStr);
		
		int playerRank = PlayerPrefs.GetInt( String.Format("RaceResultRanking_{0}", stageName), 0);
		
		Debug.Log("playerRank: " + playerRank);
		
		for(int i=0; i<leaderboard.recordList.Count; i++)
		{
			if(i==playerRank)
			{
				Debug.Log("Set playerRank: " + playerRank);
				playerItem.transform.SetAsLastSibling();
				playerItem.UpdateData(i+1, leaderboard.recordList[i].playerName, leaderboard.recordList[i].raceRecord);
				
				playerItem.gameObject.SetActive(true);
			}
			else
			{
				GameObject itemObj = Instantiate<GameObject>(itemPrefab, itemParent);
				
				RankItem rankItem = itemObj.GetComponent<RankItem>();
				rankItem.UpdateData(i+1, leaderboard.recordList[i].playerName, leaderboard.recordList[i].raceRecord);
				
				itemObj.SetActive(true);
			}
		}
		
		//Scroll to Player rank
    }
}
