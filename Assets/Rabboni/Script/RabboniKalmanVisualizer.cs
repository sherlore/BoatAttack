using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboniKalmanVisualizer : MonoBehaviour
{
    public RabboniModule rabboniModule;
    public string deviceId;

    public float roll;
    public float pitch;
    public Vector3 gravity;
    public Vector3 accZeroGravity;
    public float energy;
    public Vector3 calibratedAcc;
    public Vector3 calibratedGyro;

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
            roll = rabboniModule.Roll;
            pitch = rabboniModule.Pitch;
            gravity = rabboniModule.Gravity;
            accZeroGravity = rabboniModule.AccZeroGravity;
            energy = rabboniModule.Energy;
            calibratedAcc = rabboniModule.CalibratedAcc;
            calibratedGyro = rabboniModule.CalibratedGyro;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 targetPos = transform.position + gravity * 0.05f;
        Gizmos.DrawLine(transform.position, targetPos);
        Gizmos.DrawSphere(targetPos, 0.005f);

        /*Gizmos.color = Color.yellow;

        targetPos = transform.position + accZeroGravity * 0.1f;
        Gizmos.DrawLine(transform.position, targetPos);
        Gizmos.DrawSphere(targetPos, 0.005f);*/

        Gizmos.color = Color.red;

        targetPos = transform.position + rabboniModule.lastAcc * 0.1f;
        Gizmos.DrawLine(transform.position, targetPos);
        Gizmos.DrawSphere(targetPos, 0.005f);

    }
#endif
}
