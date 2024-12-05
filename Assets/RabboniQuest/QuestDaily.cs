using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class QuestDaily : MonoBehaviour
{
	public string questId;
	
	public int currentScore;
	public int targetScore;
	public Text progressText;
	public Scrollbar progressBar;
	
	public string description;
	public Text descriptionText;
	
	public GameObject rewardButtonObj;
	public GameObject rewardGotObj;
	
	public float reward;
	public string rewardKey = "Coin";
	public Text rewardText;
	
    // Start is called before the first frame update
	
	public void Init()
	{
		progressText.text = String.Format("{0}/{1}", currentScore, targetScore);
		progressBar.size = (float)currentScore/targetScore;
		
		descriptionText.text = description;
		rewardText.text = reward.ToString("F0");
		
		rewardButtonObj.SetActive(false);
		rewardGotObj.SetActive(false);
	}
	
	public void TakeReward()
	{
		rewardButtonObj.SetActive(false);
		
		Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/Reward/{0}/{1}", user.UserId, rewardKey) ).RunTransaction(mutableData =>
		{
			if(mutableData.Value == null)
			{
				mutableData.Value = reward;
				return TransactionResult.Success(mutableData);
			}
			else
			{
				// float totalReward = (mutableData.Value is float)? (float)mutableData.Value : float.Parse(mutableData.Value.ToString());
				float totalReward = mutableData.Value.ToFloat();
				
				mutableData.Value = totalReward + reward;
				return TransactionResult.Success(mutableData);
			}
		})
		.ContinueWithOnMainThread(task => 
		{
			if (task.IsCompleted) 
			{
				rewardGotObj.SetActive(true);
				
				string today = TimeConsole.instance.Now.ToString("yyyyMMdd");
				FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/RewardTook/{0}/Daily/{1}/{2}", user.UserId, today, questId) ).SetValueAsync(true);
			}
		});
	}
	
    // Update is called once per frame
    public void UpdateData()
    {
		string today = TimeConsole.instance.Now.ToString("yyyyMMdd");
		Debug.Log("today: " + today);
		
		Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
		
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/DailyQuest/{0}/{1}/{2}", user.UserId, today, questId) ).GetValueAsync().ContinueWithOnMainThread(task => 
		{
			if (task.IsFaulted) 
			{
			  // Handle the error...
			  Debug.Log("error: " + task.Exception.ToString());
			}
			else if (task.IsCompleted) 
			{
				DataSnapshot snapshot = task.Result;
				// Do something with snapshot...
				
				if(snapshot.Exists)
				{
					currentScore = snapshot.Value.ToInt();	
				}
				else
				{
					currentScore = 0;
				}
				
				progressText.text = String.Format("{0}/{1}", currentScore, targetScore);
				progressBar.size = (float)currentScore/targetScore;
				
				if(currentScore >= targetScore)
				{
					FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/RewardTook/{0}/Daily/{1}/{2}", user.UserId, today, questId) ).GetValueAsync().ContinueWithOnMainThread(tookTask => 
					{
						if (tookTask.IsFaulted) 
						{
						  // Handle the error...
						}
						else if (tookTask.IsCompleted) 
						{
							DataSnapshot tookSnapshot = tookTask.Result;
							// Do something with snapshot...
							
							bool rewardTook = false;
							
							if(tookSnapshot.Exists)
							{
								rewardTook = tookSnapshot.Value.ToBool();	
							}
							
							if(rewardTook)
							{
								rewardGotObj.SetActive(true);
							}
							else
							{
								rewardButtonObj.SetActive(true);
							}
						}
					});
				}
			}
		});
    }
}
