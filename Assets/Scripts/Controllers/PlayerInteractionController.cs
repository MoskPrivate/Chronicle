using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {

    public int interactionRadius;
    public float damageAmount;
    public float damageTime;
    	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckOverlapSphere();
        }
	}
    void CheckOverlapSphere()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(gameObject.transform.position, interactionRadius);
        bool isAnEntity; 
        if(overlapColliders.Length > 0)
        {
            //Checking if the entity has an "Entity" component attached
            isAnEntity = CheckIfIsAnEntity(overlapColliders[0]);
            if (isAnEntity)
            {
                //Use the entitiy
                //TODO: If there are multiple colliders in the interaction sphere, get the nearest one
                GetEntity(overlapColliders[0]).Use(damageAmount, damageTime);
            }
        }
    }
    bool CheckIfIsAnEntity(Collider col)
    {
        if(col.gameObject.GetComponent<Entity>() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Entity GetEntity(Collider col)
    {
        return col.gameObject.GetComponent<Entity>();
    }
}
