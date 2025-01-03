using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public float cycleSpeed = 8;
    public float variationMagnitude = 0.125f;
    Tile selected = null;
    float phase = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Select(Tile tile)
    {
        selected = tile;
        phase = 0;
    }
    public void Deselect()
    {
        selected = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null)
        {
            Color.RGBToHSV(selected.baseColor, out float hue, out float saturation, out float value);
            float variance = Mathf.Sin(phase);
            Color selectedColor = Color.HSVToRGB(hue, saturation + variationMagnitude * variance, value + variationMagnitude * variance);
            selected.gameObject.GetComponent<MeshRenderer>().material.color = selectedColor;
            phase += cycleSpeed * Time.deltaTime;
            if (phase > 2 * Mathf.PI) phase -= 2 * Mathf.PI;
        }
    }
}
