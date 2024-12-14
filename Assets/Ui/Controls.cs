using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(OrbitalCamera))]
[RequireComponent(typeof(TileSelector))]
public class Controls : MonoBehaviour
{
    //  Indicates that the mouse hasn't moved while the select mouse button was held down.
    bool canSelect = false;
    float prevLong, prevLat;
    bool prevOnSphere = false;

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
        
        var mouse = Mouse.current;
        var orbitalCamera = GetComponent<OrbitalCamera>();

        if (mouse.scroll.y.value != 0)
        {
            orbitalCamera.Zoom(-mouse.scroll.y.value);
            canSelect = false;
        }
        if (mouse.press.IsPressed())
        {
            var camera = GetComponent<Camera>();
            var ray = camera.ScreenPointToRay(mouse.position.value);
            var currentOnSphere = Raycast(ray, transform.parent.position, orbitalCamera.groundLevel, out var currLong, out var currLat);
            if (currentOnSphere && prevOnSphere && (prevLong != currLong || prevLat != currLat)) {
                orbitalCamera.Pan(currLong - prevLong, currLat - prevLat);
            }
            canSelect = false;
            prevLong = currLong;
            prevLat = currLat;
            prevOnSphere = currentOnSphere;
        }
        else
        {
            prevOnSphere = false;
        }
        if (mouse.press.wasPressedThisFrame)
        {
            canSelect = true;
        }
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
            canSelect = false;
        }
    }
    private static bool Raycast(Ray ray, Vector3 sphereCenter, float radius, out float longditude, out float latitude)
    {
        var p = ray.origin - sphereCenter;
        var dp = Vector3.Dot(ray.direction, p);
        var det = dp*dp - p.sqrMagnitude + radius;
        if (det < 0)
        {
            longditude = 0;
            latitude = 0;
            return false;
        }
        else
        {
            var intersection = ray.GetPoint(-dp - Mathf.Sqrt(det));
            longditude = Mathf.Atan2(intersection.y, intersection.x);
            latitude = Mathf.Asin(intersection.z / radius);
            return true;
        }
    }
}
