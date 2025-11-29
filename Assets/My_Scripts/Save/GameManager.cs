using UnityEngine;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // Главный герой
    public Hero player;
    public SaveSystem saveSystem;
    // Префаб врага — чтобы можно было клонировать  сколько угодно
    public GameObject enemyPrefab;   // префаб врага


    void Update()
    {
        // S горячая клав —  Save! 
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("saving");
            // Просто передаем игрока. SaveSystem сам найдет врагов.
            saveSystem.Save(0); 
        }

        // L горячая клав —  Load! 
        if (Input.GetKeyDown(KeyCode.L))
        {
            saveSystem.Load(0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            saveSystem.Delete(0);
        }
        // resetAll

    }
    public void OnMyButtonClicked_death()
    {
        saveSystem.Load(0);
    }
    // private void load()
    // {

    //     gameManager = FindFirstObjectByType<GameManager>();
    //     gameManager.OnMyButtonClicked_death();
    // }
    private void Start()
    {
        OnMyButtonClicked_death();
    }
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "test")
    //     {
    //         OnMyButtonClicked_death();
    //         // unsubscribe if you only need this once
    //         SceneManager.sceneLoaded -= OnSceneLoaded;
    //     }
    // }
    // private void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // private void OnDisable()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }
}