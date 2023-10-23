using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
// using TMPro;

public class UIRabboniModule : MonoBehaviour
{
	// public string moduleName
	// {
		// set
		// {
			// moduleNameText.text = value;
		// }
	// }
	// public Text moduleNameText;
	
	public string targetAddress
	{
		set
		{
			targetAddressText.text = value;
		}
	}
	public Text targetAddressText;
	
	public string status
	{
		set
		{
			statusText.text = value;
		}
	}
	public Text statusText;
	
	public Image statusImg;	
		
	public GameObject buttonSet;
	public GameObject buttonLink;
	public GameObject buttonStopScan;
	
	// public bool enableRealtimeValueObservation
	// {
		// get
		// {
			// return realtimeValueObservationToggle.isOn && realtimeValueObservationToggle.gameObject.activeInHierarchy;
		// }
	// }
	// public Toggle realtimeValueObservationToggle;
	
	
	private float _maxAcc;
	public float maxAcc
	{
		get
		{
			return _maxAcc;
		}
		set
		{
			_maxAcc = value;
			
			// accXSlider.minValue = _maxAcc * -1f;
			// accXSlider.maxValue = _maxAcc;
			
			// accYSlider.minValue = _maxAcc * -1f;
			// accYSlider.maxValue = _maxAcc;
			
			// accZSlider.minValue = _maxAcc * -1f;
			// accZSlider.maxValue = _maxAcc;
		}
	}
	// public Vector3 acc
	// {
		// set
		// {
			// if(enableRealtimeValueObservation)
			// {
				// accXSlider.value = value.x;
				// accYSlider.value = value.y;
				// accZSlider.value = value.z;
				
				// accXText.text = String.Format("{0:F2}", value.x);
				// accYText.text = String.Format("{0:F2}", value.y);
				// accZText.text = String.Format("{0:F2}", value.z);
			// }
		// }
	// }
	// public Slider accXSlider;
	// public Slider accYSlider;
	// public Slider accZSlider;
	// public Text accXText;
	// public Text accYText;
	// public Text accZText;
	
	private float _maxGyro;
	public float maxGyro
	{
		get
		{
			return _maxGyro;
		}
		set
		{
			_maxGyro = value;
		}
	}
	// public Vector3 gyro
	// {
		// set
		// {
			// if(enableRealtimeValueObservation)
			// {
				// gyroXScrollbar.value = value.x/maxGyro;
				// gyroYScrollbar.value = value.y/maxGyro;
				// gyroZScrollbar.value = value.z/maxGyro;
				
				// gyroXText.text = String.Format("{0:F2}", value.x);
				// gyroYText.text = String.Format("{0:F2}", value.y);
				// gyroZText.text = String.Format("{0:F2}", value.z);
			// }
		// }
	// }
	// public GyroScrollbar gyroXScrollbar;
	// public GyroScrollbar gyroYScrollbar;
	// public GyroScrollbar gyroZScrollbar;
	// public Text gyroXText;
	// public Text gyroYText;
	// public Text gyroZText;
	
	public UnityEvent scanEvent;
	public UnityEvent scanAndConnectEvent;
	public UnityEvent stopScanEvent;
	
	public void Scan()
	{
		scanEvent.Invoke();
	}
	public void ScanAndConnect()
	{
		scanAndConnectEvent.Invoke();
	}
	public void StopScan()
	{
		stopScanEvent.Invoke();
	}
}
