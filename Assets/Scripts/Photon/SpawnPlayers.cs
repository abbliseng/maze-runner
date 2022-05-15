using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GenerateMaze maze_generator;

    private Vector3 spawnBounds = new Vector3(0,105,0);

    public void Start()
    {
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Generate Maze :)");
            int mazeWidth = (int)Random.Range(50, 100);
            int mazeHeight = (int)Random.Range(50, 100);
            Debug.Log("New maze size is: "+mazeWidth+"*"+mazeHeight);
            spawnBounds = maze_generator.GenerateMazeObjects(new Vector2(mazeWidth, mazeHeight));
            Debug.Log(spawnBounds);
        }
        */

        Vector3 randomPosition = spawnBounds;
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        // player.transform.localScale = new Vector3(Random.Range(0.2f, 1.5f), Random.Range(0.2f, 1.5f), Random.Range(0.2f, 1.5f));
        // player.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
        //.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}
