using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePortal : MonoBehaviour
{
    public float rotationSpeed = 100f;
    
    void Update()
    {
        // Calculate the rotation amount based on time elapsed since last frame and rotation speed
        float rotationAmount = rotationSpeed * Time.deltaTime;
        
        // Apply the rotation to the transform's z-axis (2D sprite rotation)
        transform.Rotate(0, 0, rotationAmount);
    }
}
