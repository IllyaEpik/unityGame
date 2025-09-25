using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    
    public void OpenSettings()
    {
        Debug.Log("-_-");
        // код настроек 
    }

    // Метод для кнопки "Exit"
    public void ExitGame()
    {
        Debug.Log("Выход из игры -_-");
        Application.Quit(); 
    }
}
