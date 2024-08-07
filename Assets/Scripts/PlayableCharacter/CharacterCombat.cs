using UnityEngine;
using System.Collections.Generic;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;

    private Character character;
    private CharacterInventory inventory;

    public GameObject upHitBox;
    public GameObject downHitBox;

    public GameObject rightHitBox;

    public GameObject leftHitBox;

    public void InitializeComponents(Character characterRef){
        character = characterRef;
        inventory = character.GetInventory();
    }

    public void HandleCombat()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Attack();
        }
    }

    private void Attack()
    {
        GameItem equippedItem = inventory.GetEquippedItem();
        if (equippedItem != null && equippedItem is IWeapon weapon)
        {
            weapon.Use();
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>()?.TakeDamage(weapon.Damage);
            }
        }
    }
}