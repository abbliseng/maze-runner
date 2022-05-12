using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GenerateMaze : MonoBehaviour
{
    public Maze mazeObj;
    [InspectorButton("GenerateMazeTexture", ButtonWidth = 200)]
    public bool generateMazeTexture;

    public GameObject wallSectionPrefab;
    public GameObject groundPrefab;
    public GameObject spawnAreaPrefab;
    public GameObject exitAreaPrefab;
    public Vector2 mazeSize;
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

    public Vector4 GenerateMazeObjects()
    {
        ClearMazeObjects();

        Vector4 spawnBounds = new Vector4();

        GameObject ground = Instantiate(groundPrefab);
        ground.transform.position = new Vector3(mazeSize.x * wallWidth / 2 - wallWidth / 2, 0, mazeSize.y * wallWidth / 2 - wallWidth / 2);
        ground.transform.localScale = new Vector3(mazeSize.x * wallWidth / 10, 1, mazeSize.y * wallWidth / 10);

        char[,] maze = mazeObj.GenerateMaze((int)mazeSize.x, (int)mazeSize.y);
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 'w')
                {
                    GameObject wallSection = Instantiate(wallSectionPrefab);
                    wallSection.transform.position = new Vector3(wallWidth * i, wallHeight/2, wallWidth * j);
                    wallSection.transform.localScale = new Vector3(wallWidth, wallHeight, wallWidth);
                } else if (maze[i, j] == 's')
                {
                    GameObject wallSection = Instantiate(spawnAreaPrefab);
                    wallSection.transform.position = new Vector3(wallWidth * i, wallHeight / 2, wallWidth * j);
                    wallSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);

                    spawnBounds = new Vector4(wallWidth * i - wallWidth / 2, wallWidth * j - wallWidth / 2, wallWidth * i + wallWidth / 2, wallWidth * j - wallWidth / 2);

                } else if (maze[i, j] == 'e')
                {
                    GameObject wallSection = Instantiate(exitAreaPrefab);
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