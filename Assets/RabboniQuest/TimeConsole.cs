using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class TimeConsole : MonoBehaviour
{
	public static TimeConsole instance;
	
	public DatabaseReference timeRef;
	public DateTime Now
	{
		get
		{
			return lastCalibratedDateTime.AddSeconds(Time.realtimeSinceStartup - lastCalibratedMoment);					
		}
	}		
	public long timestamp
	{
		get
		{
			return lastCalibratedTimestamp + Convert.ToInt64(Time.realtimeSinceStartup - lastCalibratedMoment)*1000;
		}
	}
	
	public DateTime UnixTime;
	public long lastCalibratedTimestamp; //milliseconds
	public DateTime lastCalibratedDateTime;
	public float lastCalibratedMoment;
	
	public bool isInitialized;
	
	[Header("CalibrateEvent")]
	public UnityEvent calibrateEvent;
	
	void Awake()
	{
		instance = this;
				
		UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);		
		
		//Use local time as default
		lastCalibratedMoment = Time.realtimeSinceStartup;
		lastCalibratedDateTime = DateTime.Now;
		
		TimeSpan initServerTimeSapn = DateTime.UtcNow - UnixTime;		
		lastCalibratedTimestamp = (long)initServerTimeSapn.TotalMilliseconds;	
	}
	
    public void Init()
    {
        timeRef = FirebaseDatabase.DefaultInstance.GetReference("QuestDemo/Server/Time");
		
		StartCoroutine(Calibrate() );
    }
	
	WaitForSeconds calibrationWait = new WaitForSeconds(60f);
	
	IEnumerator Calibrate()
	{		
		while(true)
		{	
			
			timeRef.RunTransaction(mutableData =>
			{
				mutableData.Value = ServerValue.Timestamp;
				return TransactionResult.Success(mutableData);
			})
			.ContinueWithOnMainThread(task => 
			{
				if (task.IsCompleted) 
				{
					lastCalibratedMoment = Time.realtimeSinceStartup;
								
					// string timestamp = task.Result.Value.ToString();
					lastCalibratedTimestamp = ToLong(task.Result.Value);
					lastCalibratedDateTime = GetDateFromTimestamp( lastCalibratedTimestamp );	
					
					if(!isInitialized)
					{
						isInitialized = true;
					}
					
					calibrateEvent.Invoke();
				}
			});
						
			yield return calibrationWait;
		}
	}
	
	public long ToLong(object obj)
    {
        // return (obj is long)? (long)obj : long.Parse(obj.ToString());
		
		if(obj is long)
		{
			return (long)obj;
		}
		else
		{
			long val;
			
			bool success = long.TryParse(obj.ToString(), out val);
			if (success)
			{
				return val;
			}
			else
			{
				Debug.LogWarning("ExtensionMethod.ToLong not match");
				return 0L;
			}
		}
    }

	public float GetTimeOffsetFromTimestamp(string timestamp)
	{
		return GetTimeOffsetFromTimestamp( long.Parse(timestamp) );
	}
	
	public float GetTimeOffsetFromTimestamp(long timestampDouble)
	{
		float timestampOffsetInSecond = Convert.ToSingle(timestampDouble - lastCalibratedTimestamp)/1000f;
		return timestampOffsetInSecond - Time.realtimeSinceStartup + lastCalibratedMoment;
	}
	
	public DateTime GetDateFromTimestamp(long timestamp )
	{
		return UnixTime.AddMilliseconds( timestamp ).ToLocalTime();
		
	}
	
	public DateTime GetDateFromTimestamp(string timestamp )
	{
		return GetDateFromTimestamp( long.Parse(timestamp) );
		
	}
	
	public DateTime GetDateFromTimestamp(object timestamp )
	{
		if(timestamp is long)
		{
			return GetDateFromTimestamp( (long)timestamp );
		}
		else
		{
			return GetDateFromTimestamp( timestamp.ToString() );
		}
	}
	
	public TimeSpan GetTimeSpanFromTimestamp(string timestamp )
	{
		return GetTimeSpanFromTimestamp( long.Parse(timestamp) );
	}
	
	public TimeSpan GetTimeSpanFromTimestamp(long timestamp )
	{
		return GetDateFromTimestamp( timestamp ) - Now;		
	}
	
	public long GetTimestampFromDate(DateTime date)
	{
		TimeSpan timeSpan = date - UnixTime.ToLocalTime();
		return (long)timeSpan.TotalMilliseconds;
	}
	
	public string GetOnlineTimeDescription(long timestamp)
	{
		TimeSpan timeSpan = GetTimeSpanFromTimestamp(timestamp);
		
		if(timeSpan.TotalSeconds < 0)
		{
			if(Math.Abs(timeSpan.TotalDays) > 30)
			{
				return GetDateFromTimestamp(timestamp).ToString("yyyy.M.d");
			}
			else if(Math.Abs(timeSpan.Days) > 0)
			{
				return String.Format("{0}天前", Math.Abs(timeSpan.Days));
			}
			else if(Math.Abs(timeSpan.Hours) > 0)
			{
				return String.Format("{0}小時前", Math.Abs(timeSpan.Hours));
			}
			else if(Math.Abs(timeSpan.Minutes) > 0)
			{
				return String.Format("{0}分鐘前", Math.Abs(timeSpan.Minutes));
			}
			else
			{
				return String.Format("{0}秒前", Math.Abs(timeSpan.Seconds));
			}
		}
		else
		{
			return String.Format("{0}秒前", Math.Abs(timeSpan.Seconds));
		}			
	}
}
