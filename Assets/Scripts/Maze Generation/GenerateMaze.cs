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

        char[,] maze = mazeObj.GenerateMaze((int)mazeSize.x, (int)mazeSize.y);
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                GameObject newSection;
                string wallString = "";
                if (maze[i,j] == 'c' || maze[i,j] == 's' || maze[i,j] == 'e')
                {
                    wallString += maze[i - 1, j] == 'w' ? 'X' : 'C';
                    wallString += maze[i + 1, j] == 'w' ? 'X' : 'C';
                    wallString += maze[i, j - 1] == 'w' ? 'X' : 'C';
                    wallString += maze[i, j + 1] == 'w' ? 'X' : 'C';

                    newSection = PhotonNetwork.InstantiateRoomObject(wallString, Vector3.zero, Quaternion.identity, 0);
                    newSection.transform.parent = mazeOverlord;
                    newSection.transform.position = new Vector3(wallWidth * i, -0.5f + 1f / 12f, wallWidth * j);
                    newSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                    if (maze[i, j] == 's')
                    {
                        GameObject spawnSection = PhotonNetwork.InstantiateRoomObject("SpawningArea", Vector3.zero, Quaternion.identity, 0);
                        spawnSection.transform.parent = mazeOverlord;
                        spawnSection.transform.position = new Vector3(wallWidth * i, wallWidth / 2, wallWidth * j);
                        spawnSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);

                        spawnBounds = new Vector3(spawnSection.transform.position.x, 5, spawnSection.transform.position.z);
                    } else if (maze[i, j] == 'e')
                    {
                        GameObject exitSection = PhotonNetwork.InstantiateRoomObject("ExitArea", Vector3.zero, Quaternion.identity, 0);
                        exitSection.transform.parent = mazeOverlord;
                        exitSection.transform.position = new Vector3(wallWidth * i, 0, wallWidth * j);
                        exitSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                    }
                }
            }
        }
        return spawnBounds;
    }

    public void ClearMazeObjects()
    {
        foreach (GameObject wallSection in GameObject.FindGameObjectsWithTag("WallSection")) {
            //Destroy(wallSection);
            PhotonNetwork.Destroy(wallSection);
        }
        foreach (GameObject groundSection in GameObject.FindGameObjectsWithTag("GroundSection"))
        {
            //Destroy(wallSection);
            PhotonNetwork.Destroy(groundSection);
        }
        foreach (GameObject groundSection in GameObject.FindGameObjectsWithTag("Spawn"))
        {
            //Destroy(wallSection);
            PhotonNetwork.Destroy(groundSection);
        }
        foreach (GameObject groundSection in GameObject.FindGameObjectsWithTag("Exit"))
        {
            //Destroy(wallSection);
            PhotonNetwork.Destroy(groundSection);
        }
    }

}