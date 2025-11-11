using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public SaveSystem saveSystem;
    public Hero player;
    public Light2D globalLight; // твой главный свет (2D Light)
    public AudioSource gameAudio; // основной источник звука

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
                CloseSettings();
            else
                TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenAchievements()
    {
        Time.timeScale = 1f;
        SaveGame();
        SceneManager.LoadScene("Achivements");
    }

    public void OpenQuests()
    {
        Time.timeScale = 1f;
        SaveGame();
        SceneManager.LoadScene("Quests"); // когда создашь сцену с заданиями
    }

    public void SaveGame()
    {
        saveSystem.SaveGame(player);
    }

    public void OpenSettings()
    {
        SaveGame();
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void SaveAndQuit()
    {
        saveSystem.SaveGame(player);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Методы для ползунков
    public void SetBrightness(float value)
    {
        if (globalLight)
            globalLight.intensity = value;
    }

    public void SetVolume(float value)
    {
        if (gameAudio)
            gameAudio.volume = value;
    }
}