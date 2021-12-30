using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Class <c>Character</c> is the base class for player and opponent logic </summary>
public class Character
{   
    private string characterName;
    private float health;
    private float baseMaxHealth;
    private float baseAttack;
    private float baseDefence;
    private float baseSpeed;
    private int baseStamina;

    private List<AttributeModifier> healthModifiers;
    private List<AttributeModifier> attackModifiers;
    private List<AttributeModifier> defenceModifiers;
    private List<AttributeModifier> speedModifiers;
    private List<AttributeModifier> staminaModifiers;
    
    public Character(string characterName,
                     float baseMaxHealth,
                     float baseAttack,
                     float baseDefence,
                     float baseSpeed,
                     int baseStamina){
        this.characterName=characterName;
        this.health=baseMaxHealth;
        this.baseMaxHealth=baseMaxHealth;
        this.baseAttack=baseAttack;
        this.baseDefence=baseDefence;
        this.baseSpeed=baseSpeed;
        this.baseStamina=baseStamina;

        this.healthModifiers=new List<AttributeModifier>();
        this.attackModifiers=new List<AttributeModifier>();
        this.defenceModifiers=new List<AttributeModifier>();
        this.speedModifiers=new List<AttributeModifier>();
        this.staminaModifiers=new List<AttributeModifier>();
    }

    public void setCharacterName(string characterName){this.characterName=characterName;}
    public void setHealth(float health){this.health=health;}
    public void setBaseMaxHealth(float baseMaxHealth){this.baseMaxHealth=baseMaxHealth;}
    public void setBaseAttack(float baseAttack){this.baseAttack=baseAttack;}
    public void setBaseDefence(float baseDefence){this.baseDefence=baseDefence;}
    public void setBaseSpeed(float baseSpeed){this.baseSpeed=baseSpeed;}
    public void setBaseStamina(int baseStamina){this.baseStamina=baseStamina;}


    public string getCharacterName(){return characterName;}
    public float getBaseMaxHealth(){return baseMaxHealth;}
    public float getBaseAttack(){return baseAttack;}
    public float getBaseDefence(){return baseDefence;}
    public float getBaseSpeed(){return baseSpeed;}
    public int getBaseStamina(){return baseStamina;}
    public List<AttributeModifier> getHealthModifiers(){return healthModifiers;}
    public List<AttributeModifier> getAttackModifiers(){return attackModifiers;}
    public List<AttributeModifier> getDefenceModifiers(){return defenceModifiers;}
    public List<AttributeModifier> getSpeedModifiers(){return speedModifiers;}
    public List<AttributeModifier> getStaminaModifiers(){return staminaModifiers;}

    public float getHealth(){
        if(health<0f){return 0f;}
        else{return health;}
    }
    public float getMaxHealth(){
        float tempMaxHealth=baseMaxHealth;
            foreach(AttributeModifier attributeModifier in healthModifiers){
                tempMaxHealth*=attributeModifier.getMultiplier();
            }
        
        return tempMaxHealth;
    }
    public float getAttack(){
        float tempAttack=baseAttack;
        foreach(AttributeModifier attributeModifier in attackModifiers){
            tempAttack*=attributeModifier.getMultiplier();
        }
        return tempAttack;
    }
    public float getDefence(){
        float tempDefence=baseDefence;
        foreach(AttributeModifier attributeModifier in defenceModifiers){
            tempDefence*=attributeModifier.getMultiplier();
        }
        if(tempDefence>100f){return 100f;}
        return tempDefence;
    }
    public float getSpeed(){
        float tempSpeed=baseSpeed;
            foreach(AttributeModifier attributeModifier in speedModifiers){
                tempSpeed*=attributeModifier.getMultiplier();
            }
        
        return tempSpeed;
    }
    public int getStamina(){
        float tempStamina=baseStamina;
            foreach(AttributeModifier attributeModifier in staminaModifiers){
                tempStamina*=attributeModifier.getMultiplier();
            }
        
        return (int)tempStamina;
    }

    public void addModifier(AttributeModifier attributeModifier){
        switch(attributeModifier.getAttribute()){
            case AttributeEnum.Max_Health:
                health*=attributeModifier.getMultiplier();
                healthModifiers.Add(attributeModifier);
                break;
            case AttributeEnum.Attack:
                attackModifiers.Add(attributeModifier);
                break;
            case AttributeEnum.Defence:
                defenceModifiers.Add(attributeModifier);
                break;
            case AttributeEnum.Speed:
                speedModifiers.Add(attributeModifier);
                break;
            case AttributeEnum.Stamina:
                staminaModifiers.Add(attributeModifier);
                break;
            default:
                break;
        }
    }

    public void endTurn(){
        for(int i=0;i<healthModifiers.Count;i++){
            healthModifiers[i].turnHasPassed();
            if(healthModifiers[i].isFinished()){
                //Gives back health lost in debuff
                if(healthModifiers[i].getMultiplier()<1){health*=(1-healthModifiers[i].getMultiplier());}
                else{health/=healthModifiers[i].getMultiplier();}
                healthModifiers.RemoveAt(i);
            }
        }
        for(int i=0;i<attackModifiers.Count;i++){
            attackModifiers[i].turnHasPassed();
            if(attackModifiers[i].isFinished()){attackModifiers.RemoveAt(i);}
        }
        for(int i=0;i<defenceModifiers.Count;i++){
            defenceModifiers[i].turnHasPassed();
            if(defenceModifiers[i].isFinished()){defenceModifiers.RemoveAt(i);}
        }
        for(int i=0;i<speedModifiers.Count;i++){
            speedModifiers[i].turnHasPassed();
            if(speedModifiers[i].isFinished()){speedModifiers.RemoveAt(i);}
        }
        for(int i=0;i<staminaModifiers.Count;i++){
            staminaModifiers[i].turnHasPassed();
            if(staminaModifiers[i].isFinished()){staminaModifiers.RemoveAt(i);}
        }
    }

    public void decreaseHealth(float damage){
        if(health-damage<0f){health=0;}
        else{health-=damage;}
    }
    public void increaseHealth(float increase){
        if(health+increase>getMaxHealth()){health=getMaxHealth();}
        else{health+=increase;}
    }
    public void fullHeal(){health=getMaxHealth();}

}
