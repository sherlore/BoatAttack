using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOS.Tool
{
	public class ObjectPoolSystem : MonoBehaviour
	{
		public PooledObject prefab; 
		private List<GameObject> pool = new List<GameObject>();
	 
		public GameObject GetPooledInstance (Transform parent) 
		{
			lock (pool) 
			{
				int lastIndex = pool.Count - 1;
				if (lastIndex >= 0) 
				{
					GameObject obj = pool[lastIndex];
					pool.RemoveAt (lastIndex);
					
					if (obj.transform.parent != parent) 
					{
						obj.transform.SetParent (parent);
					}
					
					//SetActive after parenting to avoid unnecessary dirty
					obj.SetActive (true);
					
					return obj;
				} 
				else 
				{
					GameObject obj = Instantiate<GameObject> (prefab.gameObject, parent);
					
					PooledObject poolObj = obj.GetComponent<PooledObject>();
					poolObj.pool = this;
					obj.SetActive (true);
					
					return obj;
				}
			}
		}
		
		public GameObject GetPooledInstance (Transform parent, bool active) 
		{
			lock (pool) 
			{
				int lastIndex = pool.Count - 1;
				if (lastIndex >= 0) 
				{
					GameObject obj = pool[lastIndex];
					pool.RemoveAt (lastIndex);
					
					if (obj.transform.parent != parent) 
					{
						obj.transform.SetParent (parent);
					}
					
					//SetActive after parenting to avoid unnecessary dirty
					if(active)
					{
						obj.SetActive (true);
					}
					
					return obj;
				} 
				else 
				{
					GameObject obj = Instantiate<GameObject> (prefab.gameObject, parent);
					
					PooledObject poolObj = obj.GetComponent<PooledObject>();
					poolObj.pool = this;
					
					if(active)
					{
						obj.SetActive (true);
					}
					
					return obj;
				}
			}
		}
	 
		public void BackToPool (GameObject obj) 
		{
			lock (pool) 
			{
				pool.Add (obj);
				obj.SetActive (false);
				
				obj.transform.SetParent(transform);
			}
		}
	}
}