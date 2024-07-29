using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public GameObject contents;
    bool cracked;

    private IEnumerator waitAndBreak(){
        GetComponent<BoxCollider2D>().isTrigger = true;
        Instantiate(contents,transform.position,Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
        yield return null;
    }
    public void OnHit(){
        Debug.Log("hit pot");
        if (!cracked){
            GetComponent<Animator>().SetBool("cracked",true);
            cracked = true;
        }
        else{
            GetComponent<Animator>().SetTrigger("break");
            StartCoroutine(waitAndBreak());
        }
    }
}
