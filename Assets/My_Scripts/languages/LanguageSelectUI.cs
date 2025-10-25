using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageSelectUI : MonoBehaviour
{
    public void SetUkrainian()
    {
        LanguageManager.Instance.SetLanguage("Українська");
        SceneManager.LoadScene("main");
    }

    public void SetEnglish()
    {
        LanguageManager.Instance.SetLanguage("English");
        SceneManager.LoadScene("main");
    }
}