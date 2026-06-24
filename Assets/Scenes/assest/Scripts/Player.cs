using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public HistoryManager historyManager;

    [Header("Audio")]
    public AudioManager audioManager;

    [Header("Game Stats")]
    public int score = 0;

    public int maxMoves = 10;
    public int movesLeft = 10;

    [Header("Movement")]
    public float moveDuration = 0.15f;

    private bool isMoving = false;
    private Vector2Int currentGridPos;

    private void Start()
    {
        currentGridPos =
            gridManager.WorldToGrid(transform.position);

        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateMoves(movesLeft);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MovePlayer(Vector2Int.right);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MovePlayer(Vector2Int.left);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            MovePlayer(Vector2Int.up);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            MovePlayer(Vector2Int.down);

        if (Input.GetKeyDown(KeyCode.Z))
            UndoMove();
    }
#endif

    public void MovePlayer(Vector2Int direction)
    {
        if (isMoving)
            return;

        Vector2Int targetPos =
            currentGridPos + direction;

        // Boundary Check
        if (targetPos.x < 0 ||
            targetPos.x >= gridManager.width ||
            targetPos.y < 0 ||
            targetPos.y >= gridManager.height)
        {
            return;
        }

        // Obstacle Check
        if (IsObstacle(targetPos))
        {
            Debug.Log("Obstacle Hit");
            return;
        }

        // Save Undo State
        historyManager.SaveState(
            new GameState(
                currentGridPos,
                score,
                movesLeft));

        currentGridPos = targetPos;

        Vector3 targetWorldPos =
            gridManager.GetWorldPosition(
                currentGridPos.x,
                currentGridPos.y);

        StartCoroutine(
            SmoothMove(targetWorldPos));

        movesLeft--;

        UIManager.Instance.UpdateMoves(movesLeft);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMove();

        // Lose Condition
        if (movesLeft <= 0)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayLose();

            UIManager.Instance.ShowLose();
        }
    }

    IEnumerator SmoothMove(Vector3 targetPosition)
    {
        isMoving = true;

        Vector3 startPosition =
            transform.position;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    elapsed / moveDuration);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position =
            targetPosition;

        isMoving = false;
    }

    bool IsObstacle(Vector2Int targetPos)
    {
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            Vector2Int obstaclePos =
                gridManager.WorldToGrid(
                    obstacle.transform.position);

            if (obstaclePos == targetPos)
                return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Goal
        if (other.CompareTag("Goal"))
        {
            score += 100;

            UIManager.Instance.UpdateScore(score);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayWin();

            UIManager.Instance.ShowWin(
                score,
                movesLeft);
        }

        // Bomb
        if (other.CompareTag("Bomb"))
        {
            score += 50;

            UIManager.Instance.UpdateScore(score);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayBomb();

            ActivateBomb();

            Destroy(other.gameObject);
        }
    }

    void ActivateBomb()
    {
        GameObject[] obstacles =
            GameObject.FindGameObjectsWithTag("Obstacle");

        if (obstacles.Length == 0)
            return;

        GameObject nearestObstacle = null;

        float shortestDistance =
            Mathf.Infinity;

        foreach (GameObject obstacle in obstacles)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    obstacle.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestObstacle = obstacle;
            }
        }

        if (nearestObstacle != null)
        {
            Destroy(nearestObstacle);
        }
    }

    public void UndoMove()
    {
        GameState state =
            historyManager.Undo();

        if (state == null)
            return;

        currentGridPos =
            state.playerPosition;

        score =
            state.score;

        movesLeft =
            state.moves;

        StopAllCoroutines();

        transform.position =
            gridManager.GetWorldPosition(
                currentGridPos.x,
                currentGridPos.y);

        isMoving = false;

        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateMoves(movesLeft);
    }
}