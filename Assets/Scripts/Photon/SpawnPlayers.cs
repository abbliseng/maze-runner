using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GenerateMaze maze_generator;

    private Vector3 spawnBounds;

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Generate Maze :)");
            int mazeWidth = (int)Random.Range(50, 100);
            int mazeHeight = (int)Random.Range(50, 100);
            Debug.Log("New maze size is: "+mazeWidth+"*"+mazeHeight);
            spawnBounds = maze_generator.GenerateMazeObjects(new Vector2(mazeWidth, mazeHeight));
            Debug.Log(spawnBounds);
        }

        Vector3 randomPosition = spawnBounds;
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
