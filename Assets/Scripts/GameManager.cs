using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<GameObject> tiles = new List<GameObject>();
    public RectTransform emptyTile = null;
    [SerializeField] private float minimalDistance;

    private List<Transform> pieces = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomPosition();
    }

    /// <summary>
    /// This method generate a random position for each tiles in the tiles List
    /// </summary>
    public void GenerateRandomPosition()
    {
        tiles[4].transform.SetAsLastSibling();
        tiles[4].SetActive(false);

        for (int i = 0; i < tiles.Count; i++)
        {
            //Generate a random integer
            int randomIndex = Random.Range(0, tiles.Count);

            //Change the position of the tile by the random index
            tiles[i].transform.SetSiblingIndex(randomIndex);
            pieces.Add(tiles[i].transform);
            //Make the last elements of the tiles List unactive
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit)
            {
                if (Vector2.Distance(emptyTile.position, hit.transform.position) < minimalDistance * 100)
                {
                    Vector2 lastEmptyTilePosition = emptyTile.position;
                    emptyTile.localPosition = hit.transform.localPosition;
                    hit.transform.localPosition = lastEmptyTilePosition;
                }

            }
        }*/
    }

    void SwapTile()
    {

    }

    public bool isSwappable(RectTransform _transfrom)
    {
        if (Vector2.Distance(emptyTile.anchoredPosition, _transfrom.anchoredPosition) <= minimalDistance * 100)
        {
            //Debug.Log(Vector2.Distance(emptyTile.anchoredPosition, _transfrom.anchoredPosition));
            return true;
        }
        return true;
    }
}
