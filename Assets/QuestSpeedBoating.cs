using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestSpeedBoating : MonoBehaviour
{
    public bool isGameStart;

    public Transform goalTransform;
    public Transform boatTransform;

    public float stepDistance = 1f;
    public float lastDistance;
    public UnityEvent<int> stepEvent;

    public void StartGame()
    {
        isGameStart = true;

        Vector3 dir = goalTransform.position - boatTransform.position;
        float projectDis = Vector3.Project(dir, goalTransform.forward).magnitude;
        lastDistance = projectDis;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStart) return;

        Vector3 dir = goalTransform.position - boatTransform.position;
        float projectDis = Vector3.Project(dir, goalTransform.forward).magnitude;

        if (lastDistance - projectDis > stepDistance )
        {
            int progressDis = Mathf.FloorToInt(lastDistance - projectDis);

            stepEvent.Invoke(progressDis);

            lastDistance -= progressDis;
        }
    }
}
