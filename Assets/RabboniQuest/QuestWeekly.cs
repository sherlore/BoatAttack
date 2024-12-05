using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class QuestWeekly : MonoBehaviour
{
	public string questId;
	
	public int targetScore;
	
	public Toggle[] weekToggles;
	public string[] weekDates;
	
	[Serializable]
	public class WeekReward
	{
		public string id;
		public int targetCount;
		public Text progressText;
		public Scrollbar progressBar;
		
		public string rewardKey;
		public int reward;
		
		public GameObject rewardButtonObj;
		public GameObject rewardGotObj;
	}
	
	public WeekReward[] weekReward;
	public int weekCount;
	public string weekId;
	
    // Start is called before the first frame update
    public void Init()
    {
		for(int i=0; i<weekReward.Length; i++)
		{
			weekReward[i].progressText.text = String.Format("0/{0}", weekReward[i].targetCount);
			weekReward[i].progressBar.size = 0f;
			weekReward[i].rewardButtonObj.SetActive(false);
			weekReward[i].rewardGotObj.SetActive(false);
		}
    }
	
    public void UpdateData()
    {
		GetWeekStatus();
    }
	
	public void TakeReward(int index)
	{
		weekReward[index].rewardButtonObj.SetActive(false);
		
		Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/Reward/{0}/{1}", user.UserId, weekReward[index].rewardKey) ).RunTransaction(mutableData =>
		{
			if(mutableData.Value == null)
			{
				mutableData.Value = weekReward[index].reward;
				return TransactionResult.Success(mutableData);
			}
			else
			{
				int totalReward = mutableData.Value.ToInt();
				
				mutableData.Value = totalReward + weekReward[index].reward;
				return TransactionResult.Success(mutableData);
			}
		})
		.ContinueWithOnMainThread(task => 
		{
			if (task.IsCompleted) 
			{
				weekReward[index].rewardGotObj.SetActive(true);
				
				DateTime nowDate = TimeConsole.instance.Now;
				DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
				Calendar cal = dfi.Calendar;
		
				string weekId = String.Format("Week{0}_{1}", nowDate.ToString("yyyy"), cal.GetWeekOfYear(nowDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek));
				FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/RewardTook/{0}/Weekly/{1}/{2}/{3}", user.UserId, weekId, questId, weekReward[index].id) ).SetValueAsync(true);
			}
		});
	}
	
	public void UpdateWeekReward(int index)
	{		
		Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/RewardTook/{0}/Weekly/{1}/{2}/{3}", user.UserId, weekId, questId, weekReward[index].id) ).GetValueAsync().ContinueWithOnMainThread(tookTask => 
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
					weekReward[index].rewardGotObj.SetActive(true);
				}
				else
				{
					weekReward[index].rewardButtonObj.SetActive(true);
				}
			}
		});
	}
	
	public void UpdateWeekDoneCount()
	{
		weekCount = 0;
		
		for(int i=0; i<7; i++)
		{
			if(weekToggles[i].isOn)
			{
				weekCount++;
			}
		}
		
		DateTime nowDate = TimeConsole.instance.Now;
		DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
		Calendar cal = dfi.Calendar;
		
		weekId = String.Format("Week{0}_{1}", nowDate.ToString("yyyy"), cal.GetWeekOfYear(nowDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek));
		
		for(int i=0; i<weekReward.Length; i++)
		{
			weekReward[i].progressText.text = String.Format("{0}/{1}", weekCount, weekReward[i].targetCount);
			weekReward[i].progressBar.size = (float)weekCount/weekReward[i].targetCount;
			
			if(weekCount >= weekReward[i].targetCount)
			{
				UpdateWeekReward(i);
			}
		}
	}
	
	public async void GetWeekStatus()
	{
		DateTime nowDate = TimeConsole.instance.Now;		
		DateTime firstDayOfWeek = nowDate.AddDays( (int)DayOfWeek.Sunday - (int)nowDate.DayOfWeek );
		
		Task[] tasks = new Task[7];
		
		for(int i=0; i<7; i++)
		{
			weekDates[i] = firstDayOfWeek.AddDays(i).ToString("yyyyMMdd");
			
			tasks[i] = GetDayStatus(i);
		}
		
		await Task.WhenAll(tasks);
		
		UpdateWeekDoneCount();
	}
	
	public Task GetDayStatus(int weekIndex)
	{
		Firebase.Auth.FirebaseUser user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
		return FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/DailyQuest/{0}/{1}/{2}", user.UserId, weekDates[weekIndex], questId) ).GetValueAsync().ContinueWithOnMainThread(task => 
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
				
				int currentScore = 0;
				
				if(snapshot.Exists)
				{
					currentScore = snapshot.Value.ToInt();	
				}
				else
				{
					currentScore = 0;
				}
				
				weekToggles[weekIndex].isOn = currentScore >= targetScore;
				
				/*if(weekToggles[weekIndex].isOn)
				{
					UpdateWeekDoneCount();
				}*/
			}
		});
	}
	
	
}
