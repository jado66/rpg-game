using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : Lever
{
    public bool startPulled;
    public List<LeverPuzzle> connectedLevers = new List<LeverPuzzle>();
    // Start is called before the first frame update

    void Start(){
        if (startPulled)
            toggleSingleLever();
    }
    public override void toggleLever()
    {
        base.toggleLever();
        foreach(var lever in connectedLevers){
            lever.toggleSingleLever();
        }
    }

    public void toggleSingleLever()
    {
        base.toggleLever();
    }
}
