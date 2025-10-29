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
    public bool isHolding = false;
    private bool jetpackMode = false;

    private void Start()
    {
        jumpButton.onClick.AddListener(OnButtonClick);
        AddHoldEvents();
        buttonText.text = LanguageManager.Instance.GetText("jump");
    }

    private void Update()
    {
        jetpackMode = !hero.isGround;
        jetpack.isUsingJetpack = isHolding && jetpackMode;

        if (!jetpackMode)
        {
            Debug.Log(buttonText);
            buttonText.text = LanguageManager.Instance.GetText("jump");
            jetpack.animator.SetBool("isFlying", false);
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

        var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => { isHolding = true; });
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => { isHolding = false; });
        trigger.triggers.Add(pointerUp);
    }
}