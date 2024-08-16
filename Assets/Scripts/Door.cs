using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject openSprite;
    public GameObject closedSprite;
    public GameObject linkedTeleport;

    public bool startOpen;
    public bool isLocked;
    public int keyID;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = startOpen;

        if (isOpen)
        {
            closedSprite.SetActive(false);
            openSprite.SetActive(true);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            closedSprite.SetActive(true);
            openSprite.SetActive(false);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        SetDoorState(isOpen);

        if (isOpen)
        {
            StartCoroutine(CloseDoorAfterDelay(1f));
        }
    }

    void SetDoorState(bool open)
    {
        if (open)
        {
            closedSprite.SetActive(false);
            openSprite.SetActive(true);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            closedSprite.SetActive(true);
            openSprite.SetActive(false);
            linkedTeleport.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isOpen = false;
        SetDoorState(isOpen);
    }

    // Update is called once per frame
    public override void OnCharacterInteract()
    {
        if (!isLocked || isOpen)
        {
            ToggleDoor();
        }
    }
}
