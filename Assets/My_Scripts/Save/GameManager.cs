using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

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
            saveSystem.SaveGame(player); 
        }

        // L горячая клав —  Load! 
        if (Input.GetKeyDown(KeyCode.L))
        {
            saveSystem.LoadGame(player, enemyPrefab);
            
        
        }
        
     
    }
    public void OnMyButtonClicked_death() 
        {
        saveSystem.LoadGame(player, enemyPrefab);
        }
}