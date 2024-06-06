using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerConsole : MonoBehaviour
{
    public Transform goalTransform;
    public Transform boatTransform;
    public float maxDistance;
    public float sideOffset;
    public UnityEvent<float> distanceEvent;

    // Start is called before the first frame update
    void Start()
    {
        maxDistance = PlayerPrefs.GetFloat("MaxRaceDistance", 700f);
		
		StartCoroutine(InitStartPos());
		
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(goalTransform.position, boatTransform.position);

        distanceEvent.Invoke( 1f - dis/maxDistance );
    }
	
	public void ResetPosition()
	{
		Debug.Log("ResetPosition");
		
		float dis = Vector3.Distance(goalTransform.position, boatTransform.position);
		
		float resetDistance = dis+20f;
		
		if(resetDistance > maxDistance)
		{
			resetDistance = maxDistance;
		}
		
		boatTransform.position = goalTransform.position + goalTransform.forward * resetDistance * -1f;
		boatTransform.rotation = goalTransform.rotation;
	}
	
	IEnumerator InitStartPos()
    {
        //Delay
		yield return new WaitForSeconds(0.1f);
		
		boatTransform.position = goalTransform.position + goalTransform.forward * maxDistance * -1f + goalTransform.right * sideOffset;
		boatTransform.rotation = goalTransform.rotation;
    }
}
