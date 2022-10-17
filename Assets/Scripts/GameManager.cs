using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameOver;
    public float resolveTime;

    [Header("Timer Settings")]
    [SerializeField] private float setTimer = 180f;
    [SerializeField] private Text timerText;
    private float timeRemaining;
    public bool isTimerRunning;

    [Header("Game Refs")]
    public int solvedAmount;
    List<int> randomIndexes = new List<int>();
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    public RectTransform emptyTile = null;
    [SerializeField] private float minimalDistance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject newScore;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private List<GameObject> feedbacks = new List<GameObject>();

    private void Awake()
    {
        instance = this;

        if (PersistenceData.instance != null)
        {
            bestScoreText.text = "Best Score : " + PersistenceData.instance.bestTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = setTimer;
        isTimerRunning = true;
        GenerateRandomPosition();
    }

    #region System & Features

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }

    /// <summary>
    /// This method generate a random position for each tiles in the tiles List
    /// </summary>
    public void GenerateRandomPosition()
    {
        emptyTile.transform.SetAsLastSibling();

        randomIndexes = new List<int>(new int[9]);

        for (int i = 0; i < tiles.Count; i++)
        {
            //Generate a random integer
            int randomIndex = Random.Range(0, (tiles.Count+1));
                while (randomIndexes.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, (tiles.Count + 1));
                }
            randomIndexes[i] = randomIndex;

            //Change the position of the tile by the random index
            tiles[i].transform.SetSiblingIndex(randomIndexes[i]-1);
            feedbacks[i].transform.SetSiblingIndex(randomIndexes[i] - 1);

            tiles[i].GetComponent<Tile>().currentiD = randomIndexes[i] - 1;
        }
    }

    public void SwapTile(RectTransform _transfrom)
    {
        //Save the last index of the empty tile
        int lastEmptyTileIndex = emptyTile.GetSiblingIndex();
        int lastTileIndex = _transfrom.GetSiblingIndex();

        emptyTile.SetSiblingIndex(_transfrom.GetSiblingIndex());

        _transfrom.SetSiblingIndex(lastEmptyTileIndex);

        emptyTile.GetComponent<Tile>().currentiD = lastTileIndex;

        //Reproduced the current image to the bottom
        DisplayImage();
    }

    public bool isSwappable(RectTransform _transfrom)
    {
        if (Vector2.Distance(emptyTile.anchoredPosition, _transfrom.anchoredPosition) <= minimalDistance * 100)
        {
            //Debug.Log(Vector2.Distance(emptyTile.anchoredPosition, _transfrom.anchoredPosition));
            return true;
        }
        return false;
    }

    void CountDown()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

            }
            else
            {
                Debug.Log("Time has run out!");
                isTimerRunning = false;
                timeRemaining = 0;
                resolveTime = setTimer - timeRemaining;
                GameOver();
            }
        }

        DisplayTime(timeRemaining);
    }

    public void IsResolve(int i)
    {
        solvedAmount += i;

        if (solvedAmount == tiles.Count)
        {
            Debug.Log("Solved");
            GameOver();
        }
    }

    public void GameOver()
    {
        isTimerRunning = false;
        
        isGameOver = true;
        gameOverPanel.SetActive(true);

        UpdateScore();
    }

    public void UpdateScore()
    {
        if (resolveTime < PersistenceData.instance.bestTime)
        {
            newScore.SetActive(true);

            PersistenceData.instance.bestTime = resolveTime;

            bestScoreText.text = "Best Score : " + PersistenceData.instance.bestTime;
        }
    }

    public void Save()
    {
        PersistenceData.instance.Save();
    }

    #endregion

    #region UI
    /// <summary>
    /// Display time in the proper way (00:00)
    /// </summary>
    /// <param name="timeToDisplay"></param>
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Set each tile in the bottom grid (feedback) image to the current grid
    /// </summary>
    void DisplayImage()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            feedbacks[i].transform.SetSiblingIndex(tiles[i].transform.GetSiblingIndex());
        }
    }
    #endregion
}
