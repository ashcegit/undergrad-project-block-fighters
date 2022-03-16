using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        enabled = true;
    }

    void OnDisable() {
        enabled = false;
    }

    public void quit() {
        SceneManager.LoadScene("Main Menu");
    }
}
