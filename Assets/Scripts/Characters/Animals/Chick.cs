using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chick : Animal
{

    public GameObject chickenPrefab; // Assign this in the editor
    public GameObject henPrefab;
    SceneManager sceneManager;

    int startingDay;
    int daysToTransform;


    void Start()
    {
        sceneManager = FindObjectOfType<SceneManager>();
        startingDay = sceneManager.day;

        daysToTransform = 1; // Random.Range(3, 7);


        SceneManager.OnDayChanged += OnDayChanged;

        
    }

    private void OnDestroy()
    {
        SceneManager.OnDayChanged -= OnDayChanged;
    }

    private void OnDayChanged()
    {
        int currentDay = sceneManager.day;
        if (currentDay - startingDay >= daysToTransform)
        {
            TransformToAdult();
        }
    }

    private void TransformToAdult()
    {
        // Randomly select either the Chicken or Hen prefab
        GameObject adultPrefab = UnityEngine.Random.value > 0.45 ? chickenPrefab : henPrefab;
        
        GameObject adultInstance = Instantiate(adultPrefab, transform.position, transform.rotation);

        Animal animalComponent = adultInstance.GetComponent<Animal>();
        if (animalComponent != null)
        {
            animalComponent.id = GenerateUUID();
        }

        Destroy(gameObject); // Remove the chick from the scene
    }

    private string GenerateUUID()
    {
        return System.Guid.NewGuid().ToString();
    }
    
}
