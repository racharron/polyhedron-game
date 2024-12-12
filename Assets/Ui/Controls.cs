using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(OrbitalCamera))]
[RequireComponent(typeof(TileSelector))]
public class Controls : MonoBehaviour
{
    //  Indicates that the mouse hasn't moved while the select mouse button was held down.
    bool canSelect = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        var zoom = -mouse.scroll.y.value;
        var pan = -mouse.delta.value;
        if (mouse.press.wasPressedThisFrame) canSelect = true;
        canSelect &= zoom == 0 && pan == Vector2.zero;
        if (zoom != 0 || (mouse.press.isPressed && !mouse.press.wasPressedThisFrame && pan != Vector2.zero))
        {
            gameObject.GetComponent<OrbitalCamera>().movement = new() { zoom = zoom, pan = pan };
        }
        if (canSelect && mouse.press.wasReleasedThisFrame)
        {
            Ray fromMouse = Camera.main.ScreenPointToRay(mouse.position.value);
            if (
                canSelect
                && Physics.Raycast(fromMouse, out RaycastHit hit, LayerMask.GetMask("UI"))
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
    }
}
