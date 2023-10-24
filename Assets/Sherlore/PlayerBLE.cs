using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PlayerBLE : MonoBehaviour
{	
    public string rabboniModuleId;
	
	[System.Serializable]
	public class RabboniModuleUI
	{
		public RabboniModule module;
		public RabboniModule.State status;
		public TMP_Text statusText;
		// public Image statusImg;	
		// public GameObject buttonSet;
		// public GameObject buttonLink;
		public GameObject buttonStopScan;
		public GameObject buttonInterruptConnect;
		public Scrollbar batteryLevelBar;
				
		public UnityEvent<bool> disconnectedPageEvent;
		public UnityEvent<bool> scanningPageEvent;
		public UnityEvent<bool> connectingPageEvent;
		public UnityEvent<bool> subscribedPageEvent;
	
		public UnityEvent scanEvent;
		public UnityEvent scanAndConnectEvent;
		public UnityEvent stopScanEvent;
		public UnityEvent interruptConnectEvent;
		
		public void Initialize(RabboniModule rabboniModule)
		{
			module = rabboniModule;
			
			// rabboniModule.addressLog.AddListener(LogAddress);
			rabboniModule.statusLog.AddListener(LogStatus);
			rabboniModule.statusStrLog.AddListener(LogStatusStr);
			rabboniModule.batteryLevelEvent.AddListener(UpdateBatteryLevel);
			// rabboniModule.buttonSetSwitch.AddListener(buttonSet.SetActive);
			// rabboniModule.buttonLinkSwitch.AddListener(buttonLink.SetActive);
			rabboniModule.buttonStopScanSwitch.AddListener(buttonStopScan.SetActive);
			rabboniModule.buttonInterruptConnectSwitch.AddListener(buttonInterruptConnect.SetActive);
			
			rabboniModule.Initialize();
			
			// scanEvent.AddListener(rabboniModule.Scan);
			scanAndConnectEvent.AddListener(rabboniModule.ScanAndConnect);
			stopScanEvent.AddListener(rabboniModule.StopScan);
			interruptConnectEvent.AddListener(rabboniModule.InterruptConnect);
		}
		
		public void Deinitialize()
		{
			// module.addressLog.RemoveListener(LogAddress);
			module.statusLog.RemoveListener(LogStatus);
			module.statusStrLog.RemoveListener(LogStatusStr);
			module.batteryLevelEvent.RemoveListener(UpdateBatteryLevel);
			// module.buttonSetSwitch.RemoveListener(buttonSet.SetActive);
			// module.buttonLinkSwitch.RemoveListener(buttonLink.SetActive);
			module.buttonStopScanSwitch.RemoveListener(buttonStopScan.SetActive);
			module.buttonInterruptConnectSwitch.RemoveListener(buttonInterruptConnect.SetActive);
		}
		
		// public void LogAddress(string val)
		// {
			// addressText.text = val;
		// }
		
		public void LogStatusStr(string val)
		{
			statusText.text = val;
		}
		
		public void LogStatus(int val)
		{
			RabboniModule.State newStatus = (RabboniModule.State)val;
			
			if(status == newStatus) return;
			
			if(status == RabboniModule.State.Disconnected)
			{
				disconnectedPageEvent.Invoke(false);
			}
			else if(status == RabboniModule.State.Scanning)
			{
				scanningPageEvent.Invoke(false);
			}
			else if(status == RabboniModule.State.Connecting)
			{
				connectingPageEvent.Invoke(false);
			}
			else if(status == RabboniModule.State.Subscribed)
			{
				subscribedPageEvent.Invoke(false);
			}
			
			status = newStatus;
			
			if(status == RabboniModule.State.Disconnected)
			{
				disconnectedPageEvent.Invoke(true);
			}
			else if(status == RabboniModule.State.Scanning)
			{
				scanningPageEvent.Invoke(true);
			}
			else if(status == RabboniModule.State.Connecting)
			{
				connectingPageEvent.Invoke(true);
			}
			else if(status == RabboniModule.State.Subscribed)
			{
				subscribedPageEvent.Invoke(true);
			}
		}
		
		public void UpdateBatteryLevel(int val)
		{
			batteryLevelBar.size = val/100f;
		}
	}
	
	public RabboniModuleUI rabboniModuleUI;
	
	void Start()
	{
		rabboniModuleUI.Initialize(RabboniConsole.instance.listDic[rabboniModuleId]);
	}
	
	void OnDestroy()
	{
		if(rabboniModuleUI.module != null)
		{
			rabboniModuleUI.Deinitialize();
		}
	}
	
	/*public void Scan()
	{
		rabboniModuleUI.scanEvent.Invoke();
	}*/
	
	public void ScanAndConnect()
	{
		rabboniModuleUI.scanAndConnectEvent.Invoke();
	}
	
	public void StopScan()
	{
		rabboniModuleUI.stopScanEvent.Invoke();
	}
	
	public void InterruptConnect()
	{
		rabboniModuleUI.interruptConnectEvent.Invoke();
	}
}
