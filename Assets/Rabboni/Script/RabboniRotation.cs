using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboniRotation : MonoBehaviour
{
    public RabboniModule rabboniModule;
    public string deviceId;

    // Start is called before the first frame update
    void Start()
    {
        if (RabboniConsole.instance.listDic.ContainsKey(deviceId))
        {
            rabboniModule = RabboniConsole.instance.listDic[deviceId];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(rabboniModule != null && rabboniModule.UseKalman) 
        {
            transform.rotation = rabboniModule.KalmanRotation;
        }
    }
}
