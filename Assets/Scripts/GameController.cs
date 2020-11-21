using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMesh infoText;
    public Pin[] pins;
    public Player player;

    public float evaluationTime = 10f;
    private float gameTimer = 0f;
    private bool evaluating = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        infoText.text = "Throw the bowling ball!";

        if (!evaluating)
        {
            if (!player.holdingBowlingBall)
            {
                evaluating = true;
                gameTimer = evaluationTime;
            }
        }
        else
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0f)
            {
                int score = pins.Count(x => x == null);
                infoText.text = $"Your score: {score}";
            }

            if (gameTimer <= -3f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
