using UnityEngine;
using System.Collections.Generic;

public class HistoryManager : MonoBehaviour
{
    private Stack<GameState> history =
        new Stack<GameState>();

    public void SaveState(GameState state)
    {
        history.Push(state);
    }

    public GameState Undo()
    {
        if (history.Count == 0)
            return null;

        return history.Pop();
    }
}