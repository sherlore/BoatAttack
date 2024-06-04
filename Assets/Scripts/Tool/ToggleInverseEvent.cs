using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleInverseEvent : MonoBehaviour
{
    public UnityEvent<bool> inverseEvent;

    public void TriggerEvent(bool isOn)
    {
        inverseEvent.Invoke(!isOn);
    }
}
