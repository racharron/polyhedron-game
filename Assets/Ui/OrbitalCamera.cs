using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitalCamera : MonoBehaviour
{
    public float zoomSpeed = 0.125f;
    public float minHeight = 0.125f, groundLevel = 1f;

    /// <summary>
    /// The height from the ground of the camera (r = height + ground level).
    /// </summary>
    public float height = 2f;
    /// <summary>
    /// The longditude of the camera.
    /// </summary>
    public float longditude = 0f;
    /// <summary>
    /// The latitude of the camera.
    /// </summary>
    public float latitude = 0f;
    /// <summary>
    /// The polar `r` coordinate from the center of the orbited object.
    /// </summary>
    public float R { get { return height + groundLevel; } }

    void Start()
    {
        UpdateTransform();
    }

    Vector3 Cartesian()
    {
        var r = height + groundLevel;
        return new(
            r * Mathf.Cos(latitude) * Mathf.Cos(longditude),
            r * Mathf.Sin(latitude),
            r * Mathf.Cos(latitude) * Mathf.Sin(longditude)
        );
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    public void Zoom(float scroll)
    {
        var sr = Mathf.Exp(zoomSpeed * scroll);
        height = Mathf.Max(height * sr, minHeight);
        scroll = 0;
    }
    public void Pan(float dLong, float dLat)
    {
        longditude += dLong;
        if (longditude < 0) longditude += 2 * Mathf.PI;
        if (longditude >= 2 * Mathf.PI) longditude -= 2 * Mathf.PI;
        latitude = Mathf.Clamp(latitude + dLat, -Mathf.PI / 2, Mathf.PI / 2);
    }

    private void UpdateTransform()
    {
        Transform parent = gameObject.transform.parent;
        // Calculate the cartesian coordinates for unity
        transform.position = Cartesian() + parent.position;

        // Make the camera look at the target
        transform.LookAt(parent.position, parent.up);
    }
}