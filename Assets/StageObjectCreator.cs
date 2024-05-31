using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectCreator : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;

    public float createLength;
    public float gap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateObjects() 
    {
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward;

        float length = 0f;

        while(length < createLength) 
        {
            Instantiate(prefab, startPos+direction*length, prefab.transform.rotation, parent);
            length += gap;
        }
    }
}
