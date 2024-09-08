using System;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    private Animator animator;
    private Vector2 movement;

    private List<Enum> buttonsPressed = new List<Enum>();

    [SerializeField] private Character character;

    private CharacterActions actions;
    private CharacterStats stats;
    [SerializeField] private Rigidbody2D rigidbody;

    private Collider2D collider;
    
    public Vector3 characterCenter;
    public Vector3 playerFacingDirection;

    public bool onBoat;

    public bool inWater;

    public float timeSinceLastTeleport = 0f;
    public float teleportCooldown = 2f; // Adjust this value as needed

    private CharacterWorldInteraction interactions;

    [SerializeField] private float movementThreshold = 0.25f;
    [SerializeField] private float closingDelay = 0.5f;
    private Coroutine closingCoroutine;

    private TooltipUI tooltip;

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        actions =   character.GetActions();
        rigidbody =  GetComponent<Rigidbody2D>();
        interactions = character.GetWorldInteraction();
        animator = character.GetAnimator();
        collider = GetComponent<Collider2D>();
        stats = character.GetStats(); 
        tooltip = GameObject.Find("Tooltip").GetComponent<TooltipUI>();
    }

    public void FixedUpdate(){
        characterCenter = collider.bounds.center; 
        timeSinceLastTeleport += Time.fixedDeltaTime; // Update the timer
    } 

    public void ResetTeleportTimer()
    {
        timeSinceLastTeleport = 0f; // Reset the timer
    }

    public bool CanTeleport()
    {
        return timeSinceLastTeleport >= teleportCooldown; // Check cooldown
    }

    public void HandleMovement(Vector2 change, bool isLeftShiftPressed)
    {
        movement = Vector2.zero;

        movement.x = change.x;
        movement.y = change.y;
       
        if (change != Vector2.zero){

            if (interactions.chestLocation.HasValue){
                Vector3 chestLocationValue = interactions.chestLocation.Value;

                float distanceMoved = Vector3.Distance(chestLocationValue, characterCenter);

                if (distanceMoved >= movementThreshold)
                {
                    HandleClosingExternalInventory();
                    interactions.chestLocation = null;
                }
            }

            playerFacingDirection.x = change.x;
            playerFacingDirection.y = change.y;
            playerFacingDirection.Normalize();
        }

        bool isSprinting = isLeftShiftPressed && stats.Stamina > 0;

        if (inWater){ // TODO This is backwards. Something making this backwards
            Move(isSprinting);
        } else {
            Swim(isSprinting);
        }

        HandleAnimation(isSprinting);

        if (!isSprinting){
            if (movement == Vector2.zero){
                stats.RegainStamina(stats.StaminaDrainPerSecond/2 * Time.deltaTime);
            }
            else{
                stats.RegainStamina(stats.StaminaDrainPerSecond/4 * Time.deltaTime);
            }
        }
    }

    private void HandleClosingExternalInventory()
    {
        tooltip.HideTooltip();
        Chest chest = interactions.GetOpenChest();
        Store store = interactions.GetOpenStore();
        Sign sign = interactions.GetOpenSign();

        if (sign != null)
        {
            sign.HideSignText();
        }
        if (chest != null)
        {
            interactions.CloseOpenChest();
        }
        if (store != null)
        {
            interactions.CloseOpenStore();
        }
    }

    public void EnterWater(){
        Debug.Log("Enter water");
        inWater = true;
        animator.SetBool("swimming", false); //TODO only works backwards??
    }

    public void ExitWater(){
        inWater = false;
        animator.SetBool("swimming", true); //TODO only works backwards??
    }

    private void Swim(bool isSprinting)
    {
        float currentSpeed = isSprinting ? stats.FastSwimSpeedMultiplier * stats.SwimSpeed : stats.SwimSpeed;
        rigidbody.MovePosition(rigidbody.position + movement.normalized * currentSpeed * Time.fixedDeltaTime);
    
        if (isSprinting){
            stats.DepleteStamina(stats.StaminaDrainPerSecond * Time.deltaTime);
        }
    }
    private void Move(bool isSprinting)
    {
        float currentSpeed = isSprinting ? stats.RunSpeedMultiplier * stats.Speed : stats.Speed;
        rigidbody.MovePosition(rigidbody.position + movement.normalized * currentSpeed * Time.fixedDeltaTime);
    
        if (isSprinting && movement != Vector2.zero){
            stats.DepleteStamina(stats.StaminaDrainPerSecond * Time.deltaTime);
        }
    }

    private void HandleAnimation(bool isSprinting){

        if (movement != Vector2.zero){
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
            animator.SetBool("moving", true);
            animator.SetBool("running", isSprinting);

        }
        else{
            animator.SetBool("moving", false);
            animator.SetBool("running", false);
        }
    }

    private void OnDrawGizmos()
    {
        if (collider != null)
        {
            // Draw character center as a red sphere
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(collider.bounds.center, 0.1f);

            // Draw player facing direction as a green line
            Gizmos.color = Color.green;
            Gizmos.DrawLine(characterCenter, characterCenter + playerFacingDirection);
        }
    }
}