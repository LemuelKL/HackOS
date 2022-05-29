// Close menu when clicked outside

// Should be attached to the menu directly.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class ToggleableMenuController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool pointerInside = false;
    public void OnPointerEnter(PointerEventData e) {
        pointerInside = true;
    }
    public void OnPointerExit(PointerEventData e){
        pointerInside = false;
    }
    void Update() {
        if (Mouse.current.leftButton.wasReleasedThisFrame) {
            if (!pointerInside) {
                this.gameObject.SetActive(false);
            }
        }
    }
}
