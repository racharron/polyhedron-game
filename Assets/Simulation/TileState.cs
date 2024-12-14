using UnityEngine;

public class TileState : MonoBehaviour
{
    public State state;

    void Start()
    {
        Controls.active.onNextTurn.AddListener(Tick);
    }

    void Tick()
    {
        state = state.Next();
    }
}
