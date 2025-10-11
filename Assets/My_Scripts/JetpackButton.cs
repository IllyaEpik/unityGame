using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class JetpackButtonController : MonoBehaviour
{
    public Button jumpButton;
    public TMP_Text buttonText;
    public Jetpack jetpack;

    private bool isHolding = false;
    private bool jetpackMode = false;

    private void Start()
    {
        jumpButton.onClick.AddListener(OnButtonClick);
        AddHoldEvents();
    }

    private void Update()
    {
        if (isHolding && jetpackMode)
        {
            jetpack.UseJetpackFromUI();
        }
    }

    private void OnButtonClick()
    {
        jetpackMode = true;
        buttonText.text = "Jetpack";
    }

    private void AddHoldEvents()
    {
        var trigger = jumpButton.gameObject.AddComponent<EventTrigger>();

        var pointerDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDown.callback.AddListener((data) => { isHolding = true; });
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUp.callback.AddListener((data) => { isHolding = false; });
        trigger.triggers.Add(pointerUp);
    }
}