using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class WinScript : MonoBehaviour {

    public string nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<OrdinaryHuman>() != null)
        {
            Invoke("LoadNextScene", 5f);
        }
    }


    void LoadNextScene()
    {
        EditorSceneManager.LoadScene(nextScene);
    }
}
