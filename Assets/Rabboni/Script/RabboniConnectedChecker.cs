using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RabboniConnectedChecker : MonoBehaviour
{
	public RabboniModulePack[] rabboniModulePacks;
	
    public UnityEvent<bool> connectedEvent;
	
	void Start()
	{
		for(int i=0; i<rabboniModulePacks.Length; i++)
		{
			if (RabboniConsole.instance.listDic.ContainsKey(rabboniModulePacks[i].deviceId))
			{
				rabboniModulePacks[i].rabboniModule = RabboniConsole.instance.listDic[rabboniModulePacks[i].deviceId];
			}
		}
	}

    void Update()
    {
		bool isConnected = IsConnected();
        connectedEvent.Invoke(isConnected);
    }
	
	public bool IsConnected()
	{
		for(int i=0; i<rabboniModulePacks.Length; i++)
		{
			if(rabboniModulePacks[i] == null || !rabboniModulePacks[i].rabboniModule.isConnected)
			{
				return false;
			}
		}
		return true;
	}
}
