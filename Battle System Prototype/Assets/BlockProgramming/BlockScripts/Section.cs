using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    Header header;
    Body body;

    void OnAwake(){
        header=transform.Find("Header").GetComponent<Header>();
        body=transform.Find("Body").GetComponent<Body>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Header getHeader(){return header;}
    public Body getBody(){return body;}
}
