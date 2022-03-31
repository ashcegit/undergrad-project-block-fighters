using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInteraction:Interaction
{   
    private AttributeModifier attributeModifier;
    private StatusEffect statusEffect;

    public StatusEffectInteraction(StatusEffect statusEffect){
        this.statusEffect=statusEffect;
        if(Random.Range(0.0f,1.0f)<=statusEffect.getChance()){result=InteractionEnum.Hit;}
        else{result=InteractionEnum.Miss;}
    }

    public StatusEffect getStatusEffect(){return statusEffect;}
    public AttributeModifier getAttributeModifier(){
        return new AttributeModifier(statusEffect.getTurns(),
                                     statusEffect.getAttribute(),
                                     statusEffect.getMultiplier());
    }
    public AttributeEnum getAttribute(){return statusEffect.getAttribute();}
    public float getMultiplier(){return statusEffect.getMultiplier();}
    public float getChance(){return statusEffect.getChance();}
    public int getTurns(){return statusEffect.getTurns();}
}
