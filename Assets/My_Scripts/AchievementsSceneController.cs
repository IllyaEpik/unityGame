using UnityEngine;

public class AchievementsSceneController : MonoBehaviour
{
    public Transform container;           // ScrollView Content
    public GameObject achievementPrefab;  // Prefab одной ачивки

    private void Start()
    {
        GenerateUI();
    }

    private void GenerateUI()
    {
        // Удаляем старые элементы
        foreach (Transform child in container)
            Destroy(child.gameObject);

        // Создаём новые по списку
        foreach (var a in AchievementsManager.Instance.achievements.items)
        {
            GameObject obj = Instantiate(achievementPrefab, container);
            obj.GetComponent<AchievementEntryUI>().Setup(a);
        }
    }
}