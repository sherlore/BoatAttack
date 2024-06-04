using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceLeaderboard
{
	public List<RaceRecord> recordList;
	
	public RaceLeaderboard()
	{
		recordList = new List<RaceRecord>();
	}
	
	public static RaceLeaderboard CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<RaceLeaderboard>(jsonString);
	}

	public string SaveToString()
	{
		return JsonUtility.ToJson(this);
	}
}
