using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] tileGraphics;

    public LayerMask obstacleLayer;
    public Color highlightedColor;
    public bool isWalkable;

    public Color creatableColor;
    public bool isCreatable;

    GameMaster gm;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int randTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randTile];
        gm = FindObjectOfType<GameMaster>();
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        return obstacle ? false : true;
    }

    public void Highlight()
    {
        rend.color = highlightedColor;
        isWalkable = true;

    }

    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;
        isCreatable = false;
    }

    private void OnMouseDown()
    {
        if (isWalkable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(this.transform.position);
        }else if (isCreatable)
        {
            BarrackItem item =  Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if(unit != null)
            {
                unit.hasAttacked = true;
                unit.hasMoved = true;
            }
        }
    }

    public void SetCreatable()
    {
        rend.color = creatableColor;
        isCreatable = true;
    }
}
