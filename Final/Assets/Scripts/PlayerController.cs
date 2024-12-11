using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpPower = 10f;
    public float gravityModifier = 2f;
    public float ceiling = 12.0f;
    public float horizontalSpeed = 10f;
    public float verticalSpeed = 10f;
    public bool gameOver = false;
    private bool isOnGround = true;
    public float maxLeft = -5.2f;
    public float maxRight = 4.6f;
    public float maxUp = 5f;
    public float maxDown = -5f;
    private AudioSource audioSource;
    public AudioClip jumpSound;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameOver) return;

        HandleJump();
        HandleHorizontalMovement();
        HandleVerticalMovement();
        ClampCharacterPosition();
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isOnGround = false;
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        if (transform.position.y > ceiling)
        {
            transform.position = new Vector3(transform.position.x, ceiling, transform.position.z);
            playerRb.velocity = Vector3.zero;
        }
    }

    private void HandleHorizontalMovement()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        transform.Translate(Vector3.right * horizontalInput * horizontalSpeed * Time.deltaTime);
    }

    private void HandleVerticalMovement()
    {
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        transform.Translate(Vector3.forward * verticalInput * verticalSpeed * Time.deltaTime);
    }

    private void ClampCharacterPosition()
    {
        float clampedZ = Mathf.Clamp(transform.position.z, maxLeft, maxRight);
        float clampedY = Mathf.Clamp(transform.position.y, maxDown, maxUp);
        transform.position = new Vector3(transform.position.x, clampedY, clampedZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Object"))
        {
            gameOver = true;
            Debug.Log("GAME OVER!");
        }
    }
}
