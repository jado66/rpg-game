using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhideUi : MonoBehaviour
{
    // Reference to the Canvas component
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        // Unhide the Canvas by enabling it
        if (canvas != null)
        {
            canvas.enabled = true;
        }
        else
        {
            Debug.LogWarning("Canvas component is not assigned.");
        }
    }
}
