﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpinner : MonoBehaviour
{
	public float speed;
	
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
