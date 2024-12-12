using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class ArcBallCamera: MonoBehaviour
{
    public float rotationSpeed = 0.5f, zoomSpeed = 0.125f;
    public float minHeight = 2f, groundLevel = 1f;

    //  Allow
    public bool enable;

    // The target that the camera looks at
    Vector3 target = Vector3.zero;

    public float height = 4f, longditude = 0f, latitude = 0f;

    void Start()
    {
        transform.position = Cartesian();
        transform.LookAt(target);
    }

    Vector3 Cartesian()
    {
        Vector3 ret = new Vector3();

        ret.x = height * Mathf.Cos(latitude) * Mathf.Cos(longditude);
        ret.y = height * Mathf.Sin(latitude);
        ret.z = height * Mathf.Cos(latitude) * Mathf.Sin(longditude);

        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            Mouse mouse = Mouse.current;

            var sr = Mathf.Exp(zoomSpeed * -mouse.scroll.y.value);
            height = Mathf.Max((height - groundLevel) * sr + groundLevel, minHeight);

            if (mouse.press.isPressed)
            {
                // Get the deltas that describe how much the mouse cursor got moved between frames
                var dx = rotationSpeed * mouse.delta.x.value * (1 - groundLevel / height);
                var dy = rotationSpeed * mouse.delta.y.value * (1 - groundLevel / height);
                //  A scale factor.

                // Only update the camera's position if the mouse got moved in either direction
                if ((dx != 0f || dy != 0f) && !mouse.press.wasPressedThisFrame)
                {
                    // Rotate the camera left and right
                    longditude -= dx * Time.deltaTime;

                    // Rotate the camera up and down
                    // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
                    latitude = Mathf.Clamp(latitude - dy * Time.deltaTime, -Mathf.PI / 2, Mathf.PI / 2);
                }
            }

            // Calculate the cartesian coordinates for unity
            transform.position = Cartesian() + target;

            // Make the camera look at the target
            transform.LookAt(target, new Vector3(-Mathf.Cos(longditude) * Mathf.Sin(latitude), Mathf.Cos(latitude), -Mathf.Sin(longditude) * Mathf.Sin(latitude)));
        }
    }
}