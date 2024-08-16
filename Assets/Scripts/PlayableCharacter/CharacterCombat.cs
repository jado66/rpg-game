using UnityEngine;
using System.Collections;

public class CharacterCombat : MonoBehaviour
{
    private Animator animator;

    private Character character;
    private CharacterMovement movement;

    public GameObject upHitBox;
    public GameObject downHitBox;

    public GameObject rightHitBox;

    public GameObject leftHitBox;

    public float recoilForce = .5f; // This needs to be a property of the weapon
    

    public int damageDealt = 5;

    public float knockTime = .01f;

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        movement = character.GetMovement();
        animator = character.GetAnimator();

    }

    public IEnumerator Attack()
    {
        animator.SetTrigger("attack");

        // Deactivate all hitboxes first
        upHitBox.SetActive(false);
        rightHitBox.SetActive(false);
        downHitBox.SetActive(false);
        leftHitBox.SetActive(false);

        // Activate the appropriate hitbox based on movement.playerFacingDirection
        if (movement.playerFacingDirection.x > 0) {
            rightHitBox.SetActive(true);
        } else if (movement.playerFacingDirection.x < 0) {
            leftHitBox.SetActive(true);
        } else if (movement.playerFacingDirection.y > 0) {
            upHitBox.SetActive(true);
        } else if (movement.playerFacingDirection.y < 0) {
            downHitBox.SetActive(true);
        }
        yield return new WaitForSeconds(.5f); // Adjust this value to your needs

        // Deactivate all hitboxes after attack
        upHitBox.SetActive(false);
        rightHitBox.SetActive(false);
        downHitBox.SetActive(false);
        leftHitBox.SetActive(false);

        yield return null;
    }
}