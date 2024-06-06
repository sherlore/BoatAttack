using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoatTeam : MonoBehaviour
{
	public Material matTeam1_Boy;
	public Material matTeam1_Girl;
	public Material matTeam2_Boy;
	public Material matTeam2_Girl;
	
	public Renderer[] boys;
	public Renderer[] girls;
	
    public void SetTeam(bool isTeam1)
    {
        if(isTeam1)
		{
			foreach(Renderer boy in boys)
			{
				boy.material = matTeam1_Boy;
			}
			foreach(Renderer girl in girls)
			{
				girl.material = matTeam1_Girl;
			}
		}
		else
		{
			foreach(Renderer boy in boys)
			{
				boy.material = matTeam2_Boy;
			}
			foreach(Renderer girl in girls)
			{
				girl.material = matTeam2_Girl;
			}
		}
    }
}
