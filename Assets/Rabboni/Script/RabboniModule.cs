using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RabboniModule : MonoBehaviour
{
	public enum State
	{
		Disconnected,
		Scanning,
		Connecting,
		Subscribed
	}
	
	public RabboniConsole console;
	// public UIRabboniModule UIModule;
	
	public State state;
	
	public string targetAddress;
	
	public string deviceId = "Device";
	public RabboniConsole.DeviceType deviceType;
	
	public bool subscribeChecked;
	public bool prefsChecked;
	public bool isConnected;
	
	public float _accScale = 2f;
	public float accScale
	{
		get
		{
			return _accScale;
		}
		set
		{
			_accScale = value;
			// UIModule.maxAcc = value;
		}
	}

	//Right-handed
	public Vector3 _lastAcc;
	public Vector3 lastAcc
	{
		get
		{
			return _lastAcc;
		}
		set
		{
			_lastAcc = value;
			// UIModule.acc = value;
		}
	}

	public Vector3 unityAcc 
	{
		get 
		{
			return SwitchHandness(lastAcc);
		}
	}
	
	public float _gyroScale = 1000f;
	public float gyroScale
	{
		get
		{
			return _gyroScale;
		}
		set
		{
			_gyroScale = value;
			// UIModule.maxGyro = value;
		}
    }

    //Right-handed
    public Vector3 _lastGyro;
	public Vector3 lastGyro
	{
		get
		{
			return _lastGyro;
		}
		set
		{
			_lastGyro = value;
			// UIModule.gyro = value;
		}
    }

    public Vector3 unityGyro
    {
        get
        {
            return SwitchHandness(lastGyro);
        }
    }

    [Header("Log")]
	public UnityEvent<string> statusStrLog;
	public UnityEvent<int> statusLog;
	
	public UnityEvent<bool> buttonStopScanSwitch;
	public UnityEvent<bool> buttonInterruptConnectSwitch;
	
	[Header("Event")]
	public UnityEvent connectedEvent;
	public UnityEvent disconnectedEvent;
	public UnityEvent<Vector3, Vector3> IMUEvent;
	
	[Header("Battery")]
	public int batteryLevel;
	public UnityEvent<int> batteryLevelEvent;

	[Header("Kalman")]
	public RabboniKalman rabboniKalman;
	public bool UseKalman 
	{
		get 
		{
			return rabboniKalman != null;
		}
	}
	public Quaternion KalmanRotation
    {
        get
        {
            return rabboniKalman.kalmanRotation;
        }
    }
    public float Roll
    {
        get
        {
            return rabboniKalman.GetRoll();
        }
    }
    public float Pitch
    {
        get
        {
            return rabboniKalman.GetPitch();
        }
    }
    public Vector3 Gravity
    {
        get
        {
            return KalmanRotation * Vector3.up;
        }
    }
	public Vector3 AccZeroGravity
    {
        get
        {
            return unityAcc - Gravity;
        }
    }
	public float Energy
    {
        get
        {
            return AccZeroGravity.magnitude;
        }
    }
    public Vector3 CalibratedAcc
    {
        get
        {
            return Quaternion.Inverse(KalmanRotation) * unityAcc;
        }
    }
    public Vector3 CalibratedGyro
    {
        get
        {
            return Quaternion.Inverse(KalmanRotation) * unityGyro;
        }
    }

	public Vector3 SwitchHandness(Vector3 val) 
	{
		val.x = -val.x;
		return val;
	}

    // public Text testLog;

#if UNITY_EDITOR

    [Header("Simulation")]
    public Vector3 simulationAcc;
	public Vector3 simulationGyro;

	void Update()
	{
		lastAcc = simulationAcc;
		lastGyro = simulationGyro;

        IMUEvent.Invoke(lastAcc, lastGyro);

        if (UseKalman)
        {
            rabboniKalman.UpdateIMU(lastAcc, lastGyro);
        }
    }
	#endif
	
	public string GetDeviceName()
	{
		return console.GetDeviceName(deviceType);
	}
	
    // Start is called before the first frame update
    // void Start()
    // {
		// Initialize();
    // }
	
	public void Initialize()
	{
		if(isConnected)
		{
			statusStrLog.Invoke("已連線");
			statusLog.Invoke( (int)state );
			batteryLevelEvent.Invoke(batteryLevel);
			
			GetBatteryLevel();
		}
		else
		{
			statusStrLog.Invoke("尚未連接Rabboni");
			statusLog.Invoke( (int)state );
		}
	}
	
	/*public void Scan()
	{
		state = State.Scanning;
		statusLog.Invoke( state );		
		statusStrLog.Invoke("Scanning...");
		
		buttonSetSwitch.Invoke(false);
		buttonStopScanSwitch.Invoke(true);

		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => 
		{
			// we only want to look at devices that have the name we are looking for
			// this is the best way to filter out devices
			for(int i = 0; i < console.DeviceNameRabboni.Length; i++)
			{
				if (name.Contains (console.DeviceNameRabboni[i]))
				{
					// it is always a good idea to stop Scanning while you connect to a device
					// and get things set up
					BluetoothLEHardwareInterface.StopScan ();

					targetAddress = address;

					deviceType = RabboniConsole.DeviceType.Rabboni;
					statusStrLog.Invoke("發現目標");	
					
					addressLog.Invoke(targetAddress);	
					PlayerPrefs.SetString(String.Format("{0}_TargetAddress", deviceId), targetAddress);
					
					// statusColorLog.Invoke(console.UIModule.disconnectedColor);
					// buttonLinkSwitch.Invoke(true);
					buttonSetSwitch.Invoke(true);
					buttonStopScanSwitch.Invoke(false);
					
					state = State.Disconnected;
					statusLog.Invoke( state );
					return;
				}
			}
			
			for(int i = 0; i < console.DeviceNameNaxsen.Length; i++)
			{
				if (name.Contains (console.DeviceNameNaxsen[i]))
				{
					// it is always a good idea to stop Scanning while you connect to a device
					// and get things set up
					BluetoothLEHardwareInterface.StopScan ();

					targetAddress = address;

					deviceType = RabboniConsole.DeviceType.Naxsen;
					statusStrLog.Invoke("發現目標");	
					
					addressLog.Invoke(targetAddress);	
					PlayerPrefs.SetString(String.Format("{0}_TargetAddress", deviceId), targetAddress);
					
					buttonSetSwitch.Invoke(true);
					buttonStopScanSwitch.Invoke(false);
					
					state = State.Disconnected;
					statusLog.Invoke( state );
					return;
				}
			}
		}, null, false, false);
	}*/
	
	public void StopScan()
	{
		state = State.Disconnected;
		statusLog.Invoke( (int)state );
		statusStrLog.Invoke("已停止尋找Rabboni");	
		
		BluetoothLEHardwareInterface.StopScan ();
		
		// buttonSetSwitch.Invoke(true);
		buttonStopScanSwitch.Invoke(false);
	}
	
	public void ScanAndConnect()
	{
		state = State.Scanning;
		statusLog.Invoke( (int)state );
		
		statusStrLog.Invoke("正在尋找Rabboni...");	
		// buttonSetSwitch.Invoke(false);
		buttonStopScanSwitch.Invoke(true);

		BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => 
		{
			for(int i = 0; i < console.DeviceNameRabboni.Length; i++)
			{
				if (name.Contains (console.DeviceNameRabboni[i]))
				{
					// it is always a good idea to stop Scanning while you connect to a device
					// and get things set up
					BluetoothLEHardwareInterface.StopScan ();

					targetAddress = address;

					deviceType = RabboniConsole.DeviceType.Rabboni;
					statusStrLog.Invoke("發現Rabboni");	
					
					buttonStopScanSwitch.Invoke(false);
					Invoke("Connect", 0.3f);
					return;
				}
			}
			
			for(int i = 0; i < console.DeviceNameNaxsen.Length; i++)
			{
				if (name.Contains (console.DeviceNameNaxsen[i]))
				{
					// it is always a good idea to stop Scanning while you connect to a device
					// and get things set up
					BluetoothLEHardwareInterface.StopScan ();

					targetAddress = address;

					deviceType = RabboniConsole.DeviceType.Naxsen;
					statusStrLog.Invoke("發現Naxsen");	
					
					buttonStopScanSwitch.Invoke(false);
					Invoke("Connect", 0.3f);
					return;
				}
			}

		}, null, false, false);
	}
		
	public void Connect()
	{						
		state = State.Connecting;
		statusLog.Invoke( (int)state );
		statusStrLog.Invoke("連接中...");
		
		buttonInterruptConnectSwitch.Invoke(true);
		
		subscribeChecked = false;
		prefsChecked = false;

		BluetoothLEHardwareInterface.ConnectToPeripheral (targetAddress, null, null, (address, serviceUUID, characteristicUUID) => 
		{
			//Android / iOS return different UUID format, android give fullUUID, iOS give only UUID
			// #if UNITY_ANDROID
			// testLog.text += String.Format("{0}x{1}\n", serviceUUID.Substring(4, 4), characteristicUUID.Substring(4, 4));
			// #else
			// testLog.text += String.Format("{0}x{1}\n", serviceUUID, characteristicUUID);
			// #endif
			
			if (address == targetAddress)
			{
				if (!subscribeChecked)
				{
					if (console.IsEqual (serviceUUID, console.SubscribeServiceUUID) && console.IsEqual (characteristicUUID, console.SubscribeCharacteristic))
					{
						subscribeChecked = true;
					}
				}
				
				if (!prefsChecked)
				{
					if (console.IsEqual (serviceUUID, console.PrefsServiceUUID) && console.IsEqual (characteristicUUID, console.PrefsCharacteristic))
					{
						prefsChecked = true;
					}
				}
				
				console.moduleDic[address] = this;
				if(!isConnected && subscribeChecked && prefsChecked)
				{
					statusStrLog.Invoke("成功連接");	
						
					isConnected = true;
					
					buttonInterruptConnectSwitch.Invoke(false);
									
					Invoke("InitPrefs", 0.3f);		
					// Invoke("SubscribePrefs", 0.5f);		
				}
			}
		}, (disconnectedAddress) => 
		{
			BluetoothLEHardwareInterface.Log ("Device Disconnected: " + disconnectedAddress);
			if(console.moduleDic.ContainsKey(disconnectedAddress) )
			{
				console.moduleDic[disconnectedAddress].OnBLEDisconnected();
				console.moduleDic.Remove(disconnectedAddress);
			}
		});		
	}
	
	public void InterruptConnect()
	{
		BluetoothLEHardwareInterface.DisconnectPeripheral (targetAddress, (disconnectedAddress) => 
		{
		});
			
		state = State.Disconnected;
		statusLog.Invoke( (int)state );
		statusStrLog.Invoke("已中斷連接");	
		
		if(console.moduleDic.ContainsKey(targetAddress) )
		{
			console.moduleDic[targetAddress].OnBLEDisconnected();
			console.moduleDic.Remove(targetAddress);
		}
		
		isConnected = false;
		lastAcc = Vector3.zero;
		lastGyro = Vector3.zero;
		
		// buttonSetSwitch.Invoke(true);
		buttonStopScanSwitch.Invoke(false);
		buttonInterruptConnectSwitch.Invoke(false);
				
		disconnectedEvent.Invoke();
	}
	
	public void OnBLEDisconnected()
	{
		state = State.Disconnected;
		statusLog.Invoke( (int)state );			
		statusStrLog.Invoke("失去連線");	
		
		isConnected = false;
		lastAcc = Vector3.zero;
		lastGyro = Vector3.zero;
			
		// buttonSetSwitch.Invoke(true);
		buttonStopScanSwitch.Invoke(false);
				
		disconnectedEvent.Invoke();
	}
	
	
	public void InitPrefs()
	{
		statusStrLog.Invoke("正在初始化...");	
				
		string cmd = String.Empty;
		
		if(deviceType == RabboniConsole.DeviceType.Rabboni)
		{
			cmd = String.Format("45{0}{1}011201010000{2}00000009C4", "00", "00", "00" );
		}
		else
		{
			cmd = String.Format("45{0}{1}01000001000000{2}00", "00", "00", "00" );
		}
		
		byte[] data = console.StringToByteArray(cmd);
		// byte[] data = new byte[] { 0x49 };
		
		BluetoothLEHardwareInterface.WriteCharacteristic (targetAddress, console.PrefsServiceUUID, console.PrefsCharacteristic, data, data.Length, true, (characteristicUUID) => 
		{
			statusStrLog.Invoke("初始化完成");	
			
			Invoke("ResetPrefs", 0.3f);
		});
	}
	
	public void ResetPrefs()
	{
		statusStrLog.Invoke("設定靈敏度...");	
				
		string cmd = String.Empty;
		
		if(deviceType == RabboniConsole.DeviceType.Rabboni)
		{
			cmd = String.Format("45{0}{1}011201010000{2}00000009C4", console.GetAccScaleCmd(), console.GetGyroScaleCmd(), console.GetDataRateCmd() );
		}
		else
		{
			cmd = String.Format("45{0}{1}01000001000000{2}00", console.GetAccScaleCmd(), console.GetGyroScaleCmd(), console.GetDataRateCmd() );
		}
		
		accScale = console.GetAccScaleValue();
		gyroScale = console.GetGyroScaleValue();
		
		byte[] data = console.StringToByteArray(cmd);
		// byte[] data = new byte[] { 0x49 };
		
		BluetoothLEHardwareInterface.WriteCharacteristic (targetAddress, console.PrefsServiceUUID, console.PrefsCharacteristic, data, data.Length, true, (characteristicUUID) => 
		{
			statusStrLog.Invoke("設定完成");	
			
			Invoke("Subscribe", 0.3f);
			Invoke("GetBatteryLevel", 1f);
			//Invoke("GetBatteryLevel2", 2f);
			// Invoke("RequestMTU", 1f);
		});
	}
		
	
	/*public void InitPrefs()
	{
		UIModule.status = "初始化Rabboni參數";
		testLog.text += "初始化Rabboni參數: 2G\n";
		string cmd = "4503020112010100000000001009C4";
		byte[] data = StringToByteArray(cmd);
		// byte[] data = new byte[] { 0x49 };
		
		BluetoothLEHardwareInterface.WriteCharacteristic (targetAddress, PrefsServiceUUID, PrefsCharacteristic, data, data.Length, true, (characteristicUUID) => {

			UIModule.status = "Rabboni參數初始化成功";
			testLog.text += String.Format("Rabboni參數初始化: {0}\n", cmd);
			testLog.text += String.Format("等待: {0:F1}sec\n", 1f);	
			
			if(InvokeSubscribe2G)
			{
				Invoke("Subscribe", 1f);
			}
		});
	}*/
	
	
	public void SubscribePrefs()
	{
		statusStrLog.Invoke("訂閱Prefs回覆");	
		
		Invoke("RequestPrefs", 5f);
		
		BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (targetAddress, "fff0", "fff7", null, (address, characteristicUUID, bytes) => 
		{	
			string data = console.ByteArrayToString(bytes);
				
			statusStrLog.Invoke(data);				
		});
	}
	
	public void RequestPrefs()
	{
		statusStrLog.Invoke("讀取Rabboni參數");	
		byte[] data = new byte[] { 0x49 };
		
		BluetoothLEHardwareInterface.WriteCharacteristic (targetAddress, "fff0", "fff6", data, data.Length, false, (characteristicUUID) => 
		{
			// UIModule.status = "讀取Rabboni參數";
			// testLog.text += "讀取Rabboni參數\n";
			// testLog.text += String.Format("{0}\n", characteristicUUID);
			// Invoke("Subscribe", 5f);
		});
	}	
	
	public void OnSubscriveData(byte[] bytes)
	{
		string data = console.ByteArrayToString(bytes);
		
		int[] dataElement = new int[6];
		
		for(int i=0; i < 6; i++)
		{
			string tempHex = data.Substring(i*4, 4);
			short tempVal = Convert.ToInt16(tempHex, 16);
			dataElement[i] = tempVal;
		}
		
		lastAcc = new Vector3(dataElement[0], dataElement[1], dataElement[2]) * accScale / 32768f;
		lastGyro = new Vector3(dataElement[3], dataElement[4], dataElement[5]) * gyroScale / 32768f;
		
		IMUEvent.Invoke(lastAcc, lastGyro);

		if (UseKalman) 
		{
			rabboniKalman.UpdateIMU(lastAcc, lastGyro);
		}
	}
	
		
	public void Subscribe()
	{
		state = State.Subscribed;
		statusLog.Invoke( (int)state );	
		statusStrLog.Invoke("已連線");	
		
		BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (targetAddress, console.SubscribeServiceUUID, console.SubscribeCharacteristic, null, (address, characteristicUUID, bytes) => 
		{			
			if(console.moduleDic.ContainsKey(address) )
			{
				console.moduleDic[address].OnSubscriveData(bytes);
			}
		});
		
		connectedEvent.Invoke();
	}

	/*public void RequestMTU()
	{
		BluetoothLEHardwareInterface.RequestMtu(targetAddress, 185, (address, newMTU) =>
		{
			statusStrLog.Invoke("RequestMTU");

			//Invoke("Subscribe", 0.3f);
			Invoke("GetBatteryLevel", 1f);
			Invoke("GetBatteryLevel2", 5f);
			//Invoke("RequestMTU", 1f);
		});
	}*/
	
	public void GetBatteryLevel()
	{
		BluetoothLEHardwareInterface.ReadCharacteristic(targetAddress, "180f", "2a19", (characteristic, bytes) =>
		{			
			string tempHex = console.ByteArrayToString(bytes);
			// testLog.text += String.Format("BatteryLevel: {0}\n", tempHex);	
			
			short tempVal = Convert.ToInt16(tempHex, 16);
			
			batteryLevel = tempVal;		
			// testLog.text += String.Format("BatteryLevel(int): {0}\n", batteryLevel);	
			
			batteryLevelEvent.Invoke(batteryLevel);
		});
	}
}
