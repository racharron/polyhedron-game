using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TileDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var state = GetComponent<RectTransform>().parent.parent.GetComponent<TileState>().state;
        GetComponent<TMP_Text>().text
            = "L:\t" + State.DisplayQuantity(state.liquidity)
            + "\nD:\t" + State.DisplayQuantity(state.development)
            + "\nI:\t" + State.DisplayQuantity(state.infrastructure)
            + "\nT:\t" + State.DisplayQuantity(state.technology);
    }
}
