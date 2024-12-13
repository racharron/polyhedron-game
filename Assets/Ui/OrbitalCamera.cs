using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitalCamera : MonoBehaviour
{
    public float rotationSpeed = 0.07f, zoomSpeed = 0.125f;
    public float minHeight = 1.25f, groundLevel = 1f;

    public float scroll;
    public Vector2 pan;

    public float height = 4f, longditude = 0f, latitude = 0f;

    void Start()
    {
        UpdateTransform();
    }

    Vector3 Cartesian()
    {
        return new(
            height * Mathf.Cos(latitude) * Mathf.Cos(longditude),
            height * Mathf.Sin(latitude),
            height * Mathf.Cos(latitude) * Mathf.Sin(longditude)
        );
    }

    // Update is called once per frame
    void Update()
    {
        bool update = false;
        if (scroll != 0)
        {
            var sr = Mathf.Exp(zoomSpeed * scroll);
            height = Mathf.Max((height - groundLevel) * sr + groundLevel, minHeight);
            scroll = 0;
            update = true;
        }
        if (pan != Vector2.zero)
        {
            // Get the deltas that describe how much the mouse cursor got moved between frames 
            var delta = (rotationSpeed * pan)*(height - groundLevel);

            // Rotate the camera left and right
            longditude += delta.x * Time.deltaTime;

            // Rotate the camera up and down
            // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
            latitude = Mathf.Clamp(latitude + delta.y * Time.deltaTime, -Mathf.PI / 2, Mathf.PI / 2);
            pan = Vector2.zero;
            update = true;
        }
        if (update) UpdateTransform();
    }

    private void UpdateTransform()
    {
        var cartesian = Cartesian();
        transform.SetLocalPositionAndRotation(cartesian, Quaternion.LookRotation(-cartesian, Up()));
    }

    private Vector3 Up()
    {
        return new Vector3(-Mathf.Cos(longditude) * Mathf.Sin(latitude), Mathf.Cos(latitude), -Mathf.Sin(longditude) * Mathf.Sin(latitude));
    }
}
