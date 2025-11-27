using UnityEngine;
using TMPro;

public class AchievementEntryUI : MonoBehaviour 
{
    public TMP_Text titleText;
    public TMP_Text descText;

    public void Setup(AchievementData data) {
        titleText.text = data.title;
        descText.text = data.description;

        if (data.achieved)
        {
            titleText.color = Color.green;
            descText.color = Color.green;
        }
        else
        {
            titleText.color = Color.red;
            descText.color = Color.red;
        }
    }
}