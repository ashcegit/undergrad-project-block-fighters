using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
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

    public void gameOver() {
        SceneManager.LoadScene("Main Menu");
    }
}
