using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboniDic : MonoBehaviour
{
	public static RabboniDic instance;
	
	public RabboniModule[] rabboniModules;
	public Dictionary<string, RabboniModule> dic;
	
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			
			dic = new Dictionary<string, RabboniModule>();
			
			for(int i=0; i<rabboniModules.Length; i++)
			{
				dic[ rabboniModules[i].deviceId ] = rabboniModules[i];
			}
			
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(this);
		}
	}
}
