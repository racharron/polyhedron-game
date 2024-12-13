using UnityEngine;

[RequireComponent(typeof(OldTileState))]
public class TileState : MonoBehaviour
{
    public State state;

    void Start()
    {
        Controls.nextTurn.AddListener(Tick);
    }

    void Tick()
    {
        state = state.Next();
    }
}
