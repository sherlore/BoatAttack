using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleEvent : MonoBehaviour
{
	public UnityEvent eventOn;
	public UnityEvent eventOff;
	
    public void TriggerEvent(bool isOn)
	{
		if(isOn)
		{
			eventOn.Invoke();
		}
		else
		{
			eventOff.Invoke();
		}
	}
}
