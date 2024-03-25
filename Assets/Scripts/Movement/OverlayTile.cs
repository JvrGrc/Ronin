using System.Collections.Generic;
using UnityEngine;
using static ArrowTranslator;
public class OverlayTile : MonoBehaviour
{
    [SerializeField] int G;
    [SerializeField] int H;
    [SerializeField] bool isBlocked = false;
    [SerializeField] OverlayTile previous;
    [SerializeField] List<Sprite> arrows;
    [SerializeField] Vector3Int gridLocation;

    public int GetG() { return G; }
    public void SetG(int value) { G = value; }

    public int GetH() { return H; }
    public void SetH(int value) { H = value; }

    public int F { get { return G + H; } }

    public bool IsBlocked() { return isBlocked; }

    public OverlayTile GetPrevious() { return previous; }
    public void SetPrevious(OverlayTile value) { previous = value; }

    public Vector3Int GetGridLocation() { return gridLocation; }
    public void SetGridLocation(Vector3Int value) { gridLocation = value; }
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void ShowTile1()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.45f, 0.45f, 1);
    }

    public void SetSprite(ArrowDirection d)
    {
        if (d == ArrowDirection.None)
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        else
        {
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
            GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)d];
            GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
