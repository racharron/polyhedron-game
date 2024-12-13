using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(OrbitalCamera))]
[RequireComponent(typeof(TileSelector))]
public class Controls : MonoBehaviour
{
    //  Indicates that the mouse hasn't moved while the select mouse button was held down.
    bool canSelect = false;

    public static UnityEvent nextTurn = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard.escapeKey.IsPressed())
        {
            Application.Quit();
            return;
        }
        Mouse mouse = Mouse.current;

        var orbitalCamera = GetComponent<OrbitalCamera>();
        orbitalCamera.scroll = -mouse.scroll.y.value;
        if (mouse.press.isPressed) orbitalCamera.pan = -mouse.delta.value;
        if (mouse.press.wasPressedThisFrame)
        {
            canSelect = true;
        }
        canSelect &= orbitalCamera.scroll == 0 && orbitalCamera.pan == Vector2.zero;
        if (canSelect && mouse.press.wasReleasedThisFrame)
        {
            Ray fromMouse = Camera.main.ScreenPointToRay(mouse.position.value);
            if (
                canSelect
                && Physics.Raycast(fromMouse, out RaycastHit hit)
                && hit.collider.gameObject.TryGetComponent(out Tile selected)
            )
            {
                gameObject.GetComponent<TileSelector>().Select(selected);
            }
            else
            {
                gameObject.GetComponent<TileSelector>().Deselect();
            }
        }
        if (keyboard.spaceKey.wasPressedThisFrame) nextTurn.Invoke();
    }
}
