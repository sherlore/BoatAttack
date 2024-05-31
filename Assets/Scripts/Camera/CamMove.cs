using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamMove : MonoBehaviour 
{
	public Vector2 moveVector;
	public float liftVal;
	public Vector2 turnVector;

	public float scaleH;
	public float scaleV;
	public float scaleL;
	public float scaleR;
	public float scaleRv;
	public Transform childCam;
	
	public Transform target;
	public Vector3 offest;
	
	public bool _tracking;
	public bool tracking
	{
		get
		{
			return _tracking;
		}
		set
		{
			/*if(!_tracking)
			{
				originPos = transform.position;
				originRot = childCam.rotation;			
			}
			else
			{
				transform.position = originPos;
				childCam.rotation = originRot;				
			}*/
			
			_tracking = value;
		}
	}
	public Vector3 originPos;
	public Quaternion originRot;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float h = moveVector.x;
		float v = moveVector.y;
		float l = liftVal;
		transform.Translate(Vector3.right * h * Time.unscaledDeltaTime * scaleH + Vector3.forward * v * Time.unscaledDeltaTime * scaleV + Vector3.up * l * Time.unscaledDeltaTime *  scaleL);
		
		float r = turnVector.x;
		transform.Rotate(Vector3.up * r * Time.unscaledDeltaTime * scaleR);
		float rv = turnVector.y;
				
		childCam.Rotate(Vector3.right * rv * Time.unscaledDeltaTime * scaleRv *-1f);
		
		/*if(!tracking && Input.GetButtonDown("Jump") )
		{
			tracking = true;
		}*/
		
		if(tracking)
		{
			transform.position = target.position + offest;
			childCam.LookAt(target);
			
			/*if(Input.GetButtonUp("Jump") )
			{
				tracking = false;
			}*/
		}
		
		
	}

	public void Move(InputAction.CallbackContext context) 
	{
        moveVector = context.ReadValue<Vector2>();

    }

    public void Lift(InputAction.CallbackContext context)
    {
        liftVal = context.ReadValue<float>();

    }

    public void Turn(InputAction.CallbackContext context)
    {
        turnVector = context.ReadValue<Vector2>();

    }
}
