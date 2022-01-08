using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlFunction : MonoBehaviour
{
    public abstract int function(Block block,int pointer,List<Block> blockStack);
}
