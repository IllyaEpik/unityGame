using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class LanguageData
{
    public string language;
    public List<Translation> translations;
}

[System.Serializable]
public class Translation
{
    public string key;
    public string value;
}

[System.Serializable]
public class LanguagesCollection
{
    public List<LanguageData> languages;
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public string currentLanguage = "English";
    private Dictionary<string, string> translations = new Dictionary<string, string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLanguage = PlayerPrefs.GetString("Language", "English");
            LoadLanguage(currentLanguage);
            SceneManager.sceneUnloaded += changeLang;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(string language)
    {
        currentLanguage = language;
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.Save();
        LoadLanguage(language);
    }
    private void changeLang(Scene scene)
    {
        GameObject attackButton = GameObject.FindWithTag("attackButton");
            Debug.Log("yes");
        if (attackButton != null)
        {   
            TextMeshProUGUI textOfAttack =  attackButton.GetComponent<TextMeshProUGUI>();
            Debug.Log("what");
            if (textOfAttack != null) textOfAttack.text = "атака";
        }
    }
    public void LoadLanguage(string language)
    {
        translations.Clear();

        TextAsset json = Resources.Load<TextAsset>("languages");
        if (json == null)
        {
            Debug.LogError("Файл languages.json не знайден в папці Resources!");
            return;
        }

        LanguagesCollection allLanguages = JsonUtility.FromJson<LanguagesCollection>(json.text);
        
        foreach (var lang in allLanguages.languages)
        {
            if (lang.language == language)
            {
                foreach (var t in lang.translations)
                    translations[t.key] = t.value;
                return;
            }
        }
    }

    public string GetText(string key)
    {
        if (translations.ContainsKey(key))
            return translations[key];
        return key;
    }
}