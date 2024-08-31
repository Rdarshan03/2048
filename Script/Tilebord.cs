
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilebord : MonoBehaviour
{
    public float swipeThreshold = 50f; // Minimum distance in pixels to recognize a swipe
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;


    public Manage gameManager;
    public Tile tilePrefab; 
    public Tilestate[] tileStates;

    private Tilegrid grid;
    private List<Tile> tiles;
    private bool waiting;

   
    private void Awake()
    {
        grid = GetComponentInChildren<Tilegrid>();
        tiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
         
        tiles.Clear();
    }
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0],2);
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    private void Update()
    {
        if (!waiting)
        {

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) )
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.Height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, grid.Width - 2, -1, 0, 1);
            }
            HandleKeyboardInput();
            HandleTouchInput();

        }
    }
    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTiles(Vector2Int.down, 0, 1, grid.Height - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTiles(Vector2Int.right, grid.Width - 2, -1, 0, 1);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                endTouchPosition = touch.position;
                Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                if (swipeDirection.magnitude > swipeThreshold)
                {
                    swipeDirection.Normalize();
                    HandleSwipe(swipeDirection);
                }

                isSwiping = false;
            }
        }
    }
    void HandleSwipe(Vector2 direction)
    {
        if (Vector2.Dot(direction, Vector2.right) > 0.5f)
        {
            MoveTiles(Vector2Int.right, grid.Width - 2, -1, 0, 1);
        }
        else if (Vector2.Dot(direction, Vector2.left) > 0.5f)
        {
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Vector2.Dot(direction, Vector2.up) > 0.5f)
        {
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Vector2.Dot(direction, Vector2.down) > 0.5f)
        {
            MoveTiles(Vector2Int.down, 0, 1, grid.Height - 2, -1);
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                Tilecell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }   
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Tilecell newCell = null;
        Tilecell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    AnimationClip clip = new AnimationClip();
                    Merge(tile, adjacent.tile);

                    return true;
                }

                break;
            }
            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction); 
        }
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }
    private bool CanMerge(Tile a, Tile b)
    {

        return a.number == b.number && !b.locked; 
    }

    private void Merge(Tile a , Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        int number = b.number * 2;

        b.SetState(tileStates[index], number);

        b.transform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.OutBounce).OnComplete(() => b.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InBack));

        gameManager.IncreaseScore(number);
    }
     
    private int IndexOf(Tilestate state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }
    private IEnumerator WaitForChanges()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count != grid.Size)
        {
            CreateTile();
        }
        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }
    public bool CheckForGameOver()
    {
        if (tiles.Count != grid.Size)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            Tilecell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            Tilecell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            Tilecell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left );
            Tilecell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        return true;
    }
}
