using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance;

    public AchievementList achievements = new AchievementList(); // список заполняем через инспектор

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // живёт между сценами
    }

    // Разблокировка ачивки по ID
    public void Unlock(string id)
    {
        foreach (var a in achievements.items)
        {
            if (a.id == id)
            {
                a.achieved = true;
                return;
            }
        }
    }
}