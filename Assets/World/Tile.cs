using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour
{
    public float wealth, production, offense, defense;
    public GameObject[] neighbors;

    public Color baseColor {  get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wealth = Mathf.Exp(Random.Range(-2, 2));
        production = Mathf.Exp(Random.Range(-2, 2));
        offense = Mathf.Exp(Random.Range(-2, 2));
        defense = Mathf.Exp(Random.Range(-2, 2));
        baseColor = Random.ColorHSV(0, 1, 0.5f, 0.5f, 0.5f, 0.5f);
        gameObject.GetComponent<MeshRenderer>().material.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
