using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class RabboniQuestProcceed : MonoBehaviour
{
	public bool activeForProcceed;
	public string questId;
	Firebase.Auth.FirebaseUser user;
	
	public string today;
	public UnityEvent authEvent;
	
    // Start is called before the first frame update
    void Start()
    {
        if(Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser != null)
		{
			user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
			
			authEvent.Invoke();
		}
    }

    // Update is called once per frame
    public void UpdateToday()
    {
		activeForProcceed = true;
        today = TimeConsole.instance.Now.ToString("yyyyMMdd");
    }
	
	public void AddScore(int val)
	{
		if(!activeForProcceed) return;
		
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/DailyQuest/{0}/{1}/{2}", user.UserId, today, questId) ).RunTransaction(mutableData =>
		{
			if(mutableData.Value == null)
			{
				mutableData.Value = val;
				return TransactionResult.Success(mutableData);
			}
			else
			{
				int score = mutableData.Value.ToInt();
				
				mutableData.Value = score + val;
				return TransactionResult.Success(mutableData);
			}
		});
	}
}
