using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float airDrag;
    [SerializeField] private float groundDrag;
    private Vector3 move;
    private float forwardMovement, sidewaysMovement;
    private bool isGrounded;
    private Vector3 groundCheckBox;
    private Quaternion quat;

    [Header("Componenets/Layers")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform tf;
    [SerializeField] private BoxCollider hitbox;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator anim;


    private void Start()
    {
        rb.freezeRotation = true;
        groundCheckBox = new Vector3(hitbox.size.x / 2, .1f, hitbox.size.z);
        quat = new Quaternion(0, 0, 0, 0);
    }

    private void Update()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, groundCheckBox, quat, groundLayer);
        PlayerInput();
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("hop", 1);
        }
    }

    private void FixedUpdate()
    {
        //MovePlayer();
    }

    private void PlayerInput()
    {
        sidewaysMovement = Input.GetAxisRaw("Horizontal");
        forwardMovement = Input.GetAxisRaw("Vertical");
        move = tf.forward * forwardMovement + tf.right * sidewaysMovement;
        move = move.normalized;
    }

    private void MovePlayer()
    {
        print(move.normalized * moveSpeed);
        rb.AddForce(move.normalized * moveSpeed, ForceMode.Acceleration);
    }

    //Updates the drag of the player based on whether they are in the air or on ground
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

    }
}
