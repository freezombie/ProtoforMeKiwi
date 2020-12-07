using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBranch : MonoBehaviour 
{
	List<Banana> bananaList = new List<Banana> ();
	int indexHelper = 0;
	// Use this for initialization
	void Start () 
	{
		foreach (Transform tr in this.transform) 
		{
			if (tr.CompareTag ("Banana")) 
			{
				tr.gameObject.AddComponent<Banana>();
				Banana banana = tr.gameObject.GetComponent<Banana>();
				banana.SetIndex(indexHelper);
				indexHelper++;
				bananaList.Add(banana);
                MeshCollider col = tr.gameObject.AddComponent<MeshCollider>();
                col.convex = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public int GetAmountOfBananas()
	{
		return bananaList.Count;
	}

	public void DropABanana()
	{
		int random = Random.Range (0, bananaList.Count);
		bananaList [random].Drop ();
		bananaList.RemoveAt (random);
	}
}
