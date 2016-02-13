using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public int maxHealth=50;
    public int health=50;
    float lastDamageTime = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lastDamageTime += Time.deltaTime;
	}

    public int getHealth()
    {
        return health;
    }

    public void increaseHealth(int heal)
    {
        health += heal;
        checkHealth();
    }

    // don't allow damange if within 5 seconds
    public void decreaseHealthTimed(int damage)
    {
        if(lastDamageTime > 1f)
        {
            decreaseHealth(damage);
        }
    }

    public void decreaseHealth(int damage)
    {
        lastDamageTime = 0f;
        health -= damage;
        checkHealth();
    }

    
    void checkHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health<=0)
        {
            die();
        } 
    }

    void die()
    {
        if (transform.gameObject.GetComponent<OrdinaryHuman>() == null)
        {
            Destroy(transform.gameObject);
        }
        else
        {
            transform.gameObject.GetComponent<OrdinaryHuman>().die();
        }
        
    }
}
