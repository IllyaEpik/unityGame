using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsMenu : MonoBehaviour
{
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
        SceneManager.LoadScene("test");
    }
}
