using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Timer Settings")]
    [SerializeField] private float setTimer = 180f;
    [SerializeField] private Text timerText;
    private float timeRemaining;
    public bool isTimerRunning;

    [Header("GameView")]
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    public RectTransform emptyTile = null;
    [SerializeField] private float minimalDistance;

    [Header("GameView")]
    [SerializeField] private List<GameObject> feedbacks = new List<GameObject>();

    private List<Transform> pieces = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = setTimer;
        isTimerRunning = true;
        GenerateRandomPosition();
    }

    /// <summary>
    /// This method generate a random position for each tiles in the tiles List
    /// </summary>
    public void GenerateRandomPosition()
    {
        emptyTile.transform.SetAsLastSibling();

        for (int i = 0; i < tiles.Count; i++)
        {
            //Generate a random integer
            int randomIndex = Random.Range(0, tiles.Count);

            //Change the position of the tile by the random index
            tiles[i].transform.SetSiblingIndex(randomIndex);
            feedbacks[i].transform.SetSiblingIndex(randomIndex);
            pieces.Add(tiles[i].transform);
            //Make the last elements of the tiles List unactive
        }
    }

    // Update is called once per frame
    void Update()
    {
        CountDown(); 
        DisplayImage();
    }

    void DisplayImage()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            feedbacks[i].transform.SetSiblingIndex(tiles[i].transform.GetSiblingIndex());
        }
    }

    public void SwapTile(RectTransform _transfrom)
    {
        int lastEmptyTileIndex = emptyTile.GetSiblingIndex();
        
        emptyTile.SetSiblingIndex(_transfrom.GetSiblingIndex());
        _transfrom.SetSiblingIndex(lastEmptyTileIndex);

        
        DisplayImage();



        /*Vector2 lastEmptyTilePosition = emptyTile.position;
        emptyTile.localPosition = _transfrom.localPosition;
        _transfrom.localPosition = lastEmptyTilePosition;*/
    }

    public bool isSwappable(RectTransform _transfrom)
    {
        if (Vector2.Distance(emptyTile.anchoredPosition, _transfrom.anchoredPosition) <= minimalDistance * 100)
        {
            Debug.Log(Vector2.Distance(emptyTile.anchoredPosition, _transfrom.anchoredPosition));
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
                timeRemaining = 0;
                isTimerRunning = false;
            }
        }

        DisplayTime(timeRemaining);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
