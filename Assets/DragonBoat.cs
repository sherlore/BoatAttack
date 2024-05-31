using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DragonBoat : MonoBehaviour
{
    public float boatingPower = 100f;
    public float leftAxis;
    public float rightAxis;

    public Rigidbody boatRb;
    public Transform leftPowerPos;
    public Transform rightPowerPos;

    public float animSpeedCoef;
    public UnityEvent<float> leftAnimSpeed;
    public UnityEvent<float> rightAnimSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
