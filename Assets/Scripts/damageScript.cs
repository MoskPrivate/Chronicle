using UnityEngine;
using System.Collections;

public class damageScript : MonoBehaviour {

    public int damage; 
	void OnCollisionEnter(Collision col)
    {
        if(col.transform.root.GetComponent<Entity>() != null)
        {
            col.transform.root.GetComponent<Entity>().GetDamaged(damage);
        }
    }
}
