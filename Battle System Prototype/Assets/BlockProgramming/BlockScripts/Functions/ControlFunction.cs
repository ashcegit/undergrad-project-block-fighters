using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlFunction : MonoBehaviour
{
    public abstract int function(int pointer,List<Block> blockStack);
}
