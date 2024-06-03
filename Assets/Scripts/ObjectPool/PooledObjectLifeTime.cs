using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOS.Tool
{
	public class PooledObjectLifeTime : PooledObject
	{
		public float lifeTime = 5f;
		
		void OnEnable()
		{
			StartCoroutine( Back(lifeTime) );
		}
		
		private IEnumerator Back(float delay)
		{
			yield return new WaitForSeconds(delay);
			BackToPool();
		}
	}
}