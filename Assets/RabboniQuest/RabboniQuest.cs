using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class RabboniQuest : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
	
	[Header("Account")]
	public string accountEmail;
	public string accountPassword;
	
	[Header("Not Login")]
	public GameObject notLoginPage;
	
	[Header("Wait Login")]
	public GameObject waitLoginPage;
	public Button loginButton;
	public InputField emailField;
	public InputField passwordField;
	public GameObject loginLogObj;
	public Text loginLog;
	public UnityEvent loginedEvent;
	
	[Header("Register")]
	public GameObject registerPage;
	public Button registerButton;
	public InputField registerEmailField;
	public InputField registerPasswordField;
	public InputField userNameField;
	public GameObject registerLogObj;
	public Text registerLog;
	
	[Header("Verfication")]
	public GameObject verficationPage;
	public Text verficationMailAddress;
	public Button verficationButton;
	public GameObject verficationLogObj;
	public Text verficationLog;
	
	[Header("Forgot")]
	public GameObject forgotPswPage;
	public InputField forgotEmailField;
	public Button forgotButton;
	public GameObject forgotLogObj;
	public Text forgotLog;
	
	[Header("Pref")]
	public GameObject prefPage;
	public InputField userEditNameField;
	
	[Header("Quest")]
	public GameObject questPage;
	Firebase.Auth.FirebaseUser user;
	public Text userNameText;
	
	public UnityEvent questInitEvent;
	public UnityEvent questLoadEvent;
	
	[Serializable]
	public class UserReward
	{
		public bool isListenReward;
		public DatabaseReference rewardRef;
		
		public string rewardKey;
		
		public int userReward;
		public Text userRewardText;
		    
		public void HandleRewardChanged(object sender, ValueChangedEventArgs args) 
		{
			if (args.DatabaseError != null) 
			{
				Debug.LogError(args.DatabaseError.Message);
				return;
			}
			
			DataSnapshot snapshot = args.Snapshot;
			
			if(snapshot.Exists)
			{
				int val = snapshot.Value.ToInt();
				userReward = val;
				userRewardText.text = userReward.ToString();
			}
		}
	}
	
	public UserReward[] rewardSet;
	
	[Header("Time")]
	public string today;
	public string weekId;
	public string[] dateOfThisWeek;
	
	[Header("Quest Data")]
	public Dictionary<string, object> dailyQuestDic;
	public Dictionary<string, object> rewardTookDic;
	
	void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		LoadLoginInfo();
    }
	
	public void ClearToday()
	{
		string today = TimeConsole.instance.Now.ToString("yyyyMMdd");
		
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/DailyQuest/{0}/{1}", user.UserId, today) ).RemoveValueAsync();
		FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/RewardTook/{0}/Daily/{1}", user.UserId, today) ).RemoveValueAsync();
	}
	
	public void LoadAccountInfo()
	{
		userNameText.text = user.DisplayName;
		
		for(int i=0; i<rewardSet.Length; i++)
		{
			rewardSet[i].isListenReward = true;
			rewardSet[i].rewardRef = FirebaseDatabase.DefaultInstance.GetReference( String.Format("QuestDemo/Reward/{0}/{1}", user.UserId, rewardSet[i].rewardKey) );
			rewardSet[i].rewardRef.ValueChanged += rewardSet[i].HandleRewardChanged;
		}
		
		StartCoroutine( QuestData() );
    }
	
	public void TestDate()
	{
		DateTime localDate = DateTime.Now;
		today = localDate.ToString("yyyyMMdd");
		
		DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
		Calendar cal = dfi.Calendar;
		
		Debug.Log("dfi.FirstDayOfWeek: " + dfi.FirstDayOfWeek);
		Debug.Log("GetWeekOfYear: " + cal.GetWeekOfYear(localDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek));
	}
	
	void OnDestroy()
	{
		for(int i=0; i<rewardSet.Length; i++)
		{
			if(rewardSet[i].isListenReward)
			{
				rewardSet[i].rewardRef.ValueChanged -= rewardSet[i].HandleRewardChanged;
			}
		}
	}
	
	public void LoadLoginInfo()
	{
		accountEmail = PlayerPrefs.GetString("PlayerMail");
		emailField.text = accountEmail;
		accountPassword = PlayerPrefs.GetString("PlayerPassword");
		passwordField.text = accountPassword;
				
		bool autoLogin = PlayerPrefs.GetInt("AutoLogin", 0) > 0;
		
		if(autoLogin)
		{
			LoginAccount();
		}
	}
	
	public string GetAuthError(string reason)
	{
		if(reason.Contains("The email address is badly formatted") )
		{
			return "*Email 格式錯誤";
		}
		else if(reason.Contains("The given password is invalid") )
		{
			return "*密碼格式錯誤";
		}
		else if(reason.Contains("The email address is already in use by another account") )
		{
			return "*Email 已被註冊";
		}
		else if(reason.Contains("The password is invalid") )
		{
			return "密碼錯誤";
		}
		else if(reason.Contains("There is no user record corresponding to this identifier.") )
		{
			return "*未註冊的Email";
		}
		else if(reason.Contains("An email address must be provided") )
		{
			return "*Email欄位不可為空";
		}		
		else if(reason.Contains("A password must be provided") )
		{
			return "*密碼欄位不可為空";
		}
		else if(reason.Contains("Empty email or password are not allowed") )
		{
			return "*Email或密碼欄位不可為空";
		}
		else if(reason.Contains("unusual activity") )
		{
			return "*因過多錯誤嘗試，將暫時封鎖連線來源";
		}	
		else
		{
			Debug.LogWarning(reason);
			// return reason;
			return "未預期的錯誤";
		}
	}
	
	public void LoginAccount()
	{
		// loginLogObj.SetActive(false);
		loginLog.text = String.Empty;
		
		auth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task => 
		{				
			if (task.IsCanceled) 
			{
				Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
				loginLog.text = "*已取消登入";				
				loginLogObj.SetActive(true);
				
				return;
			}
			if (task.IsFaulted) 
			{
				loginLog.text =  GetAuthError( task.Exception.ToString() ) ;
				loginLogObj.SetActive(true);
				
				return;
			}

			accountEmail = emailField.text;
			accountPassword = passwordField.text;

			Firebase.Auth.AuthResult result = task.Result;
			
			user = result.User;
			SaveLoginInfo(accountEmail, accountPassword);
			
			if(user.IsEmailVerified)
			{
				//Login
				waitLoginPage.SetActive(false);
				
				PlayerPrefs.SetInt("AutoLogin", 1);
				
				loginedEvent.Invoke();
				
				questPage.SetActive(true);
			}
			else
			{
				waitLoginPage.SetActive(false);
				
				verficationMailAddress.text = accountEmail;
				verficationPage.SetActive(true);
			}
		});
	}
	
	public void RegisterAccount()
	{
		registerButton.interactable = false;
		
		auth.CreateUserWithEmailAndPasswordAsync(registerEmailField.text, registerPasswordField.text).ContinueWithOnMainThread(task => 
		{			
			registerButton.interactable = true;
			
			if (task.IsCanceled) 
			{
				// Debug.Log(task.Exception.ToString() );
				registerLog.text = GetAuthError( task.Exception.ToString() ) ;
				registerLogObj.SetActive(true);
				
				return;
			}
			if (task.IsFaulted) 
			{
				// Debug.Log(task.Exception.ToString() );
				registerLog.text = GetAuthError( task.Exception.ToString() ) ;
				registerLogObj.SetActive(true);
				
				return;
			}
							
			accountEmail = registerEmailField.text;
			accountPassword = registerPasswordField.text;

			SaveLoginInfo(accountEmail, accountPassword);
			
			string userName = System.String.IsNullOrEmpty(userNameField.text)? "新用戶" : userNameField.text;
			
			Firebase.Auth.AuthResult result = task.Result;
			user = result.User;
			Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
				DisplayName = userName
			};
			user.UpdateUserProfileAsync(profile);
									
			//Auto Verfication
			registerPage.SetActive(false);
			
			verficationMailAddress.text = accountEmail;
			verficationPage.SetActive(true);
			SendVerficationMail();
		});
	}
	
	public void EditName()
	{
		Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
			DisplayName = userEditNameField.text
		};
		user.UpdateUserProfileAsync(profile);
		
		userNameText.text = userEditNameField.text;
	}
	
	public void SignOut()
	{
		PlayerPrefs.SetInt("AutoLogin", 0);
		// PlayerPrefs.DeleteAll();
		
		for(int i=0; i<rewardSet.Length; i++)
		{
			if(rewardSet[i].isListenReward)
			{
				rewardSet[i].rewardRef.ValueChanged -= rewardSet[i].HandleRewardChanged;
				
				rewardSet[i].isListenReward = false;
				rewardSet[i].userRewardText.text = "0";
			}
		}
		
		Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
	}
	
	public void SendVerficationMail()
	{
		verficationLogObj.SetActive(false);
		verficationButton.interactable = false;
		
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if (user != null) 
		{
			user.SendEmailVerificationAsync().ContinueWithOnMainThread(task => 
			{
				verficationButton.interactable = true;
				if (task.IsCanceled) 
				{
					Debug.LogWarning("SendEmailVerificationAsync was canceled.");
					verficationLog.text = GetAuthError( task.Exception.ToString() ) ;
					verficationLogObj.SetActive(true);
					return;
				}
				if (task.IsFaulted) 
				{
					Debug.LogWarning("SendEmailVerificationAsync encountered an error: " + task.Exception);
					verficationLog.text = GetAuthError( task.Exception.ToString() ) ;
					verficationLogObj.SetActive(true);
					return;
				}
				
				verficationLog.text = "已傳送";
				verficationLogObj.SetActive(true);
			});
		}
	}
	
    public void SendPasswordResetMail()
	{
		forgotLog.text = String.Empty;
		// forgotLogObj.SetActive(false);
		forgotButton.interactable = false;
		
		auth.SendPasswordResetEmailAsync(forgotEmailField.text).ContinueWithOnMainThread(task => 
		{
			forgotButton.interactable = true;
			
			if (task.IsCanceled) 
			{
				Debug.LogError("SendPasswordResetEmailAsync was canceled.");
				forgotLog.text = GetAuthError( task.Exception.ToString() ) ;
				forgotLogObj.SetActive(true);
				return;
			}
			if (task.IsFaulted) 
			{
				Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
				forgotLog.text = GetAuthError( task.Exception.ToString() ) ;
				forgotLogObj.SetActive(true);
				return;
			}
			
		});
	}
	
	public void SaveLoginInfo(string mail, string pwd)
	{
		PlayerPrefs.SetString("PlayerMail", mail);
		PlayerPrefs.SetString("PlayerPassword", pwd);
	}
	
	IEnumerator QuestData()
	{		
		questInitEvent.Invoke();
		
		while(!TimeConsole.instance.isInitialized)
		{		
			yield return null;
		}
		
		questLoadEvent.Invoke();
	}
}
