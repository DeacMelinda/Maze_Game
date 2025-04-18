using System;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;
    [SerializeField]
    private GameObject _rightWall;
    [SerializeField]
    private GameObject _frontWall;
    [SerializeField]
    private GameObject _backWall;
    [SerializeField]
    private GameObject _unvisitedBlock;


    public bool IsVisited { get; private set; }
    public bool IsLeftWallCleared { get; private set; }
    public bool IsRightWallCleared { get; private set; }
    public bool IsFrontWallCleared { get; private set; }

    public bool IsBackWallCleared { get; private set; }

    private int clearedWalls = 0;

    private bool hasObject = false;

    public void Visit()
    {
       IsVisited = true;
       _unvisitedBlock.SetActive(false);
    }
    
    public void ClearLeftWall()
    {
        IsLeftWallCleared = true;
        clearedWalls++;
        _leftWall.SetActive(false);
    }
    public void ClearRightWall()
    {
        IsRightWallCleared = true;
        clearedWalls++;
        _rightWall.SetActive(false);
    }
    public void ClearFrontWall()
    {
        IsFrontWallCleared = true;
        clearedWalls++;
        _frontWall.SetActive(false);
    }
    public void ClearBackWall()
    {
        IsBackWallCleared = true;
        clearedWalls++;
        _backWall.SetActive(false);
    }

    public bool CanPlaceObject()
    {
        if(clearedWalls != 2)
            return false;
        if (IsLeftWallCleared && IsRightWallCleared)
            return false;
        if (IsFrontWallCleared && IsBackWallCleared)
            return false;
        return !hasObject;
    }

    public void PlaceObject(GameObject prefab)
    {
        float rotate = UnityEngine.Random.Range(-5f, 5f);
        if (IsLeftWallCleared && IsBackWallCleared)
        {
            Vector3 position = new Vector3(transform.position.x+0.5f, transform.position.y, transform.position.z+0.5f);
            Instantiate(prefab, position, Quaternion.identity);
            hasObject = true;
        }
        if (IsLeftWallCleared && IsFrontWallCleared)
        {
            Vector3 position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z - 0.5f);
            Instantiate(prefab, position, Quaternion.identity);
            hasObject = true;
        }
        if (IsRightWallCleared && IsFrontWallCleared)
        {
            Vector3 position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z - 0.5f);
            Instantiate(prefab, position, Quaternion.identity);
            hasObject = true;
        }
        if (IsRightWallCleared && IsBackWallCleared)
        {
            Vector3 position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z + 0.5f);
            Instantiate(prefab, position, Quaternion.identity);
            hasObject = true;
        }
    }

    public void PlaceCoin(GameObject coinPrefab)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Instantiate(coinPrefab, position, Quaternion.identity);
    }

    public bool HasObject()
    {
        return hasObject;
    }
}
