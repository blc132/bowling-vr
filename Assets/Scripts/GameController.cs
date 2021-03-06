﻿using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameController : MonoBehaviour
{
    public TextMesh scoreText;
    public TextMesh roundText;
    public TextMesh bonusText;

    public Pin[] pins;
    public Player player;
    public GameObject pinPrefab;

    public int numberOfRounds = 10;
    public GameObject walls;
    public WallMaterialSet[] materialSets;

    public AudioClip[] backgroundMusics;

    private AudioSource audio;

    private float gameTime = 6f;

    private float gameTimer;

    private List<Vector3> pinPositions;

    private bool playerThrewBall;
    private bool scoreCalculated;

    private int score;
    private int round = 1;
    private bool secondTurn;

    private bool strike;
    private bool spare;

    private bool lastRoundStrike;
    private bool lastRoundSpare;

    private int firstRoundBasicScore;

    // Start is called before the first frame update
    void Start()
    {
        //Na starcie sceny czyli w sumie aplikacji pobierz wszystkie pozycje kręgli i je zapisz żeby potem na podstawie tych pozycji ustawiać nowe kręgle
        pinPositions = pins.Select(x => x.transform.position).ToList();

        ChangeWallMaterialSet(0);

        audio = gameObject.GetComponent<AudioSource>();
        audio.clip = backgroundMusics[0];
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {score}";
        roundText.text = $"Round: {round}";

        if (!playerThrewBall)
        {
            if (!player.holdingBowlingBall)
            {
                playerThrewBall = true;
                gameTimer = gameTime;
            }
        }
        else
        {
            //"licznik" gameTimer przechowuje ile minęło czasu od rzutu kulą
            gameTimer -= Time.deltaTime;

            if (gameTimer <= 0f && !scoreCalculated)
            {
                CalculatedTotalScore();
            }

            if (gameTimer <= -3f)
            {
                //jak dalej mamy rundy do rozegrania
                if (round < numberOfRounds)
                    ManageSceneAfterThrow();
                //jak już rozegraliśmy wszystkie rundy
                else
                {
                    //jak minęło 10 sekund to zrestartuj całą scenę
                    if (gameTimer <= -10f)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    //jak nie to wypisuj tekst końcowy
                    else
                    {
                        bonusText.text = $"Congrats, You scored {score} points!";
                        scoreText.text = $"Game restarting in {(int)(gameTimer + 10)}";
                        roundText.text = "";
                    }
                }
            }
        }

        if (Input.GetKeyDown("1"))
            ChangeWallMaterialSet(0);

        if (Input.GetKeyDown("2"))
            ChangeWallMaterialSet(1);

        if (Input.GetKeyDown("3"))
            ChangeWallMaterialSet(2);

        if (Input.GetKeyDown("7"))
            ChangeBackgroundMusic(0);

        if (Input.GetKeyDown("8"))
            ChangeBackgroundMusic(1);

        if (Input.GetKeyDown("9"))
            ChangeBackgroundMusic(2);

        if (Input.GetKeyDown("0"))
            ChangeBackgroundMusic(-1);

    }

    private void ChangeWallMaterialSet(int setNumber)
    {
        var wallsMeshRenderer = walls.GetComponent<Renderer>();

        Material[] newMaterials = new Material[]{
                materialSets[setNumber].FrontWallMaterial, // przednia ściana
                materialSets[setNumber].FrontWallMaterial, // tylnia ściana za kręglami na torze
                materialSets[setNumber].FrontWallMaterial, // element ozdobny
                materialSets[setNumber].SideWallMaterial // ściany boczne
            };

        wallsMeshRenderer.materials = newMaterials;
    }

    private void ChangeBackgroundMusic(int setNumber)
    {
        if (setNumber == -1)
            audio.Stop();
        else
        {
            audio.clip = backgroundMusics[setNumber];
            audio.Play();
        }
    }

    private void CalculatedTotalScore()
    {
        //bazowo jest tyle ile zostało zrzuconych kręgli (czyli są nullami bo zostały zdestroyowane)
        int basicTurnScore = pins.Count(x => x == null);

        //jak druga runda to odejmij całkowity wynik z dwóch od wyniku z pierwszej żeby mieć wynik drugiej xD
        if (secondTurn)
            basicTurnScore -= firstRoundBasicScore;

        int extraTurnScore = basicTurnScore;

        //jak w ostatniej rundzie był strike to pomnóż wynik przez 2
        if (lastRoundStrike)
            extraTurnScore = basicTurnScore * 2;

        //jak w ostatniej rundzie był spare i teraz jest pierwsza tura to pomnóż wynik przez 2
        if (lastRoundSpare && !secondTurn)
            extraTurnScore = basicTurnScore * 2;

        Debug.Log("Pins: " + pins.Length);
        Debug.Log("Basic turn Score: " + basicTurnScore);
        Debug.Log("Extra turn Score: " + extraTurnScore);

        if (basicTurnScore == pins.Length && !secondTurn)
        {
            strike = true;
            bonusText.text = "Strike!";
        }
        else
            bonusText.text = "";

        if (secondTurn && basicTurnScore + firstRoundBasicScore == pins.Length)
        {
            spare = true;
            bonusText.text = "Spare!";
        }
        else
            bonusText.text = "";

        if (!secondTurn)
            firstRoundBasicScore = basicTurnScore;

        score += extraTurnScore;
        scoreCalculated = true;
    }

    private void ManageSceneAfterThrow()
    {
        //Jak druga tura albo strike to zresetuj turę i ustaw kręgle
        if (secondTurn || strike)
        {
            round++;
            secondTurn = false;
            SetNewSetOfPins();
            ResetTurn();
        }
        //Jak pierwsza tura to zresetuj turę
        else
        {
            secondTurn = true;
            ResetTurn();
        }
    }

    private void ResetTurn()
    {
        //Ustaw flagi związane z bonusami z aktualnej rundy na poprzednią rundę
        lastRoundSpare = spare;
        lastRoundStrike = strike;

        //Zresetuj flagi związane z aktualną rundą
        spare = false;
        strike = false;

        //Daj kule do ręki
        player.GetBowlingBallToHand();

        //Zresetuj flagi i timery
        playerThrewBall = false;
        scoreCalculated = false;
        gameTime = 10f;
        gameTimer = 0f;
    }

    private void SetNewSetOfPins()
    {
        //ustaw flagę remove na true żeby obiekt się usunął
        foreach (var pin in pins)
        {
            if (pin != null)
            {
                pin.remove = true;
            }
        }

        //utwórz nowy zestaw kręgli ustawiając je na na pozycji takiej jakie były wcześniejsze
        for (var i = 0; i < pinPositions.Count; i++)
        {
            var pinPosition = pinPositions[i];

            GameObject pinObject = Instantiate(pinPrefab);
            Pin pin = pinObject.GetComponent<Pin>();
            pin.transform.position = pinPosition;
            pins[i] = pin;
        }
    }
}
