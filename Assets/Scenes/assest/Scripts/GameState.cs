using UnityEngine;

[System.Serializable]
public class GameState
{
    public Vector2Int playerPosition;
    public int score;
    public int moves;

    public GameState(Vector2Int pos, int score, int moves)
    {
        playerPosition = pos;
        this.score = score;
        this.moves = moves;
    }
}