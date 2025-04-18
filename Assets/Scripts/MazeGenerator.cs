using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;
    [SerializeField]
    private int _mazeWidth;
    [SerializeField]
    private int _mazeDepth;
    [SerializeField]
    private MazeCell[,] _mazeGrid;
    [SerializeField]
    private GameObject _treePrefab;
    [SerializeField]
    private GameObject _bushPrefab;
    [SerializeField]
    private GameObject _coinPrefab;
    [SerializeField]
    private Camera _upCamera;
    [SerializeField]
    private Camera _characterCamera;
    
    public int _numberOfCoins = 50;

    private int visited_cells = 0;
    

    IEnumerator Start()
    {
        _upCamera.enabled = true;
        _characterCamera.enabled = false;
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeDepth; j++)
            {
                Vector3 position = new Vector3(i*7.5f, 0, j*7.5f);
                _mazeGrid[i, j] = Instantiate(_mazeCellPrefab, position, Quaternion.identity);
            }
        }
        yield return GenerateMaze(null, _mazeGrid[0, 0]);

        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeDepth; j++)
            {
                if (_mazeGrid[i, j].CanPlaceObject())
                {
                    int treeChance = UnityEngine.Random.Range(0, 100);
                    if(treeChance < 40)
                        _mazeGrid[i, j].PlaceObject(_treePrefab);
                    else if(treeChance < 80)
                    {
                        _mazeGrid[i, j].PlaceObject(_bushPrefab);
                    }
                }
            }
        }

        for (int i = 0; i < _mazeWidth; i++)
        {
            for (int j = 0; j < _mazeDepth; j++)
            {
                int coinChance = UnityEngine.Random.Range(0, 100);
                if(coinChance < 60 && !_mazeGrid[i,j].HasObject())
                {
                    _mazeGrid[i, j].PlaceCoin(_coinPrefab);
                    _numberOfCoins++;
                }
            }
        }

        yield return new WaitForSeconds(1.0f);

        yield return SmoothCameraTransition(2.0f);
        yield return null;
    }

    private IEnumerator SmoothCameraTransition(float duration)
    {
        float elapsed = 0f;

        Vector3 initialPosition = _upCamera.transform.position;
        Quaternion initialRotation = _upCamera.transform.rotation;

        Vector3 targetPosition = _characterCamera.transform.position;
        Quaternion targetRotation = _characterCamera.transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            _upCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            _upCamera.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            yield return null;
        }

        _upCamera.enabled = false;
        _characterCamera.enabled = true;
    }

    private IEnumerator GenerateMaze(MazeCell prevCell, MazeCell currCell)
    {
        visited_cells++;
        currCell.Visit();
        ClearWalls(prevCell, currCell);

        yield return new WaitForSeconds(0.02f);

        if (visited_cells == _mazeWidth * _mazeDepth)
            yield return null;

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currCell, nextCell);
            }
                
        } while (nextCell != null);

    }

    private MazeCell GetNextUnvisitedCell(MazeCell currCell)
    {
        var unvisitedCells = GetUnvisitedCells(currCell);
        return unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currCell)
    {
        int x = Mathf.RoundToInt(currCell.transform.position.x / 7.5f);
        int z = Mathf.RoundToInt(currCell.transform.position.z / 7.5f);

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (!cellToRight.IsVisited)
                yield return cellToRight;
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (!cellToLeft.IsVisited)
                yield return cellToLeft;
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (!cellToFront.IsVisited)
                yield return cellToFront;
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (!cellToBack.IsVisited)
                yield return cellToBack;
        }
    }

    private void ClearWalls(MazeCell prevCell, MazeCell currCell)
    {
        if (prevCell == null)
            return;

        Vector3 direction = currCell.transform.position - prevCell.transform.position;

        if (direction.x > 0)
        {
            prevCell.ClearRightWall();
            currCell.ClearLeftWall();
        }
        else if (direction.x < 0)
        {
            prevCell.ClearLeftWall();
            currCell.ClearRightWall();
        }
        else if (direction.z > 0)
        {
            prevCell.ClearFrontWall();
            currCell.ClearBackWall();
        }
        else if (direction.z < 0)
        {
            prevCell.ClearBackWall();
            currCell.ClearFrontWall();
        }
        MazeCell nextCell = GetNextUnvisitedCell(currCell);

        if (nextCell == null)
        {
            Instantiate(_treePrefab, currCell.transform.position, Quaternion.identity);
        }
    }
}
