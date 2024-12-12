using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitalCamera: MonoBehaviour
{
    public float rotationSpeed = 0.5f, zoomSpeed = 0.125f;
    public float minHeight = 1.25f, groundLevel = 1f;
    
    public class Movement
    {
        public Vector2 pan;
        public float zoom;
    }

    //  Deleted after every update, so this is the movement for this frame.
    public Movement movement;

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
        if (movement != null)
        {
            var sr = Mathf.Exp(zoomSpeed * movement.zoom);
            height = Mathf.Max((height - groundLevel) * sr + groundLevel, minHeight);

            // Get the deltas that describe how much the mouse cursor got moved between frames
            var delta = (1 - groundLevel / height) * rotationSpeed * movement.pan;
            //  A scale factor.

            // Rotate the camera left and right
            longditude += delta.x * Time.deltaTime;

            // Rotate the camera up and down
            // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
            latitude = Mathf.Clamp(latitude + delta.y * Time.deltaTime, -Mathf.PI / 2, Mathf.PI / 2);
            UpdateTransform();
            movement = null;
        }
    }

    private void UpdateTransform()
    {
        // Calculate the cartesian coordinates for unity
        transform.position = Cartesian();

        // Make the camera look at the target
        transform.LookAt(Vector3.zero, Up());
    }

    private Vector3 Up()
    {
        return new Vector3(-Mathf.Cos(longditude) * Mathf.Sin(latitude), Mathf.Cos(latitude), -Mathf.Sin(longditude) * Mathf.Sin(latitude));
    }
}