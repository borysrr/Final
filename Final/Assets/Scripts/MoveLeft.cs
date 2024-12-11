using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private float moveSpeed = 30f;
    private PlayerController playerScript;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerScript.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
    }
}
