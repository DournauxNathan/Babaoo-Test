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
    List<int> randomIndexes = new List<int>();
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    public RectTransform emptyTile = null;
    [SerializeField] private float minimalDistance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject newScore;
    [SerializeField] private Text newBestTimeText;
    [SerializeField] private Text bestTimeText;
    [SerializeField] private List<GameObject> feedbacks = new List<GameObject>();

    private void Awake()
    {
        instance = this;

        if (PersistenceData.instance != null)
        {
            float minutes = Mathf.FloorToInt(PersistenceData.instance.bestTime / 60);
            float seconds = Mathf.FloorToInt(PersistenceData.instance.bestTime % 60);
            bestTimeText.text = string.Format("Best time : {0:00}:{1:00}", minutes, seconds);
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

    public void SwapTile(Transform _transfrom)
    {
        //Save the last index of the empty tile
        int lastEmptyTileIndex = emptyTile.GetSiblingIndex();
        //Save the last tile index
        int lastTileIndex = _transfrom.GetSiblingIndex();

        //ID Swap
        emptyTile.SetSiblingIndex(_transfrom.GetSiblingIndex());
        _transfrom.SetSiblingIndex(lastEmptyTileIndex);
        emptyTile.GetComponent<Tile>().currentiD = lastTileIndex;
        
        //Check if the puzzle is solved after a movement
        Solve();

        //Reproduced the current image to the bottom
        DisplayImage();
    }

    public bool isSwappable(Transform _transfrom)
    {
        if (Vector2.Distance(emptyTile.position, _transfrom.position) <= minimalDistance * 100)
        {
            //Debug.Log(Vector2.Distance(emptyTile.position, _transfrom.position) <= minimalDistance * 100);
            
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
                
                GameOver();
            }
        }

        DisplayTime(timeRemaining);
    }

    public void Solve()
    {
        int correctTile = 0;
        Debug.Log(correctTile);

        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                if (tile.GetComponent<Tile>().isInRightPlace)
                    correctTile++;
            }
        }

        if (correctTile == (tiles.Count - 1))
        {
            Debug.Log("Solved");

            resolveTime = setTimer - timeRemaining;

            GameOver();
        }
    }

    public void GameOver()
    {
        isTimerRunning = false;
        
        isGameOver = true;
        gameOverPanel.SetActive(true);

        //Update the best time (Save Persistent data)
        UpdateTime();
    }

    public void UpdateTime()
    {
        if (resolveTime < PersistenceData.instance.bestTime)
        {
            newScore.SetActive(true);

            PersistenceData.instance.bestTime = resolveTime;

            float minutes = Mathf.FloorToInt(PersistenceData.instance.bestTime / 60);
            float seconds = Mathf.FloorToInt(PersistenceData.instance.bestTime % 60);
            newBestTimeText.text = string.Format("Best Score : {0:00}:{1:00}", minutes, seconds);
            
        }
    }

    public void Save()
    {
        //Save the best time when the player click on Restart or Home
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
