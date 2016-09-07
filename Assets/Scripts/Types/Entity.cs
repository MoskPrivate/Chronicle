using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public float health;
    public GameObject inventory;
    [HideInInspector]
    private float maxHealth;

    public enum entityType { Player, EnvObject, Building, Animal };
    public enum interactType { None,Damage, Use };

    public int resourceID;
    public entityType Type;
    public interactType interactionType;

    AudioSource asrc;

    float timeCounter = 0;

    void Awake()
    {
        maxHealth = health;
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        asrc = GetComponent<AudioSource>();
    }
    public void Use()
    {

    }

    public void Damage(float amount, float strikeRate)
    {

        if(timeCounter > strikeRate)
        {
            timeCounter = 0;
            health -= amount;
            asrc.Play();
        }
        timeCounter += Time.time;

        
        if(health <= 0)
        {
            asrc.Play();
            Die();
        }
    }
    public void GetDamaged(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        if(Type == entityType.EnvObject)
        {
            GiveResources();
        }
        Destroy(gameObject);
    }
    void GiveResources()
    {
        if(inventory != null)
        {
            if(inventory.GetComponent<Inventory>() != null)
            {
                inventory.GetComponent<Inventory>().AddItem(resourceID, 5);
            }
        }
    }

}

