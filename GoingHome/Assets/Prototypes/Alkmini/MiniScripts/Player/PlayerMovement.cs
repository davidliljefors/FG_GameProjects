//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerMovement: MonoBehaviour
//{
//    //Right-Left Variables

//    public float speed = 10;
//    public float moveLimiter = 0.7f;
    
//    private Rigidbody m_rb;

//    //Jump variables
//    public float jumpVeclocity = 5f;
//    public float fallMultiplier = 2.5f;
//    public float lowJumpMultiplier = 2f;

//    private void Start()
//    {
//        m_rb = GetComponent<Rigidbody>();
//    }

//    private void FixedUpdate()
//    {
//        Move();
//        Jump();
        
//    }

//    void Jump()
//    {
//        if (Input.GetButtonDown("Jump"))
//        {
//            m_rb.velocity = new Vector3(m_rb.velocity.x, 0, 0);
//            m_rb.velocity += Vector3.up * jumpVeclocity;

//        }
//        if (m_rb.velocity.y < 0)
//        {
//            m_rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
//        }
//        else if (m_rb.velocity.y > 0 && !Input.GetButton("Jump"))
//        {
//            m_rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
//        }

//    }

//    void Move()
//    {
//        float moveHorizontal = Input.GetAxis("Horizontal_P1");
//        Vector3 movement = new Vector3(moveHorizontal, 0f, 0f);
//        Vector3 currentPos = transform.position += movement * Time.deltaTime * speed;

//    }
   
//}
