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

        /*
        GameObject ground = PhotonNetwork.InstantiateRoomObject("Ground", Vector3.zero, Quaternion.identity);
        ground.transform.parent = mazeOverlord;
        ground.transform.position = new Vector3(mazeSize.x * wallWidth / 2 - wallWidth / 2, 0, mazeSize.y * wallWidth / 2 - wallWidth / 2);
        ground.transform.localScale = new Vector3(mazeSize.x * wallWidth / 10, 1, mazeSize.y * wallWidth / 10);
        ground.GetComponent<Renderer>().material.mainTextureScale = new Vector2(wallWidth, wallHeight);
        
        GameObject roof = PhotonNetwork.InstantiateRoomObject("Ground", Vector3.zero, Quaternion.identity);
        roof.transform.parent = mazeOverlord;
        roof.transform.position = new Vector3(mazeSize.x * wallWidth / 2 - wallWidth / 2, 5, mazeSize.y * wallWidth / 2 - wallWidth / 2);
        roof.transform.localScale = new Vector3(mazeSize.x * wallWidth / 10, 1, mazeSize.y * wallWidth / 10);
        roof.GetComponent<Renderer>().material.mainTextureScale = new Vector2(wallWidth, wallHeight);
        // roof.transform.Rotate(0, 180, 0);
        */

        char[,] maze = mazeObj.GenerateMaze((int)mazeSize.x, (int)mazeSize.y);
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                string wallString = "";
                if (maze[i,j] == 'c' || maze[i,j] == 's' || maze[i,j] == 'e')
                {
                    wallString += maze[i - 1, j] == 'w' ? 'X' : 'C';
                    wallString += maze[i + 1, j] == 'w' ? 'X' : 'C';
                    wallString += maze[i, j - 1] == 'w' ? 'X' : 'C';
                    wallString += maze[i, j + 1] == 'w' ? 'X' : 'C';
                }
                switch (wallString)
                {
                    case "XXXX":
                        //  X
                        // XCX
                        //  X
                        break;
                    // ÅTERVÄNDSGRÄNDER
                    case "CXXX":
                        //  X
                        // CCX
                        //  X
                        break;
                    case "XCXX":
                        //  X
                        // XCC
                        //  X
                        break;
                    case "XXCX":
                        //  X
                        // XCX
                        //  C
                        break;
                    case "XXXC":
                        //  C
                        // XCX
                        //  X
                        break;
                    // KORRIDORER
                    case "CCXX":
                        //  X
                        // CCC
                        //  X
                        break;
                    case "CXCX":
                        //  X
                        // CCX
                        //  C
                        break;
                    case "CXXC":
                        //  C
                        // CCX
                        //  X
                        break;
                    case "XCCX":
                        //  X
                        // XCC
                        //  C
                        break;
                    // 3-VÄGS KORSNINGAR
                    case "CCCX":
                        //  X
                        // CCC
                        //  C
                        break;
                    case "CXCC":
                        //  C
                        // CCX
                        //  C
                        break;
                    case "CCXC":
                        //  C
                        // CCC
                        //  X
                        break;
                    case "XCCC":
                        //  C
                        // XCC
                        //  C
                        break;
                    // 4-VÄGS KORSNING
                    case "CCCC":
                        //  C
                        // CCC
                        //  C
                        break;
                }

                /*if (maze[i, j] == 'w')
                {
                    GameObject wallSection = PhotonNetwork.InstantiateRoomObject("WallSection", Vector3.zero, Quaternion.identity, 0);
                    Debug.Log("Placing walls: " + gameObject.name);
                    wallSection.transform.parent = mazeOverlord;
                    wallSection.transform.position = new Vector3(wallWidth * i, wallWidth/2, wallWidth * j);
                    wallSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                    // wallSection.GetComponent<Renderer>().material.mainTextureScale = new Vector2(wallWidth, wallHeight);
                } else */if (maze[i, j] == 's')
                {
                    GameObject spawnSection = PhotonNetwork.InstantiateRoomObject("SpawningArea", Vector3.zero, Quaternion.identity, 0);
                    spawnSection.transform.parent = mazeOverlord;
                    spawnSection.transform.position = new Vector3(wallWidth * i, wallWidth/2, wallWidth * j);
                    spawnSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);

                    spawnBounds = new Vector3(spawnSection.transform.position.x, 5, spawnSection.transform.position.z);

                } else if (maze[i, j] == 'e')
                {
                    GameObject exitSection = PhotonNetwork.InstantiateRoomObject("ExitArea", Vector3.zero, Quaternion.identity, 0);
                    exitSection.transform.parent = mazeOverlord;
                    exitSection.transform.position = new Vector3(wallWidth * i, 0, wallWidth * j);
                    exitSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                }
                if (maze[i, j] == 'c' || maze[i, j] == 's' || maze[i, j] == 'e')
                {
                    GameObject flatSection = PhotonNetwork.InstantiateRoomObject("FlatSection", Vector3.zero, Quaternion.identity, 0);
                    flatSection.transform.parent = mazeOverlord;
                    flatSection.transform.position = new Vector3(wallWidth * i, -0.5f+1f/12f, wallWidth * j);
                    flatSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                    /*
                    GameObject flatTopSection = PhotonNetwork.InstantiateRoomObject("RoofSection", Vector3.zero, Quaternion.identity, 0);
                    flatTopSection.transform.parent = mazeOverlord;
                    flatTopSection.transform.position = new Vector3(wallWidth * i, wallWidth - 1f - 2f / 6f, wallWidth * j);
                    flatTopSection.transform.localScale = new Vector3(wallWidth, wallWidth, wallWidth);
                    */
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