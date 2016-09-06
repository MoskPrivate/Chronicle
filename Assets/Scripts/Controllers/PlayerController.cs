using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float interactionRadius;
    [SerializeField]
    float damageAmount;
    public LayerMask mask;
    public GameObject inventory;
    public GameObject playerGraphics;
    public GameObject moving;
    public GameObject idle;
    public bool isInteracting;

	void Update()
    {
        //If <<INTERACT>> Key is pressed
        if(Input.GetKey(KeyCode.Space))
        {
            

            Collider[] nearByColliders = Physics.OverlapSphere(transform.position, interactionRadius, mask);
            //if There are multiple colliders, find the nearest one.
            if(nearByColliders.Length > 1)
            {
                isInteracting = true;
                CheckAnimation();
                Collider closestCollider = nearByColliders[0];
                foreach (Collider c in nearByColliders)
                {
                    Vector3 minDist = Vector3.zero;
                    Vector3 distance = c.transform.position - transform.position;
                    if (distance.normalized.magnitude < minDist.normalized.magnitude)
                    {
                        minDist = distance;
                        closestCollider = c;
                    }
                }
                Interact(closestCollider);

            }
            //if only one collider;
            if(nearByColliders.Length == 1)
            {
                isInteracting = true;
                Interact(nearByColliders[0]);
            }
            if(nearByColliders.Length > 0)
            {
                GetComponent<PlayerMovementController>().enabled = false;
            }
            else
            {
                isInteracting = false;
                GetComponent<PlayerMovementController>().enabled = true;
            }
            CheckAnimation();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isInteracting = false;
            GetComponent<PlayerMovementController>().enabled = true;
        }
        CheckAnimation();
            
    }
    void CheckAnimation()
    {
        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            moving.SetActive(false);
            idle.SetActive(true);
        }
        else if (isInteracting)
        {
            moving.SetActive(false);
            idle.SetActive(true);
        }
        else 
        {
            idle.SetActive(false);
            moving.SetActive(true);
        }
        
    }
    void Interact(Collider target)
    {
        Vector3 direction = -(target.transform.position - this.transform.position).normalized;
        Vector3 snapDir = new Vector3(Mathf.RoundToInt(direction.x), 0, Mathf.RoundToInt(direction.z));
        playerGraphics.transform.LookAt(playerGraphics.transform.position + snapDir);
        if(target.transform.parent.GetComponent<Entity>() != null)
        {

            //Checking interaction type
            if(target.transform.parent.GetComponent<Entity>().interactionType == Entity.interactType.Damage)
            {
                target.transform.parent.GetComponent<Entity>().Damage(damageAmount, 0.5f);
            }
            if (target.transform.parent.GetComponent<Entity>().interactionType == Entity.interactType.Use)
            {
                target.transform.parent.GetComponent<Entity>().Use();
            }
        }
    }
    void OnDrawGizmos()
    {
        //Interaction Sphere Check
        //Gizmos.DrawSphere(transform.position, interactionRadius);
    }
}
