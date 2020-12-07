using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBananaTrigger : MonoBehaviour
{
    public BananaPile scoreCounter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Banana")
        {
            StartCoroutine(WaitAndDisable(other.gameObject));
            scoreCounter.IncrementScore(); // currently this probably can be abused but whatever...
        }
    }

    IEnumerator WaitAndDisable(GameObject tobeDisabled)
    {
        yield return new WaitForSeconds(1f);
        Destroy(tobeDisabled);
    }
}
