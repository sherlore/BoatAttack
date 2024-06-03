using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceRecord
{
	public string playerName;
	public float raceRecord;
	
	public RaceRecord(){}
	
	public RaceRecord(string playerName, float raceRecord)
	{
		this.playerName = playerName;
		this.raceRecord = raceRecord;
	}
	
	public static RaceRecord CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<RaceRecord>(jsonString);
	}

	public string SaveToString()
	{
		return JsonUtility.ToJson(this);
	}
}
