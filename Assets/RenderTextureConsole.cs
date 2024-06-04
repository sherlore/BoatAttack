using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureConsole : MonoBehaviour
{
	public RenderTexture renderTexture1P;
	public RenderTexture renderTexture2P;
	
    // Start is called before the first frame update
    void Awake()
    {
        renderTexture1P.width = Screen.width/2;
        renderTexture1P.height = Screen.height;
		
        renderTexture2P.width = Screen.width/2;
        renderTexture2P.height = Screen.height;
    }
}
