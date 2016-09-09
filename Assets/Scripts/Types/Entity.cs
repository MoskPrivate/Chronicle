using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {

    public float health;
    private Inventory inventory;
    private float maxHealth;

    public enum entityType { Player, EnvObject, Building, Animal };
    public enum interactType { None,Damage, Use , Gather};

    public entityType Type;
    public interactType interactionType;

    public List<ResourceGiver> resourcesToGiveList = new List<ResourceGiver>();

    AudioSource asrc;

    float timeCounter = 0;

    void Awake()
    {
        maxHealth = health;
        inventory = GameObject.FindObjectOfType<Inventory>();
        asrc = GetComponent<AudioSource>();
    }
    public void Use(float _damageAmount, float _damageTime)
    {
        if(interactionType == interactType.Use)
        {
            //TODO: Do interaction type USE   
        }
        if(interactionType == interactType.Gather)
        {
            Damage(_damageAmount, _damageTime);
        }
    }

    public void Damage(float _damageAmount, float _timeBetweenDamages)
    {

        timeCounter += Time.deltaTime;
        if(timeCounter > _timeBetweenDamages)
        {
            health -= _damageAmount;
            timeCounter = 0;
        }
        
        if(health <= 0)
        {
            if(interactionType == interactType.Gather)
            {
                GiveResources();
            }
            Die();
        }
    }
    public void GetDamaged(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //GiveResources();
            Die();
        }
    }
    
    void Die()
    {
        
        Destroy(gameObject);
    }
    void GiveResources()
    {
        if(inventory != null)
        {
            if(inventory.GetComponent<Inventory>() != null)
            {
                for (int i = 0; i < resourcesToGiveList.Count; i++)
                {
                    inventory.GetComponent<Inventory>().AddItem(resourcesToGiveList[i].itemId, resourcesToGiveList[i].amount);
                }
                
            }
        }
    }

}
[System.Serializable]
public class ResourceGiver {
    public int itemId;
    public int amount;
}

