using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class OrdinaryHuman : MonoBehaviour {

    // first waypoint index to find
    // and last index to stop at
    public int startIndexWay;
    public int topIndexWay;
    public GameObject WayParent;

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
    List<MovingEnemy> MovingEnemyList;

    int currentTarget = -1;
    bool dead = false;
    bool isAnyPressed = false;

    // direction for ordinaryhuman to move towards
    // should be normalized 
    Vector2 direction;

    // acceleration of the speed
    public float acceleration = 2f;

    string oppDirection = "";

    Vector2 respawnPosition;

	// Use this for initialization
	void Start () {

        respawnPosition = transform.position;


        body = gameObject.GetComponent<Rigidbody2D>();
        hs = gameObject.GetComponent<HealthScript>();

        // order waypoint list then sort, cast into waypoint list, and assign
        WayPointList = WayParent.GetComponentsInChildren<WayPointScript>().ToList();
        WayPointList = WayPointList.OrderBy(wp => wp.wayIndex).ToList<WayPointScript>();

        MovingEnemyList = GameObject.FindObjectsOfType<MovingEnemy>().ToList<MovingEnemy>();

        if (WayPointList.Count() > 0)
        {
            currentTarget = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {

        StopEnemies();
    }

    void StopEnemies()
    {
        bool w = Input.GetKeyDown(KeyCode.W);
        bool a = Input.GetKeyDown(KeyCode.A);
        bool s = Input.GetKeyDown(KeyCode.S);
        bool d = Input.GetKeyDown(KeyCode.D);

        
        // w,a,s,or d not pressed yet
        if (!isAnyPressed)
        {
            if (w)
                oppDirection = "w";
            else if (a)
                oppDirection = "a";
            else if (s)
                oppDirection = "s";
            else if (d)
                oppDirection = "d";
            else
                oppDirection = "";

            Debug.Log("input: " + oppDirection);
            if (w || a || s || d)
            {

                isAnyPressed = true;

                List<MovingEnemy> MovingEnemyToStopList = new List<MovingEnemy>();

                foreach (MovingEnemy me in MovingEnemyList)
                {
                    if (correctEnemy(oppDirection, me.getVelocity()))
                        MovingEnemyToStopList.Add(me);
                }
                foreach (MovingEnemy me in MovingEnemyToStopList)
                {
                    me.SwitchState("stop");
                }
            }
        }
        else if (oppDirection != "" && Input.GetKeyUp(oppDirection.ToLower()))
        {
            Debug.Log("key up");
            foreach (MovingEnemy me in MovingEnemyList)
            {
                me.SwitchState("forward");
            }
            isAnyPressed = false;
        }
    }

    /*
        The correct enemy to set to pause for the direciton
        w largest -y
        a largest x 
        s largest y
        d largest -x
    //*/
    bool correctEnemy(string oppDirection, Vector2 velocity)
    {
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            // x absolutely larger than y
            if (velocity.x > 0)
            {
                if (oppDirection == "a")
                    return true;
                else
                    return false;
            }
            else if (velocity.x < 0)
            {
                if (oppDirection == "d")
                    return true;
                else
                    return false;
            }
            else
                throw new System.Exception("shouldn't be here");
        }
        else if (Mathf.Abs(velocity.x) < Mathf.Abs(velocity.y))
        {
            // y absolutely large than x
            if (velocity.y > 0)
            {
                if (oppDirection == "s")
                    return true;
                else
                    return false;
            }
            else if (velocity.y < 0)
            {
                if (oppDirection == "w")
                    return true;
                else
                    return false;
            }
            else
                throw new System.Exception("shouldn't be here");
        }
        else // both x and y are 0
            return false;
    }

    void FixedUpdate ()
    {
        


        if (currentTarget != (topIndexWay+1))
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
        Debug.Log("on trigger");
        
        if (other.GetComponent<WayPointScript>() != null)
        {
            Debug.Log("not null");
            if(currentTarget == other.GetComponent<WayPointScript>().wayIndex)
            {
                Debug.Log("found target");
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

    void OnGUI ()
    {
        GUI.Label(
            new Rect(10, 10, 200, 100),
            "Health :" + hs.getHealth().ToString()
            );
        if(dead == true)
        {
            GUI.Label(
            new Rect(210, 10, 200, 100),
            "You're a Loser"
            );
        }
    }

    public void die()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled=false;
        dead = true;
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name);
    }
}
