using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPile : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    List<Transform> bananaList = new List<Transform>();
    GameObject scoreTorus;
    int pingPongCounter;
	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        audioSource = GetComponent<AudioSource>();
        foreach (Transform tr in this.transform)
        {
            if (tr.CompareTag("Banana"))
            {                
                bananaList.Add(tr);
                tr.gameObject.AddComponent<Rigidbody>();
                tr.gameObject.SetActive(false);
            }
            else if(tr.name == "Torus")
            {
                //Debug.Log("Torus found: " + tr.name);
                scoreTorus = tr.gameObject;
                scoreTorus.SetActive(false);
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void IncrementScore()
    {
        if(bananaList.Count>0)
        {
            //Debug.Log("Activating " + bananaList[0].name);
            bananaList[0].gameObject.SetActive(true);
            bananaList.RemoveAt(0);
            audioSource.Play();
            animator.enabled = true;
            scoreTorus.SetActive(true);
            animator.Play("Score");
        }        
    }

    public void DisableTorus()
    {
        pingPongCounter++;
        //Debug.Log("Disable Torus called");
        if (pingPongCounter==3)
        {        
            animator.enabled = false;
            scoreTorus.SetActive(false);
        }
    }
}
