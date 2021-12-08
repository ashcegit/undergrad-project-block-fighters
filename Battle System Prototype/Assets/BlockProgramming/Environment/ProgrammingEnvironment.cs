using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgrammingEnvironment : MonoBehaviour
{

    List<GameObject> blockStacks;

    // Start is called before the first frame update
    void Start()
    {   
        //setup clear environment listener on red clearing button
        GameObject.FindGameObjectWithTag("ClearButton")
                    .GetComponent<Button>()
                    .onClick.AddListener(ClearEnvironment);
    }

    void ClearEnvironment(){

    }

}
