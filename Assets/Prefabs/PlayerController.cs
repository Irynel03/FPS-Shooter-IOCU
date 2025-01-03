//using UnityEngine;

//public class PlayerController1 : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    [Header("Camera Settings")]
//    public Camera FPS_camera;

//    [Header("Weapon Settings")]
//    public GameObject activeWeapon; // Assign your weapon prefab here
//    public TextMesh ammoGUI;
//    public TextMesh healthGUI;

//    [Header("Player Settings")]
//    public int playerHealth = 100;
//    public float sensitivity = 30f;
//    public float moveSpeed = 5f;
//    public float jumpForce = 5f;

//    private float xRotation = 0f;
//    private CharacterController controller;
//    private Vector3 velocity;
//    private bool isGrounded;

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float groundDistance = 0.4f;
//    public LayerMask groundMask;

//    void Start()
//    {
//        // Lock and hide cursor for FPS gameplay
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;

//        controller = GetComponent<CharacterController>();

//        // Initialize weapon
//        if (activeWeapon != null)
//        {
//            activeWeapon.SetActive(true);
//        }
//    }

//    void Update()
//    {
//        HandleLook();
//        HandleMovement();
//        HandleJump();
//        UpdateUI();
//    }

//    void HandleLook()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//        FPS_camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//        transform.Rotate(Vector3.up * mouseX);
//    }

//    void HandleMovement()
//    {
//        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f;
//        }

//        float x = Input.GetAxis("Horizontal");
//        float z = Input.GetAxis("Vertical");

//        Vector3 move = transform.right * x + transform.forward * z;
//        controller.Move(move * moveSpeed * Time.deltaTime);

//        velocity.y += Physics.gravity.y * Time.deltaTime;
//        controller.Move(velocity * Time.deltaTime);
//    }

//    void HandleJump()
//    {
//        if (Input.GetButtonDown("Jump") && isGrounded)
//        {
//            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
//        }
//    }

//    void UpdateUI()
//    {
//        if (healthGUI != null)
//        {
//            healthGUI.text = "HP: " + playerHealth;
//        }

//        if (ammoGUI != null && activeWeapon != null)
//        {
//            // Example ammo display, replace with your weapon's actual ammo logic
//            ammoGUI.text = "Ammo: 30 / 90";
//        }
//    }

//    public void TakeDamage(int damage)
//    {
//        playerHealth -= damage;
//        if (playerHealth <= 0)
//        {
//            Die();
//        }
//    }

//    void Die()
//    {
//        Debug.Log("Player died!");
//        // Add respawn or game over logic here
//    }
//}
