using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

public class save_death : MonoBehaviour
{
    
    public Hero player;
    public SaveSystem saveSystem; 
    
    public GameObject enemyPrefab; 
    
    
    void Update()
    {
        
    }
    
    public void OnMyButtonClicked_death() 
    {
        
        
        saveSystem.LoadGame(player, enemyPrefab);

            
        
        
    }
}