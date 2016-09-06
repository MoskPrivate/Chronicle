using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    [SerializeField]
	public float movementSpeed;
    Rigidbody rb;
    public GameObject graphic;

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
        if(velocity != Vector3.zero)
        {
            graphic.transform.rotation = Quaternion.LookRotation(-velocity.normalized);
        }
        rb.MovePosition(rb.transform.position + (velocity * movementSpeed * Time.fixedDeltaTime));
    }
}
	

