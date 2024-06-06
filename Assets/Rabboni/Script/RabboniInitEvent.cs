using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RabboniInitEvent : MonoBehaviour
{
	public UnityEvent initializeSuccessEvent;
	public UnityEvent<string> initializeFailedLog;
	
    // Start is called before the first frame update
    void Start()
    {
        RabboniConsole.instance.initializeSuccessEvent.AddListener(TriggerSuccess);
        RabboniConsole.instance.initializeFailedLog.AddListener(TriggerFailed);
    }
	
    void OnDestroy()
    {
        RabboniConsole.instance.initializeSuccessEvent.RemoveListener(TriggerSuccess);
        RabboniConsole.instance.initializeFailedLog.RemoveListener(TriggerFailed);
    }

    public void TriggerSuccess()
    {
        initializeSuccessEvent.Invoke();
    }

    public void TriggerFailed(string log)
    {
        initializeFailedLog.Invoke(log);
    }
}
