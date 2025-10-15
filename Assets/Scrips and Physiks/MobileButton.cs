using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum EmulatedKey { None, LeftArrow, RightArrow }

    [HideInInspector] public bool isPressed = false;

    [Tooltip("Which key this mobile button should emulate while held.")]
    public EmulatedKey emulatedKey = EmulatedKey.None;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        Debug.Log($"[MobileButton] OnPointerDown: Emulating {emulatedKey}");
        if (emulatedKey == EmulatedKey.LeftArrow)
            InputEmulator.SetKeyState(KeyCode.LeftArrow, true);
        else if (emulatedKey == EmulatedKey.RightArrow)
            InputEmulator.SetKeyState(KeyCode.RightArrow, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        Debug.Log($"[MobileButton] OnPointerUp: Releasing {emulatedKey}");
        if (emulatedKey == EmulatedKey.LeftArrow)
            InputEmulator.SetKeyState(KeyCode.LeftArrow, false);
        else if (emulatedKey == EmulatedKey.RightArrow)
            InputEmulator.SetKeyState(KeyCode.RightArrow, false);
    }

    void OnDisable()
    {
        // Ensure key state is cleared if the button is disabled while pressed
        if (isPressed)
        {
            isPressed = false;
            if (emulatedKey == EmulatedKey.LeftArrow)
                InputEmulator.SetKeyState(KeyCode.LeftArrow, false);
            else if (emulatedKey == EmulatedKey.RightArrow)
                InputEmulator.SetKeyState(KeyCode.RightArrow, false);
        }
    }
}
