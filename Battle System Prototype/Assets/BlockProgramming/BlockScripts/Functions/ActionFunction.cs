using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionFunction:MonoBehaviour
{
    public abstract GameAction function(Character target,Character instigator);
}
