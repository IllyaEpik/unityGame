using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSlotsUI : MonoBehaviour
{
    [Header("Слоты")]
    [SerializeField] private Button[] slotButtons;      // Кнопки слотов
    [SerializeField] private TMP_Text[] slotLabels;     // Тексты слотов

    [Header("Управление")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button backButton;

    [Header("Цвета")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.green;

    private int selectedSlot = -1;

    private void Start()
    {
        // Слоты
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;
            slotButtons[i].onClick.AddListener(() => SelectSlot(index));
        }

        // Кнопки управления
        saveButton.onClick.AddListener(() => SaveSlot());
        loadButton.onClick.AddListener(() => LoadSlot());
        deleteButton.onClick.AddListener(() => DeleteSlot());
        backButton.onClick.AddListener(() => GoBack());

        // Сначала кнопки Save/Load/Delete скрыты
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);

        UpdateSlotLabels();
    }

    private void SelectSlot(int slotIndex)
    {
        selectedSlot = slotIndex;

        // Меняем цвет выбранного слота
        for (int i = 0; i < slotButtons.Length; i++)
        {
            ColorBlock cb = slotButtons[i].colors;
            cb.normalColor = (i == selectedSlot) ? selectedColor : normalColor;
            slotButtons[i].colors = cb;
        }

        // Показываем кнопки управления при выборе слота
        saveButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);
        deleteButton.gameObject.SetActive(true);
    }

    public void SaveSlot()
    {
        if (selectedSlot >= 0)
        {
            SaveSystem.Instance.Save(selectedSlot);
            UpdateSlotLabels();
        }
    }

    private void LoadSlot()
    {
        if (selectedSlot >= 0)
        {
            SaveSystem.Instance.Load(selectedSlot);
        }
    }

    private void DeleteSlot()
    {
        if (selectedSlot >= 0)
        {
            SaveSystem.Instance.Delete(selectedSlot);
            UpdateSlotLabels();
        }
    }

    private void GoBack()
    {
        SceneManager.LoadScene("test");
    }

    private void UpdateSlotLabels()
    {
        for (int i = 0; i < slotLabels.Length; i++)
        {
            if (SaveSystem.Instance.IsSlotOccupied(i))
                slotLabels[i].text = "Occupied";
            else
                slotLabels[i].text = "Empty";
        }
    }
}