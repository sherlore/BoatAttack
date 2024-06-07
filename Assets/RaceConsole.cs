using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceConsole : MonoBehaviour
{
	public string stageName;
	private string playerName;
	
	public float startMoment;
	
	
	void Start()
	{
		playerName = PlayerPrefs.GetString("RacePlayerName", "神秘隊伍");
	}
	
    public void StartRace()
    {
        startMoment = Time.time;
    }

    public void FinishRace()
    {
		PlayerPrefs.SetString( "LatestStage", stageName );
		
        float totalTime = Time.time - startMoment;
		
		//Check Best Time
		
		string bestStr = PlayerPrefs.GetString( String.Format("Best_{0}", stageName), String.Empty);
		
		if( String.IsNullOrEmpty(bestStr) )
		{
			//Set to Best
			UpdateBestRecord(totalTime);
		}
		else
		{
			RaceRecord bestRecord = RaceRecord.CreateFromJSON(bestStr);
			
			if(totalTime < bestRecord.raceRecord)
			{
				//Set to Best
				UpdateBestRecord(totalTime);
			}
		}
		
		//Update Leaderboard
		
		string leaderboardKey = String.Format("Leaderboard_{0}", stageName);
		string leaderboardStr = PlayerPrefs.GetString( leaderboardKey, String.Empty);	
		RaceLeaderboard leaderboard = String.IsNullOrEmpty(leaderboardStr)? new RaceLeaderboard() : RaceLeaderboard.CreateFromJSON(leaderboardStr);
		
		int rankIndex=0;
		
		for(;rankIndex<leaderboard.recordList.Count;rankIndex++)
		{
			if(totalTime<leaderboard.recordList[rankIndex].raceRecord)
			{
				break;
			}
		}
		
		leaderboard.recordList.Insert(rankIndex, new RaceRecord(playerName, totalTime) );
		PlayerPrefs.SetString( leaderboardKey, leaderboard.SaveToString() );	
				
		//Save LeaderBoard Index to player result
		PlayerPrefs.SetInt( String.Format("RaceResultRanking_{0}", stageName), rankIndex);
    }
	
	public void UpdateBestRecord(float newRecord)
	{
		RaceRecord record = new RaceRecord(playerName, newRecord);
		
		PlayerPrefs.SetString( String.Format("Best_{0}", stageName), record.SaveToString());
	}
}
