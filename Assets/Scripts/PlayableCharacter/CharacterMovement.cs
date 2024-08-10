using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Animator animator;
    private Vector2 movement;

    private List<Enum> buttonsPressed = new List<Enum>();

    [SerializeField] private Character character;
    private CharacterStats stats;
    [SerializeField] private Rigidbody2D rigidbody;

    private Collider2D collider;
    
    public Vector3 characterCenter;
    public Vector3 playerFacingDirection;

    public bool onBoat;

    public bool inWater;

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        rigidbody =  GetComponent<Rigidbody2D>();
        animator = character.GetAnimator();
        collider = GetComponent<Collider2D>();

        stats = character.GetStats(); // Reference to stats
    }

    public void FixedUpdate(){
        characterCenter = collider.bounds.center;        



    } 
    public void HandleMovement(Vector2 change, bool isLeftShiftPressed)
    {
        movement = Vector2.zero;
        movement.x = change.x;
        movement.y = change.y;

        if (change != Vector2.zero){
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

         if (movement == Vector2.zero){
            stats.RegainStamina(stats.StaminaDrainPerSecond/2 * Time.deltaTime);
        }
        
    }

    public void EnterWater(){
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

    // if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.standby 
    //                                       && currentState != PlayerState.swim && keyCount[0] == 0){
    //         cloneState = CloneState.attack;
    //         StartCoroutine(AttackCo());
    //         keyCount[0] ++;
    //     }
    //     else if (currentState == PlayerState.standby && onBoat){
    //         Debug.Log("Move boat");
    //         moveBoat();
    //     }
    //     else if (animator.GetBool("swimming")){
    //         cloneState = CloneState.swim;
    //         animator.SetFloat("moveX",change.x);
    //         animator.SetFloat("moveY",change.y);
    //         animator.SetBool("moving",true);
    //         movePlayer(false);
    //         playerFacingDirection.x = change.x;
    //         playerFacingDirection.y = change.y;
    //         playerFacingDirection.Normalize();
    //     }
    //     else if (change != Vector3.zero && currentState != PlayerState.standby && (currentState == PlayerState.walk || currentState == PlayerState.run)){
    //         // Debug.Log("Change = ("+change.x.ToString()+","+change.y.ToString());
    //         animator.SetFloat("moveX",change.x);
    //         animator.SetFloat("moveY",change.y);
    //         animator.SetBool("moving",true);
    //         movePlayer(Input.GetKey(KeyCode.LeftShift)&&(stamina >= 1));
    //         cloneState = (Input.GetKey(KeyCode.LeftShift)? CloneState.run : CloneState.walk);
    //         playerFacingDirection.x = change.x;
    //         playerFacingDirection.y = change.y;
    //         playerFacingDirection.Normalize();
    //     }
        
    //     else if (currentState != PlayerState.standby){
    //         cloneState = CloneState.walk;
    //         currentState = PlayerState.walk;
    //         animator.SetBool("moving",false);
    //         animator.SetBool("running",false);
    //     }
    //     else if (currentState == PlayerState.standby){
    //         // control bird
    //         if (astralProjecting){
    //             moveAstralProjection(change);
    //         }
    //     }
        
    //     playerMoveData.movement = change;
    //     // playerMoveData.buttonsPressed.Clear();
    //     playerMoveData.buttonsPressed = buttonsPressed.GetRange(0, buttonsPressed.Count); // Shallow copy
    //     playerMoveData.cloneState = cloneState;
    //     playerMoves.Enqueue(playerMoveData);

    //     try{
    //         buildSquareCellLocation = tilePalette.grid.WorldToCell(characterCenter+playerFacingDirection);
    //         buildSquare.transform.position = tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,1);
    //         noBuildSquare.transform.position = tilePalette.grid.CellToWorld(buildSquareCellLocation)+new Vector3(.5f,.5f,1);
    //     }
    //     catch{
    //         Debug.Log("Grid missing");
    //     }
    //     StartCoroutine("handleButtonPressesCo");

    // private IEnumerator handleButtonPressesCo(){



    //     foreach (var button in buttonsPressed){
    //         // Debug.Log("button pressed");

    //         switch(button){
    //             case KeyCode.T:
    //                 Debug.Log("torch");
    //                 torch.SetActive(!torch.activeSelf); 
    //                 setEnabledShadows(lightCount == 0 && !torch.activeSelf);
    //                 break;
    //             case KeyCode.E:
    //                 if (onBoat){
    //                     boat.onCharacterInteract();
    //                 }
    //                 else
    //                     StartCoroutine(interact("interact"));
    //                 break;
    //             case KeyCode.Q:
    //                 StartCoroutine(interact("dig"));
    //                 break;
    //             case KeyCode.C:
    //                 StartCoroutine(interact("chop"));
    //                 break;
    //             case KeyCode.F:
    //                 StartCoroutine(interact("build"));
    //                 break;
    //             case KeyCode.M:
    //                 StartCoroutine(interact("mine"));
    //                 break;
    //             case KeyCode.P:
    //                 Debug.Log("astralProjecting");
    //                 if (!astralProjecting){
    //                    astralProject(); 
    //                 }
    //                 else
    //                     returnProjectionToBody();
    //                 break;
    //         }
    //     }

    //     // reset button presses
    //     for (int i = 0; i <10; i++){
    //         if (keyCount[i] != 0)
    //             keyCount[i]+=1;
    //         if (keyCount[i]>=15)
    //             keyCount[i] = 0;
    //     }

    //     yield return null;
    



        
}