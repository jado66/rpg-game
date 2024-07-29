using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Player player;

    Vector3 hitPoint;

    // Colors [] messageColors = new Colors[]{Colors.blue,Colors.red,Colors.yellow,Colors.gray,Colors.green,Colors.magenta,new Color(102/255f, 0, 1,1)};


    void Start(){
        player = GameObject.Find("Character").GetComponent<Player>();
        health = maxHealth;    
    }
    /*
    void OnAttack(Vector3 hitPoint){ // Should be a general player method
        this.hitPoint = hitPoint;
        float length = player.attackWeapon.stats["length"];
        
        float distance = Vector3.distance(hitPoint,player.transform);
        float distance = distance/length*10;

        float sweetSpot = player.attackWeapon.stats["sweetSpot"];
        float sweetSpot = sweetSpot/length*10;

        float baseDamage = player.attackWeapon.stats["baseDamage"];        

        bool movingInSameDirection = true;
        bool movingInOppositeDirection = false;

        int standardDamage = (int)(1.5f*Mathf.exp(-.5( Mathf.pow(((distance-sweetSpot)-u)/o),2)));
        int totalDamage;
        int difference = standardDamage - (int)(baseDamage);

        string damageNotification = "";
        if (player.moving ){
            if (!player.running){
                if (movingInSameDirection){
                    totalDamage = standardDamage*1.15f;
                    damageNotification+="Lunging ";
                }
                else if (movingInOppositeDirection){
                    totalDamage = standardDamage*.985f;
                    damageNotification+="Dodging ";
                }
            }
            else{
                if (movingInSameDirection){
                    totalDamage = standardDamage*1.3f;
                    damageNotification+="Charging ";}
                else if (movingInOppositeDirection){
                    totalDamage = standardDamage*.7f;
                    damageNotification+="Retreating ";}
            }
        }

        
        if (difference >= .9f * baseDamage){
            damageNotification+="perfect ";
        }
        else if (difference >= .75f * baseDamage){
            damageNotification+="amazing ";
        }
        else if (difference >= .5f * baseDamage){
            damageNotification+="good ";
        }
        else if (difference >= baseDamage){
            damageNotification+="average ";
        }
        else if (difference >= -.75 * baseDamage){
            damageNotification+="poor ";
        }
        else if (difference >= -.5 * baseDamage){
            damageNotification+="terrible ";
        }
        else{
            damageNotification+="pathetic ";
        }

        damageNotification += "+"+totalDamage.ToString();
    
        StartCoroutine();
    
    }

    private IEnumerator displayHitMessage(string message,float power){
        float baseTime = .5f;
        // from hit point display message opposite 

        // hitMessage.setActive(true);
        // hitMessage.Text = message
        // hitMessage.fontColor = colors[Random.Range(0,colors.length)]


        // ... so if hit is here ->|| ->"display message 
        yield return new WaitForSeconds(baseTime);

        

        if (power>0){ //so longer more powerful hits stay up longer this will be fun
            yield return new WaitForSeconds(power);
        }
        
        yield return null;
    }
    
    */
}
