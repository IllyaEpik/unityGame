using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public GameObject enemy;
    public Hero hero;

    public bool isActive = false;
    public bool isCompleted = false;
    public bool isFailed = false;

    void Update()
    {
        if (!isActive || isCompleted || isFailed)
            return;

        if (hero != null && hero.health <= 0)
        {
            isFailed = true;
            isActive = false;
            Debug.Log("Місію провалено — герой загинув");
            return;
        }

        if (enemy == null)
        {
            isCompleted = true;
            isActive = false;
            Debug.Log("Місію виконано — ворога знищено");
        }
    }

    public void ActivateMission()
    {
        isActive = true;
        isCompleted = false;
        isFailed = false;
        Debug.Log(" Місія активована: ліквідуй ворога");
    }
}