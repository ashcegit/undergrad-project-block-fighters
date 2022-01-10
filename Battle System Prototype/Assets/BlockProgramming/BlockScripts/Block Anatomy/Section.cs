using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    private Header header;
    private Body body;

    void Awake(){
        header=transform.Find("Header").GetComponent<Header>();
        if(transform.Find("Body")!=null){
            body=transform.Find("Body").GetComponent<Body>();
        }else{
            body=null;
        }
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

    public void updateSpacePositions(){
        if(header!=null){
            header.updateInputSpacePositions();
        }
        if(body!=null){
            body.updateSpacePositions();
        }
    }

    public Vector2 updateBlockLayouts(){
        Vector2 sizeVector=new Vector2();
        if(header!=null){
            sizeVector+=header.updateBlockLayouts();
        }
        if(body!=null){
            sizeVector+=body.updateBlockLayouts();
            return sizeVector;
        }else{
            return GetComponentInParent<RectTransform>().sizeDelta;
        }
    }

    public void setSpacesActive(bool active){
        if(header!=null){
            header.setInputSpacesActive(active);
        }
        if(body!=null){
            body.setBlockSpacesActive(active);
        }
    }

    public void removeInputSpaces(){
        if(header!=null){
            List<InputFieldHandler> inputFieldHandlers=header.getInputFieldHandlers();
            foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
                inputFieldHandler.removeInputSpace();
            }
        }
    }

    public void initActionInputFieldHandler(){
        header.initActionInputFieldHandler();
    }
}
