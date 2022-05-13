using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Photon.Pun;

public class GenerateMaze : MonoBehaviour
{
    public Maze mazeObj;
    [InspectorButton("GenerateMazeTexture", ButtonWidth = 200)]
    public bool generateMazeTexture;

    public Transform mazeOverlord;
    public GameObject wallSectionPrefab;
    public GameObject groundPrefab;
    public GameObject spawnAreaPrefab;
    public GameObject exitAreaPrefab;
    public Vector2 mazeSizeOverride;
    public int wallWidth;
    public int wallHeight;
    [InspectorButton("GenerateMazeObjects", ButtonWidth = 200)]
    public bool generateMaze;
    [InspectorButton("ClearMazeObjects", ButtonWidth = 200)]
    public bool clearMaze;

    private void GenerateMazeTexture()
    {
        mazeObj.TestMazeGenerator();
    }

    public Vector3 GenerateMazeObjects(Vector2 mazeSize)
    {
        ClearMazeObjects();

        Vector3 spawnBounds = new Vector3();

        GameObject ground = PhotonNetwork.InstantiateRoomObject("Ground", Vector3.zero, Quaternion.identity);
        ground.transform.parent = mazeOverlord;
        ground.transform.position = new Vector3(mazeSize.x * wallWidth / 2 - wallWidth / 2, 0, mazeSize.y * wallWidth / 2 - wallWidth / 2);
        ground.transform.localScale = new Vector3(mazeSize.x * wallWidth / 10, 1, mazeSize.y * wallWidth / 10);

        char[,] maze = mazeObj.GenerateMaze((int)mazeSize.x, (int)mazeSize.y);
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 'w')
                {
                    GameObject wallSection = PhotonNetwork.InstantiateRoomObject("WallSection", Vector3.zero, Quaternion.identity, 0);
                    wallSection.transform.parent = mazeOverlord;
                    wallSection.transform.position = new Vector3(wallWidth * i, wallHeight / 2, wallWidth * j);
                    wallSection.transform.localScale = new Vector3(wallWidth, wallHeight, wallWidth);
                } else if (maze[i, j] == 's')
                {
                    GameObject wallSection = PhotonNetwork.InstantiateRoomObject("SpawningArea", Vector3.zero, Quaternion.identity, 0);
                    wallSection.transform.parent = mazeOverlord;
                    wallSection.transform.position = new Vector3(wallWidth * i, wallHeight / 2, wallWidth * j);
                    wallSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);

                    spawnBounds = new Vector3(wallSection.transform.position.x, 5, wallSection.transform.position.z);
                    // spawnBounds = new Vector4(wallWidth * i - wallWidth / 2, wallWidth * j - wallWidth / 2, wallWidth * i + wallWidth / 2, wallWidth * j - wallWidth / 2);

                } else if (maze[i, j] == 'e')
                {
                    GameObject wallSection = PhotonNetwork.InstantiateRoomObject("ExitArea", Vector3.zero, Quaternion.identity, 0);
                    wallSection.transform.parent = mazeOverlord;
                    wallSection.transform.position = new Vector3(wallWidth * i, wallHeight / 2, wallWidth * j);
                    wallSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                }
            }
        }
        return spawnBounds;
    }

    public void ClearMazeObjects()
    {
        foreach (GameObject wallSection in GameObject.FindGameObjectsWithTag("WallSection")) {
            //Destroy(wallSection);
            DestroyImmediate(wallSection);
        }
        foreach (GameObject groundSection in GameObject.FindGameObjectsWithTag("GroundSection"))
        {
            //Destroy(wallSection);
            DestroyImmediate(groundSection);
        }
        foreach (GameObject groundSection in GameObject.FindGameObjectsWithTag("Spawn"))
        {
            //Destroy(wallSection);
            DestroyImmediate(groundSection);
        }
        foreach (GameObject groundSection in GameObject.FindGameObjectsWithTag("Exit"))
        {
            //Destroy(wallSection);
            DestroyImmediate(groundSection);
        }
    }

}