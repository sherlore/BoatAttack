using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RabboniConsole : MonoBehaviour
{
	public static RabboniConsole instance;
	// public UIRabboniConsole UIModule;
	
    // public string[] DeviceName;
    public string[] DeviceNameRabboni;
    public string[] DeviceNameNaxsen;
	
	public enum DeviceType
	{
		Rabboni,
		Naxsen
	}
	
	[Header("GATT")]
	public string SubscribeServiceUUID = "1600";
	public string SubscribeCharacteristic = "1601";
	
	public string PrefsServiceUUID = "fff0";
	public string PrefsCharacteristic = "fff6";
	
	[Header("Intialize")]
	public bool isIntialized;
	public UnityEvent initializeSuccessEvent;
	public UnityEvent<string> initializeFailedLog;
	
	public Dictionary<string, RabboniModule> moduleDic = new Dictionary<string, RabboniModule>();
	
	[Header("ModuleList")]
	public RabboniModule[] rabboniModules;
	public Dictionary<string, RabboniModule> listDic;
		
	public enum AccScale
	{
		acc2G,
		acc4G,
		acc8G,
		acc16G
	}	
	public AccScale accScale;
		
	public enum GyroScale
	{
		gyro250,
		gyro500,
		gyro1000,
		gyro2000
	}
	
	public GyroScale gyroScale;
		
	public enum DataRate
	{
		sample10hz,
		sample20hz,
		sample50hz,
		sample100hz,
		sample200hz,
		sample500hz,
		sample1000hz
	}
	public DataRate dataRate;
		
	public float GetAccScaleValue()
	{
		switch(accScale)
		{
			case AccScale.acc2G:
				return 2f;
			case AccScale.acc4G:
				return 4f;
			case AccScale.acc8G:
				return 8f;
			case AccScale.acc16G:
				return 16f;
			default:
				return 2f;
		}
	}
	
	public string GetAccScaleCmd()
	{
		switch(accScale)
		{
			case AccScale.acc2G:
				return "00";
			case AccScale.acc4G:
				return "01";
			case AccScale.acc8G:
				return "02";
			case AccScale.acc16G:
				return "03";
			default:
				return "00";
		}
	}
		
	public void SetAccScale(int val)
	{
		switch(val)
		{
			case (int)AccScale.acc2G:
				accScale = AccScale.acc2G;
				break;
			case (int)AccScale.acc4G:
				accScale = AccScale.acc4G;
				break;
			case (int)AccScale.acc8G:
				accScale = AccScale.acc8G;
				break;
			case (int)AccScale.acc16G:
				accScale = AccScale.acc16G;
				break;
			default:
				accScale = AccScale.acc2G;
				break;
		}
		
		PlayerPrefs.SetInt("accScale", (int)accScale);
	}	
		
	public float GetGyroScaleValue()
	{
		switch(gyroScale)
		{
			case GyroScale.gyro250:
				return 250f;
			case GyroScale.gyro500:
				return 500f;
			case GyroScale.gyro1000:
				return 1000f;
			case GyroScale.gyro2000:
				return 2000f;
			default:
				return 250f;
		}
	}
	
	public string GetGyroScaleCmd()
	{
		switch(gyroScale)
		{
			case GyroScale.gyro250:
				return "00";
			case GyroScale.gyro500:
				return "01";
			case GyroScale.gyro1000:
				return "02";
			case GyroScale.gyro2000:
				return "03";
			default:
				return "00";
		}
	}
	
	public void SetGyroScale(int val)
	{
		switch(val)
		{
			case (int)GyroScale.gyro250:
				gyroScale = GyroScale.gyro250;
				break;
			case (int)GyroScale.gyro500:
				gyroScale = GyroScale.gyro500;
				break;
			case (int)GyroScale.gyro1000:
				gyroScale = GyroScale.gyro1000;
				break;
			case (int)GyroScale.gyro2000:
				gyroScale = GyroScale.gyro2000;
				break;
			default:
				gyroScale = GyroScale.gyro250;
				break;
		}
		
		PlayerPrefs.SetInt("gyroScale", (int)gyroScale);
	}
		
	public float GetDataRateValue()
	{
		switch(dataRate)
		{
			case DataRate.sample10hz:
				return 10f;
			case DataRate.sample20hz:
				return 20f;
			case DataRate.sample50hz:
				return 50f;
			case DataRate.sample100hz:
				return 100f;
			case DataRate.sample200hz:
				return 200f;
			case DataRate.sample500hz:
				return 500f;
			case DataRate.sample1000hz:
				return 1000f;
			default:
				return 50f;
		}
	}
	
	public string GetDataRateCmd()
	{
		switch(dataRate)
		{
			case DataRate.sample10hz:
				return "00";
			case DataRate.sample20hz:
				return "01";
			case DataRate.sample50hz:
				return "02";
			case DataRate.sample100hz:
				return "03";
			case DataRate.sample200hz:
				return "04";
			case DataRate.sample500hz:
				return "05";
			case DataRate.sample1000hz:
				return "06";
			default:
				return "02";
		}
	}
	
	public void SetDataRate(int val)
	{
		switch(val)
		{
			case (int)DataRate.sample10hz:
				dataRate = DataRate.sample10hz;
				break;
			case (int)DataRate.sample20hz:
				dataRate = DataRate.sample20hz;
				break;
			case (int)DataRate.sample50hz:
				dataRate = DataRate.sample50hz;
				break;
			case (int)DataRate.sample100hz:
				dataRate = DataRate.sample100hz;
				break;
			case (int)DataRate.sample200hz:
				dataRate = DataRate.sample200hz;
				break;
			case (int)DataRate.sample500hz:
				dataRate = DataRate.sample500hz;
				break;
			case (int)DataRate.sample1000hz:
				dataRate = DataRate.sample1000hz;
				break;
			default:
				dataRate = DataRate.sample10hz;
				break;
		}
		
		PlayerPrefs.SetInt("dataRate", (int)dataRate);
	}
	
	public string GetDeviceName(DeviceType val)
	{
		if(val == DeviceType.Rabboni)
		{
			return "Rabboni";
		}
		else if(val == DeviceType.Naxsen)
		{
			return "Naxsen";
		}
		else
		{
			return "Unknown";
		}
	}
	
	public byte[] StringToByteArray(string hex)
	{
		int NumberChars = hex.Length;
		byte[] bytes = new byte[NumberChars / 2];
		for (int i = 0; i < NumberChars; i += 2)
		{
			bytes[i / 2] = System.Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return bytes;
	}
	
	public string ByteArrayToString(byte[] ba)
	{
	  return System.BitConverter.ToString(ba).Replace("-","");
	}
	
	public string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
	}
	
	public bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = FullUUID (uuid2);

		return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
	}
	
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			
			listDic = new Dictionary<string, RabboniModule>();
			
			for(int i=0; i<rabboniModules.Length; i++)
			{
				listDic[ rabboniModules[i].deviceId ] = rabboniModules[i];
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	void Start()
    {
		// UIModule.CreateDropdownOptions(UIModule.accDropdown, typeof(AccScale), "acc");
		// UIModule.CreateDropdownOptions(UIModule.gyroDropdown, typeof(GyroScale), "gyro");
		// UIModule.CreateDropdownOptions(UIModule.dataRateDropdown, typeof(DataRate), "sample");
		
		// accScale = (AccScale)PlayerPrefs.GetInt("accScale", 0);
		// UIModule.accDropdown.SetValueWithoutNotify( (int)accScale );
		
		// gyroScale = (GyroScale)PlayerPrefs.GetInt("gyroScale", 2);
		// UIModule.gyroDropdown.SetValueWithoutNotify( (int)gyroScale );
		
		// dataRate = (DataRate)PlayerPrefs.GetInt("dataRate", 2);
		// UIModule.dataRateDropdown.SetValueWithoutNotify( (int)dataRate );
						
		Initialize();
    }
	
	public void Initialize()
	{
		// initializePanel.SetActive(false);
		// initializeBtn.SetActive(false);
		// statusStrLog.Invoke("正在初始化藍牙...");
		
		BluetoothLEHardwareInterface.Initialize (true, false, () => 
		{
			// initializePanel.SetActive(false);
			// initializeBtn.SetActive(false);
			
			isIntialized = true;
			initializeSuccessEvent.Invoke();

		}, (error) => 
		{
			// statusStrLog.Invoke("初始化失敗，請檢察藍牙是否開啟");
			// initializePanel.SetActive(true);
			// initializeBtn.SetActive(true);
			
			isIntialized = false;
			initializeFailedLog.Invoke(error);
			
			BluetoothLEHardwareInterface.Log ("Error: " + error);
		});
	}
   	

	
}
