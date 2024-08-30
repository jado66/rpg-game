using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InventoryItem
{
    public string KeyId { get; private set; }

    public Key(string id, string name, string description, int value, string keyId) 
        : base(id, name, description, Uses.Consumable, 
               new Dictionary<string, int>(), value, new List<string>(), new List<string>())
    {
        KeyId = keyId;
    }

    public override void Use(Character character)
    {
        Debug.Log($"{Name} (KeyId: {KeyId}) used by {character.playerName}.");

        if (character.TryUseKey(KeyId)){
            Consume(character);
        }
    }

    public override InventoryItem Clone()
    {
        return new Key(Id, Name, Description, Value, KeyId)
        {
            Amount = this.Amount,
            StackAmount = this.StackAmount,
        };
    }

    public void Consume(Character character)
    {
        Amount--;
        if (Amount <= 0)
        {
            Debug.Log($"Removing key {KeyId} from inventory");
            RemoveFromInventory(character);
        }
    }
}