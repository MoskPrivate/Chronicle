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
        if(overlapColliders.Length > 0)
        {
            GameObject objectToInteract = GetNearestEntity(overlapColliders);
            if (objectToInteract == null)
            {
                Debug.Log("no near objects");
                return;
            }
            else
            {
                //Checking if the entity has an "Entity" component attached
                //Use the entitiy
                //TODO: If there are multiple colliders in the interaction sphere, get the nearest one
                GetEntity(objectToInteract.transform.parent.gameObject).Use(damageAmount, damageTime);
            }
            
        }
    }
    GameObject GetNearestEntity(Collider[] cols)
    {
        float smallestSqrDistance = 10000;
        GameObject closestObject = null;
        for (int i = 0; i < cols.Length; i++)
        {
            
            if (CheckIfIsAnEntity(cols[i].transform.parent.gameObject))
            {
               
                Vector3 offSet = cols[i].gameObject.transform.position - transform.position;
                float sqrDistance = offSet.sqrMagnitude;
                if (sqrDistance < smallestSqrDistance)
                {
                    smallestSqrDistance = sqrDistance;
                    closestObject = cols[i].gameObject;
                }
                
            }
           
        }
        Debug.Log(closestObject);
        return closestObject;
        
        

        
    }
    bool CheckIfIsAnEntity(GameObject col)
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
    Entity GetEntity(GameObject col)
    {
        return col.gameObject.GetComponent<Entity>();
    }
}
