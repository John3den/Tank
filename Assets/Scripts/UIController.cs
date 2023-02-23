using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public static class Score
{
    public static int playerScore = 0;
    public static int enemyScore = 0;
}

public class UIController : MonoBehaviour
{
    public static UIController UIControl;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI enemyScoreText;
    private void Start()
    {
        UIControl = this;
        playerScoreText.text = Score.playerScore.ToString();
        enemyScoreText.text = Score.enemyScore.ToString();
    }
    public void UpdateScore(bool playerWon)
    {
        if(playerWon)
        {
            Score.playerScore++;
            playerScoreText.text = Score.playerScore.ToString();
        }
        else
        {
            Score.enemyScore++;
            enemyScoreText.text = Score.enemyScore.ToString();
        }
        SceneManager.LoadScene("SampleScene");
    }
}
