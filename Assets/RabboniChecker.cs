using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboniChecker : MonoBehaviour
{
	[System.Serializable]
	public class Condition
	{
		public ConditionPair[] conditionPairs;
	}
	
	[System.Serializable]
	public class ConditionPair
	{
		public string rabboniId;
		public RabboniModule rabboniModule;
	}
	
	public Condition condition;
	
	public GameObject startBtnObj;
	public GameObject startHintObj;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
	{
		#if UNITY_EDITOR
		startBtnObj.SetActive(true);
		startHintObj.SetActive(false);
		#else
		if(CheckConditionReady())
		{
			startBtnObj.SetActive(true);
			startHintObj.SetActive(false);
		}
		else
		{
			startBtnObj.SetActive(false);
			startHintObj.SetActive(true);
		}
		#endif
	}
	
	public bool CheckConditionReady()
	{
		for(int i=0; i<condition.conditionPairs.Length; i++)
		{
			ConditionPair conditionPair = condition.conditionPairs[i];
			
			if(conditionPair.rabboniModule == null)
			{
				conditionPair.rabboniModule = RabboniConsole.instance.listDic[conditionPair.rabboniId];
			}
			
			if(!conditionPair.rabboniModule.isConnected)
			{
				return false;
			}
		}
		
		return true;
	}
}
