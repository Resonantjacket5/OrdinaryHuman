﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    public int startIndexWay;
    public int topIndexWay;
    public float speed = 5f;
    HealthScript hs;
    bool isMoving = false;
    bool collided = false;
    Rigidbody2D rb2d;
    // list of waypoints to find
    List<WayPointScript> WayPointList;
    List<MovingEnemy> MovingEnemyList;

    int currentTarget = -1;
    bool dead = false;

	// Use this for initialization
	void Start () {
        
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        hs = gameObject.GetComponent<HealthScript>();

        // order waypoint list then sort, cast into waypoint list, and assign
        WayPointList = GameObject.FindObjectsOfType<WayPointScript>().ToList<WayPointScript>();        
        WayPointList = WayPointList.OrderBy(wp => wp.wayIndex).ToList<WayPointScript>();


        MovingEnemyList = GameObject.FindObjectsOfType<MovingEnemy>().ToList<MovingEnemy>();
        
        if(WayPointList.Count()>0)
        {
            currentTarget = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {

        // if not moving
        // then move to next nodez

        
        if (!isMoving && (currentTarget<topIndexWay+1))
        {
            collided = false;
            isMoving = true;
            StartCoroutine(moveTo(WayPointList[currentTarget].transform.position));
            //Debug.Log("start moving towards: " + WayPointList[currentTarget].transform.position);
        }
	}



    IEnumerator moveTo(Vector2 dest)
    {
        Vector2 start = transform.position;
        // time = distance / speed 
        float journeyTime = Vector2.Distance(start, dest) / speed;
        //float currentTime = 0;
        /*
        while (currentTime < journeyTime)
        {
            transform.position = Vector2.Lerp(start, dest,currentTime/journeyTime);
            journeyTime += Time.deltaTime;
            yield return null;
        }
        */
        
        // while not collided with waypoint trigger
        // continue to move towards the target
        while(!collided)
        {
            transform.position = Vector2.MoveTowards(transform.position, dest,Time.deltaTime * speed);
            yield return null;
        }

        // collided equal true, exit
        // set isMoving to false
        isMoving = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.GetComponent<WayPointScript>() != null)
        {
            if(currentTarget == other.GetComponent<WayPointScript>().wayIndex)
            {
                // set collided to true to exit Coroutine Movetowards
                collided = true;

                // set next target
                currentTarget++;
            }
        }
    }




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
    }
}
