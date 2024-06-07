using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaceBestRecordPanel : MonoBehaviour
{
	public string stageName;
	public TMP_Text playerNameText;
	public TMP_Text raceRecordText;
	
    // Start is called before the first frame update
    void Start()
    {
        string recordStr = PlayerPrefs.GetString( String.Format("Best_{0}", stageName), String.Empty);
		
		if(!String.IsNullOrEmpty(recordStr))
		{
			RaceRecord record = RaceRecord.CreateFromJSON(recordStr);
			
			playerNameText.text = record.playerName;
			raceRecordText.text = String.Format("{0:F1} 秒", record.raceRecord);
		}
    }
}
