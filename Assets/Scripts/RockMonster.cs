using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonster : Monster
{
    void Awake()
    {
        addAnimator(transform.GetChild(0).GetComponent<Animator>());
        addAnimator(transform.GetChild(1).GetComponent<Animator>());
        
    }
}
