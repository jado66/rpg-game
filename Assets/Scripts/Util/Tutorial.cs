using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Tutorial : MonoBehaviour
{
    // Number of tutorial tasks
    public int tutorialTasks;

    // Method to set the initial number of tasks (optional)
    public void SetTutorialTasks(int tasks)
    {
        tutorialTasks = tasks;
    }

    // Public method to finish a task
    public void FinishATask()
    {
        if (tutorialTasks > 0)
        {
            tutorialTasks--;

            if (tutorialTasks == 0)
            {
                ToastNotification.Instance.Toast("no-tutorial-tasks", "There are no tasks left!");
                StartCoroutine(ShowDelayedToast());
            }
            else
            {
                ToastNotification.Instance.Toast("no-tutorial-tasks", $"You have {tutorialTasks} tutorial tasks left");
            }
        }
        else
        {
            Debug.Log("There are no tasks left to complete.");
        }
    }

    private IEnumerator ShowDelayedToast()
    {
        yield return new WaitForSeconds(3);
        ToastNotification.Instance.Toast("exit-tutorial", "Exit the portal to leave!");
    }
}
