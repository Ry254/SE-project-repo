using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    virtual protected void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            EndGame();
        }
        if(Input.GetKeyDown(KeyCode.F5)){
            ResetGame();
        }
    }

    public void ResetGame(){
        SceneManager.LoadScene(0);
    }

    public void EndGame(){
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
