using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSystem
{
	public class GerstnerWavesCleaner : MonoBehaviour
	{
		public void CleanUp()
		{
			GerstnerWavesJobs.ReleaseRegistry();
		}
	}
}
