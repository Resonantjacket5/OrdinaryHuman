using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MovingEnemy : MonoBehaviour {


    // index to spawn at (the first actualy start one)
    public int spawnIndexWay = 0;
    // first waypoint index to find
    // and last index to stop at
    public int startIndexWay;
    public int topIndexWay;
    public GameObject WayParent;

    public float delayTime;

    public float maxSpeed = 4f; // velocity of the character
    public float speed = 5f; // deprecated and not being used
    public float acceleration = 2f; // acceleration of the speed
    Vector2 prevVelocity;
    
    HealthScript hs; // health script to find
    bool isMoving = false; // checks if already moving, to not set next target 
    bool collided = false; // checks if collided to exit 
    Rigidbody2D body;
    
    
    List<WayPointScript> WayPointList; // list of waypoints to find
    int currentTarget = -1; // if curTarget not set, shouldn't move
    bool dead = false;

    /*
        Keeps track if moving enemy is moving forwads 
        or moving backwards or not moving state
    //*/
    enum moveState { forward, stop, backward};
    moveState curMoveState;

    // direction for ordinaryhuman to move towards
    // should be normalized 
    Vector2 direction;


	// Use this for initialization
	void Start () {

        // fetch gameObject components
        body = gameObject.GetComponent<Rigidbody2D>();
        hs = gameObject.GetComponent<HealthScript>();

        // default moveState is forwards unless changed.
        curMoveState = moveState.forward;

        // fetch all waypoints 
        WayPointList = WayParent.GetComponentsInChildren<WayPointScript>().ToList();
        // order waypoint list then sort, cast into waypoint list, and assign
        WayPointList = WayPointList.OrderBy(wp => wp.wayIndex).ToList<WayPointScript>();
        
        // if at least one waypoint exists, then set target to start        
        if(WayPointList.Count()>0)
        {
            currentTarget = spawnIndexWay;
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
        MoveTowardsWaypoint();
        FixRotation();
    }

    void MoveTowardsWaypoint()
    {
        if (curMoveState == moveState.stop)
        {
            // do nothing while stopped
        }
        else if (currentTarget != (topIndexWay + 1))
        {
            // normalize direction vector to 1 float
            direction = Vector2.ClampMagnitude((WayPointList[currentTarget].transform.position - transform.position), 1f);
            body.velocity = body.velocity + direction * acceleration * Time.fixedDeltaTime;
            //Debug.Log(body.velocity);

            // set max speed
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

    void FixRotation()
    {
        Vector2 v = body.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //Quaternion.LookRotation(body.velocity);
    }

    public Vector2 getVelocity()
    {
        return body.velocity;
    }


    /*
        When reach next waypoint, set enemy
        to move towards next target
    */

    public void SwitchState(string nState )
    {
        Debug.Log("switch state called "+nState);

        moveState nextState;

        // find correct nextState to turn into
        if (nState == "forward")
            nextState = moveState.forward;
        else if (nState == "stop")
            nextState = moveState.stop;
        else if (nState == "backward")
            nextState = moveState.backward;
        else
        {
            Debug.Log("crash");
            throw new System.Exception(" incorrect next state passed");
        }

        // save previous velocity
        // and then set current body.velcoity to 0,0
        if (nextState == moveState.stop && curMoveState == moveState.forward)
        {
            prevVelocity = body.velocity;
            body.velocity = new Vector2(0, 0);
        }
        /*
            Switch velocity back to the forward state
        //*/
        else if (nextState == moveState.forward && curMoveState == moveState.stop)
        {
            body.velocity = prevVelocity;
        }

        curMoveState = nextState;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<WayPointScript>() != null)
        {
            if(currentTarget == other.GetComponent<WayPointScript>().wayIndex)
            {

                // when waypoint found, stop moving
                body.velocity = new Vector2(0, 0);

                if  (
                    (curMoveState == moveState.forward) && 
                    (currentTarget == topIndexWay)
                )
                    
                {
                    // do code to teleport back to the beginning
                    transform.position = WayPointList[startIndexWay].transform.position;
                    currentTarget = startIndexWay;
                    getNextTarget();
                }
                
                else if 
                (
                    (curMoveState == moveState.backward) &&
                    (currentTarget == startIndexWay)
                )
                {
                    // do code to teleport to the end
                }
                else
                {
                    // set next target
                    getNextTarget();
                    Debug.Log(currentTarget);
                }
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
        if(curMoveState==moveState.forward)
        {
            currentTarget++;
        }
        else if (curMoveState == moveState.backward)
        {
            currentTarget--;
        }
        
        currentTarget = correctIndex(currentTarget);
    }

    // correct if moves past start or end 
    int correctIndex(int index)
    {
        if (index < startIndexWay)
        {
            return topIndexWay;
        }
        else if (index > topIndexWay)
        {
            return startIndexWay;
        }
        else
            return index;
    }

    /*
        GUI handles buttons and labels 
    */

    public void die()
    {
        Destroy(gameObject);
    }
}
