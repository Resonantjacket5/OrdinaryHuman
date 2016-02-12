using UnityEngine;
using System.Collections;

// decreases gameObject's Healthscript
// by onTriggerEnter
public class DamageScript : MonoBehaviour {

    public float delayNextDamageTime = 3f;
    public int damage = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<HealthScript>() != null)
        {
            HealthScript hs = other.GetComponent<HealthScript>();
            hs.decreaseHealthTimed(damage);
        }
    }
}
