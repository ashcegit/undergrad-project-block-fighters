using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlFunction : MonoBehaviour
{
    public abstract string getName();
    public abstract int function(int pointer,ref List<Block> blockStack);
}
