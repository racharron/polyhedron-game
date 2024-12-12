using UnityEngine;

public class Tile : MonoBehaviour
{
    public float wealth, production, offense, defense;
    public GameObject[] neighbors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wealth = Mathf.Exp(Random.Range(-2, 2));
        production = Mathf.Exp(Random.Range(-2, 2));
        offense = Mathf.Exp(Random.Range(-2, 2));
        defense = Mathf.Exp(Random.Range(-2, 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
