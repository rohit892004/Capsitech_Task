using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text moveText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Win Panel")]
    public TMP_Text finalScoreText;
    public TMP_Text finalMoveText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    public void UpdateMoves(int movesLeft)
    {
        moveText.text = "Moves Left : " + movesLeft;
    }

    public void ShowWin(int score, int movesLeft)
    {
        winPanel.SetActive(true);

        finalScoreText.text =
            "Final Score : " + score;

        finalMoveText.text =
            "Moves Left : " + movesLeft;
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
    }
}