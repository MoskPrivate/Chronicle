using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    [SerializeField]
	public float movementSpeed;
    Rigidbody rb;
    public GameObject graphic;
    public enum waterPosition { left, right, top, bottom , none , topLeft, topRight, bottomRight, bottomLeft };

    //BOOLS
    bool left;
    bool right;
    bool top;
    bool bottom;
    bool topLeft;
    bool topRight;
    bool bottomRight;
    bool bottomLeft;


    public LayerMask groundMask;
    public float waterRayThreshold;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        

        
    }
    void FixedUpdate()
    {
        Vector3 velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (velocity != Vector3.zero)
        {
            graphic.transform.rotation = Quaternion.LookRotation(-velocity.normalized);
        }
        IsNextToWater();
        if (top)
        {
            if(velocity.z > 0)
            {
                velocity.z = 0;
            }
        }
        if (bottom)
        {
            if (velocity.z < 0)
            {
                velocity.z = 0;
            }
        }
        if (left)
        {
            if (velocity.x < 0)
            {
                velocity.x = 0;
            }
        }
        if (right)
        {
            if (velocity.x > 0)
            {
                velocity.x = 0;
            }
        }

        // DIAGONALS

        if (topLeft)
        {
            if (velocity.x < 0)
            {
                velocity.x = 0;
            }
            if (velocity.z > 0)
            {
                velocity.z = 0;
            }
        }
        if (topRight)
        {
            if (velocity.x > 0)
            {
                velocity.x = 0;
            }
            if (velocity.z > 0)
            {
                velocity.z = 0;
            }
        }
        if (bottomRight)
        {
            if (velocity.x > 0)
            {
                velocity.x = 0;
            }
            if (velocity.z < 0)
            {
                velocity.z = 0;
            }
        }
        if (bottomLeft)
        {
            if (velocity.x < 0)
            {
                velocity.x = 0;
            }
            if (velocity.z < 0)
            {
                velocity.z = 0;
            }
        }


        rb.MovePosition(rb.transform.position + (velocity * movementSpeed * Time.fixedDeltaTime));
    }
    void IsNextToWater()
    {
        //TOP
        Ray rayTop = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + waterRayThreshold),-Vector3.up);
        if (!Physics.Raycast(rayTop, Mathf.Infinity, groundMask))
        {
            top = true;
        }
        else
        {
            top = false;
        }
        //BOTTOM
        Ray rayBottom = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - waterRayThreshold), -Vector3.up);
        if (!Physics.Raycast(rayBottom, Mathf.Infinity, groundMask))
        {
            bottom = true;
        }
        else
        {
            bottom = false;
        }
        //LEFT
        Ray rayLeft = new Ray(new Vector3(transform.position.x - waterRayThreshold, transform.position.y, transform.position.z), -Vector3.up);
        if (!Physics.Raycast(rayLeft, Mathf.Infinity, groundMask))
        {
            left = true;
        }
        else
        {
            left = false;
        }
        //RIGHT
        Ray rayRight = new Ray(new Vector3(transform.position.x + waterRayThreshold, transform.position.y, transform.position.z + waterRayThreshold), -Vector3.up);
        if (!Physics.Raycast(rayRight, Mathf.Infinity, groundMask))
        {
            right = true;
        }
        else
        {
            right = false;
        }
        //DIAGONALS

        //TOP LEFT
        Ray rayTopLeft = new Ray(new Vector3(transform.position.x - waterRayThreshold/2, transform.position.y, transform.position.z + waterRayThreshold/2), -Vector3.up);
        if (!Physics.Raycast(rayTopLeft, Mathf.Infinity, groundMask))
        {
            topLeft = true;
        }
        else
        {
            topLeft = false;
        }
        //TOP RIGHT
        Ray rayTopRight = new Ray(new Vector3(transform.position.x + waterRayThreshold / 2, transform.position.y, transform.position.z + waterRayThreshold / 2), -Vector3.up);
        if (!Physics.Raycast(rayTopRight, Mathf.Infinity, groundMask))
        {
            topRight = true;
        }
        else
        {
            topRight = false;
        }
        //BOTTOM RIGHT
        Ray rayBottomRight = new Ray(new Vector3(transform.position.x + waterRayThreshold/2, transform.position.y, transform.position.z - waterRayThreshold/2), -Vector3.up);
        if (!Physics.Raycast(rayBottomRight, Mathf.Infinity, groundMask))
        {
            bottomRight = true;
        }
        else
        {
            bottomRight = false;
        }
        //TOP RIGHT
        Ray rayBottomLeft = new Ray(new Vector3(transform.position.x - waterRayThreshold/2, transform.position.y, transform.position.z - waterRayThreshold/2), -Vector3.up);
        if (!Physics.Raycast(rayBottomLeft, Mathf.Infinity, groundMask))
        {
            bottomLeft = true;
        }
        else
        {
            bottomLeft = false;
        }

    }
}
	

