using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Space
{
    Vector2 getPosition();
    void setPosition(Vector2 position);
    int getIndex();
    void setIndex(int index);
    bool getActive();
    void setActive(bool active);
}
