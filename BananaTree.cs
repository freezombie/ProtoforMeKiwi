using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaTree : MonoBehaviour
{
    Animator animator;
    List<BananaBranch> bananaBranchList = new List<BananaBranch>();
	GameObject[] bananaBranchGOList;
    public int amountOfBananasDroppedOnHit;
    float buffer = 0.15f;
    bool allBananasDropped = false;
	int amountOfBananas=0;
    //int amountOfBananasDropped;
	// Use this for initialization
	void Start ()
    {
		bananaBranchGOList = GameObject.FindGameObjectsWithTag("BananaBranch");
		foreach(GameObject go in bananaBranchGOList)
		{
			bananaBranchList.Add(go.GetComponent<BananaBranch>());
			amountOfBananas += bananaBranchList[bananaBranchList.Count-1].GetAmountOfBananas();
		}
        //Debug.Log("Amount of Bananas: " + amountOfBananas);
        //Debug.Log("Amount of banana branches: " + bananaBranchList.Count);
        //Debug.Log("Amount of banana branch game objects: " + bananaBranchGOList.Length);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //Debug.Log("The collision between BananaTree and " + collision.gameObject.name + " had impulse magnitude of " + collision.impulse.magnitude + " and relative velocity magnitude of " + collision.relativeVelocity.magnitude);
		if(collision.gameObject.CompareTag("Stick") && collision.impulse.magnitude > 15 && amountOfBananas > 0)
        {            
			if(amountOfBananas > amountOfBananasDroppedOnHit)
                StartCoroutine(StartDroppingBananas(amountOfBananasDroppedOnHit));
            else
				StartCoroutine(StartDroppingBananas(amountOfBananas));

        }
        
    }

    IEnumerator StartDroppingBananas(int amountToDrop)
    {
        Debug.Log("Bananas to go: " + amountToDrop);
        yield return new WaitForSeconds(buffer);        
		DropABanana (null);
        amountToDrop--;
        if(amountToDrop>0)
            StartCoroutine(StartDroppingBananas(amountToDrop));                
    }

	public void DropABanana(BananaBranch bb)
	{
		if (bb == null) {			
			int random = Random.Range (0, bananaBranchList.Count);
			bananaBranchList [random].DropABanana ();
		} 
		else
		{
			bb.DropABanana ();
		}
		amountOfBananas--;
	}
}
