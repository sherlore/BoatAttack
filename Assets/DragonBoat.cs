using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DragonBoat : MonoBehaviour
{
	public bool _isActive;
	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			_isActive = value;
		}
	}
	
    public float boatingPower = 100f;
    public float leftAxis;
    public float rightAxis;
	
	[Header("Rabboni")]
    public RabboniModule rabboniModuleL;
    public string deviceIdL;
    public RabboniModule rabboniModuleR;
    public string deviceIdR;

	[Header("Boat")]
    public Rigidbody boatRb;
    public Transform leftPowerPos;
    public Transform rightPowerPos;

    public float animSpeedCoef;
    public UnityEvent<float> leftAnimSpeed;
    public UnityEvent<float> rightAnimSpeed;
	
    public UnityEvent goalEvent;
	
    // Start is called before the first frame update
    void Start()
    {
		boatingPower = PlayerPrefs.GetFloat("BoatingPower", 3f);
		
        leftAnimSpeed.Invoke(0f);
        rightAnimSpeed.Invoke(0f);
		
		#if !UNITY_EDITOR
        if (RabboniConsole.instance.listDic.ContainsKey(deviceIdL))
        {
            rabboniModuleL = RabboniConsole.instance.listDic[deviceIdL];
			rabboniModuleL.IMUEvent.AddListener(UpdateEnergyL);
        }
		
        if (RabboniConsole.instance.listDic.ContainsKey(deviceIdR))
        {
            rabboniModuleR = RabboniConsole.instance.listDic[deviceIdR];
			rabboniModuleL.IMUEvent.AddListener(UpdateEnergyR);
        }
		#endif
    }

    // Update is called once per frame
    void Update()
    {
		if(!IsActive) return;
		
		#if UNITY_EDITOR
        if (leftAxis > 0f) 
        {
            var forward = boatRb.transform.forward;
            forward.y = 0f;
            forward.Normalize();
            boatRb.AddForceAtPosition(boatingPower * leftAxis * forward, leftPowerPos.position, ForceMode.Acceleration);
        }
		
        if (rightAxis > 0f) 
        {
            var forward = boatRb.transform.forward;
            forward.y = 0f;
            forward.Normalize();
            boatRb.AddForceAtPosition(boatingPower * rightAxis * forward, rightPowerPos.position, ForceMode.Acceleration);
        }		
		#endif
    }
	
	public void UpdateEnergyL(Vector3 acc, Vector3 gyro)
	{
		leftAxis = rabboniModuleL.Energy/16f;
		
        leftAnimSpeed.Invoke(leftAxis * animSpeedCoef);
		
		if(!IsActive) return;
		
        if (leftAxis > 0f) 
        {
            var forward = boatRb.transform.forward;
            forward.y = 0f;
            forward.Normalize();
            boatRb.AddForceAtPosition(boatingPower * leftAxis * forward, leftPowerPos.position, ForceMode.Acceleration);
        }

	}
	
	public void UpdateEnergyR(Vector3 acc, Vector3 gyro)
	{
		rightAxis = rabboniModuleR.Energy/16f;

        rightAnimSpeed.Invoke(rightAxis * animSpeedCoef);
		
		if(!IsActive) return;
		
        if (rightAxis > 0f) 
        {
            var forward = boatRb.transform.forward;
            forward.y = 0f;
            forward.Normalize();
            boatRb.AddForceAtPosition(boatingPower * rightAxis * forward, rightPowerPos.position, ForceMode.Acceleration);
        }	
	}
	
    public void LeftBoating(InputAction.CallbackContext context)
    {
        leftAxis = context.ReadValue<float>();

        leftAnimSpeed.Invoke(leftAxis * animSpeedCoef);
    }
	
    public void RightBoating(InputAction.CallbackContext context)
    {
        rightAxis = context.ReadValue<float>();

        rightAnimSpeed.Invoke(rightAxis * animSpeedCoef);
    }
	
	private void OnTriggerEnter(Collider other)
    {
		// Debug.Log("OnTriggerEnter: " + other.name);
		// Debug.Log("Tag: " + other.tag);
		
        if(IsActive && other.CompareTag("Goal"))
		{
			goalEvent.Invoke();
		}
    }
}
