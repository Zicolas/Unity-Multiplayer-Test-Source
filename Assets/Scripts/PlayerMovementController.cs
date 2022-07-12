using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerMovementController : NetworkBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public GameObject playerBody;

    public MeshRenderer PlayerMesh;
    public Material[] PlayerColors;

    private void Start()
    {
        playerBody.SetActive(false);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            if(playerBody.activeSelf == false)
            {
                SetPostion();
                playerBody.SetActive(true);
                PlayerCosmeticsSetup();
            }
            
            if(hasAuthority)
            {
                Move();
            }
        }
    }

    public void SetPostion()
    {
        transform.position = new Vector3(Random.Range(-10, 10), 1f, Random.Range(-10, 10));
    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }

    public void PlayerCosmeticsSetup()
    {
        PlayerMesh.material = PlayerColors[GetComponent<PlayerObjectController>().PlayerColor];
    }
}
