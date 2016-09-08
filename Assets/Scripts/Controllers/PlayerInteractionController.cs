using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {

    public int interactionRadius;
    public float damageAmount;
    public float damageTime;
    public LayerMask entityLayerMask;
    	
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            
            CheckOverlapSphere();
        }
	}
    void CheckOverlapSphere()
    {
        Collider[] overlapColliders = Physics.OverlapSphere(gameObject.transform.position, interactionRadius,entityLayerMask);
        bool isAnEntity; 
        if(overlapColliders.Length > 0)
        {
            GameObject objectToInteract = overlapColliders[0].transform.parent.gameObject;
            //Checking if the entity has an "Entity" component attached
            isAnEntity = CheckIfIsAnEntity(objectToInteract);
            Debug.Log(isAnEntity);
            if (isAnEntity)
            {
                //Use the entitiy
                    //TODO: If there are multiple colliders in the interaction sphere, get the nearest one
                GetEntity(objectToInteract).Use(damageAmount, damageTime);
            }
        }
    }
    bool CheckIfIsAnEntity(GameObject col)
    {
        Debug.Log(col.name);
        if(col.gameObject.GetComponent<Entity>() != null)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }
    Entity GetEntity(GameObject col)
    {
        return col.gameObject.GetComponent<Entity>();
    }
}
