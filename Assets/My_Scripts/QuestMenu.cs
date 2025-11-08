using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsMenu : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
    }
    private void Update()
    {
        // скролл стрелками
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector2.up * Time.unscaledDeltaTime * 200);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector2.down * Time.unscaledDeltaTime * 200);
    }
    

    public void BackToGame()
    {
        // load();
        SceneManager.LoadScene("test");
    }
    
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "test")
    //     {
    //         load();
    //         // unsubscribe if you only need this once
    //         SceneManager.sceneLoaded -= OnSceneLoaded;
    //     }
    // }
}
