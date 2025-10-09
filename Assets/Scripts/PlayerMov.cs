using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float GroundDrag;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horInput;
    float verInput;

    Vector3 moveDir;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MInput();

        if (grounded)
            rb.drag = GroundDrag;
        else
            rb.drag = 0;

        SpeedControl();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MInput()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");
    }
    private void MovePlayer()
    {
        moveDir = orientation.forward * verInput + orientation.right * horInput;
        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
