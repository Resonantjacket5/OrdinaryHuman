using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MovingEnemy : MonoBehaviour {

    // first waypoint index to find
    // and last index to stop at
    public int startIndexWay;
    public int topIndexWay;

    // velocity of the character
    public float maxSpeed = 10f;
    public float speed = 5f;

    // health script to find
    HealthScript hs;
    bool isMoving = false; // checks if already moving, to not set next target 
    bool collided = false; // checks if collided to exit 
    Rigidbody2D body;
    
    // list of waypoints to find
    List<WayPointScript> WayPointList;
    int currentTarget = -1;
    bool dead = false;

    // direction for ordinaryhuman to move towards
    // should be normalized 
    Vector2 direction;

    // acceleration of the speed
    public float acceleration = 2f;



    Vector2 respawnPosition;

	// Use this for initialization
	void Start () {

        respawnPosition = transform.position;


        body = gameObject.GetComponent<Rigidbody2D>();
        hs = gameObject.GetComponent<HealthScript>();

        WayPointList = GameObject.FindObjectsOfType<WayPointScript>().ToList<WayPointScript>();
        // order waypoint list then sort, cast into waypoint list, and assign
        WayPointList = WayPointList.OrderBy(wp => wp.wayIndex).ToList<WayPointScript>();
        Debug.Log(WayPointList);
        foreach(  WayPointScript wp in WayPointList)
        {
            Debug.Log(wp.wayIndex);
        }
        
        if(WayPointList.Count()>0)
        {
            currentTarget = startIndexWay;
        }
	}
	
	// Update is called once per frame
	void Update () {

        // if not moving
        // then move to next node
        if (!isMoving && (currentTarget < topIndexWay + 1))
        {
            collided = false;
            isMoving = true;
            //StartCoroutine(moveTo(WayPointList[currentTarget].transform.position));
            Debug.Log("start moving towards: " + WayPointList[currentTarget].transform.position);
        }
    }

    void FixedUpdate ()
    {
        if(currentTarget != (topIndexWay+1))
        {
            // normalize direction vector to 1 float
            direction = Vector2.ClampMagnitude((WayPointList[currentTarget].transform.position - transform.position), 1f);
            body.velocity = body.velocity + direction * acceleration * Time.fixedDeltaTime;
            if (body.velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                body.velocity = body.velocity.normalized * maxSpeed;
            }

        }
        else
        {
            // Ordinary human reaches last waypoint
            // stop moving
            //body.velocity = new Vector2(0, 0);
        }
    }

    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<WayPointScript>() != null)
        {
            if(currentTarget == other.GetComponent<WayPointScript>().wayIndex)
            {
                body.velocity = new Vector2(0, 0);
                // set collided to true to exit Coroutine Movetowards
                collided = true;
                // set next target
                currentTarget++;
            }
        }
    }

    /*
        Handle current waypoint count, and the next waypoint
    */
    void getPrevTarget()
    {

    }

    void getNextTarget()
    {

    }

    /*
        GUI handles buttons and labels 
    */

    public void die()
    {
        Destroy(gameObject);
    }
}
