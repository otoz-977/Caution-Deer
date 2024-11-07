using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    public GameObject gameoverScreen;
    public GameObject wonScreen;
    public GameObject pauseScreen;
    void Update()
    {
    }
    public void gameover(bool rt){
        pauseScreen.SetActive(true);       
        wonScreen.SetActive(rt);
        gameoverScreen.SetActive(!rt);
        
    }
    public void restartGame(){
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
