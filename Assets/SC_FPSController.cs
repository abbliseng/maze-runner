using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public Camera playerPixelCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public PhotonView view;

    [HideInInspector]
    public bool canMove = true;


    public GameObject mainMenu;
    public GameObject adminMenu;
    public TMPro.TMP_InputField mapWidth;
    public TMPro.TMP_InputField mapHeight;

    private GenerateMaze generateMaze;
    private Vector3 mazeStart;


    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get ref to the maze generator
        generateMaze = GameObject.FindGameObjectWithTag("MazeGenerator").GetComponent<GenerateMaze>();
        playerCamera = GameObject.FindGameObjectWithTag("RenderCamera").GetComponent<Camera>();
    }

    public void genMaze()
    {
        if (generateMaze != null && int.Parse(mapWidth.text) > 0 && int.Parse(mapHeight.text) > 0)
        {
            Vector2 mazeSize = new Vector2(int.Parse(mapWidth.text), int.Parse(mapHeight.text));
            mazeStart = generateMaze.GenerateMazeObjects(mazeSize);
        }
    }

    public void teleportPlayersToStart()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        gameObject.GetComponent<CharacterController>().enabled = true;
        
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenu.SetActive(!mainMenu.activeSelf);
                if (PhotonNetwork.IsMasterClient)
                {
                    adminMenu.SetActive(!adminMenu.activeSelf);
                }
                Cursor.lockState = mainMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = mainMenu.activeSelf ? true : false;
            }

            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove && !mainMenu.activeSelf)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                // playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                playerPixelCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        } else
        {
            // playerCamera.enabled = false;
            playerPixelCamera.enabled = false;
        }
    }
}