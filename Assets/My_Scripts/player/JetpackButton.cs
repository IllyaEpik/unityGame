using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class JetpackButtonController : MonoBehaviour
{
    public Button jumpButton;
    public TMP_Text buttonText;
    public Jetpack jetpack;
    public Hero hero;
    private bool isHolding = false;

    private void Start()
    {
        AddHoldEvents();
        buttonText.text = LanguageManager.Instance.GetText("jump");
    }

    private void Update()
    {
        // Управление джетпаком через удержание кнопки
        if (isHolding && hero.battery > 0)
        {
            jetpack.StartJetpack();
            buttonText.text = "Jetpack";
        }
        else
        {
            jetpack.StopJetpack();
            // buttonText.text = LanguageManager.Instance.GetText("jump");
        }
    }

    private void AddHoldEvents()
    {
        var trigger = jumpButton.gameObject.AddComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => { isHolding = true; });
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => { isHolding = false; });
        trigger.triggers.Add(pointerUp);
    }
}