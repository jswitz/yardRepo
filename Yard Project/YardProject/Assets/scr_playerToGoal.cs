using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerToGoal : MonoBehaviour
{

    public scr_playerTank myPlayerTank;
    public float speed = 0.1f;
    public float rotationSpeed = 4.0f; //speed at which players turn
    public float minSpeed = 0.5f;
    public float maxSpeed = 1.0f;
    public float maxBoostSpeed = 2.0f;
    public float neighbourDistance = 5.0f; //how close players have to be to flock, smaller number means more explusive flocks, this will be kept big, to ensure always flocking for players
    float targetSpeed = 1.0f;
    public float accelRate = 2.0f;
    bool turning = false;
    public bool isBoosting = false;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        targetSpeed = maxSpeed;
    }

    void Update()
    {


        Bounds b = new Bounds(myPlayerTank.transform.position, myPlayerTank.movementLimits * 2); //determine limits of box without the raised Y axis

        if (!b.Contains(transform.position)) //if player is not within the box, do this
        {
            turning = true;
        }
        else
        {
            turning = false;
            ApplyRules();

        }

        if (turning) //calc direction back towards the centre of the tank and rotate towards it
        {
            Vector3 direction = myPlayerTank.transform.position - transform.position; //calc direction
            transform.rotation = Quaternion.Slerp(transform.rotation, //rotate player
                                                  Quaternion.LookRotation(direction),
                                                  Time.deltaTime * rotationSpeed);
            speed = Random.Range(minSpeed, maxSpeed); //with new random speed
            ApplyRules();
            /*if (isBoosting == true)
            {
                isBoosting = false; //turn of boost is is boosting
            }
            else
            {
                
                /*if(Random.Range( 0, 5) < 1) //else apply flocking rules 1/5 times
                {
                    
                }
            }*/
        }

        //Randomly change the speed
        /*if(Random.Range( 0, 10) < 3 && isBoosting == false) //3/10times, and not boosting
        {
            if(speed < targetSpeed) //and speed is less than targetspeed (1)
            {
                speed = maxBoostSpeed - speed;
                speed += accelRate * Time.deltaTime;
                isBoosting = true;
            }
        }

        if(speed >= maxBoostSpeed && isBoosting == true) //stop boosting if speed is above max speed
        {
            speed = Random.Range(minSpeed, maxSpeed);
            isBoosting = false;
        }*/

        transform.Translate(0, 0, speed * Time.deltaTime); //moves the players by speed
    }

    void ApplyRules()
    {
        GameObject[] gos; //gos = gameobjects plural
        gos = scr_playerTank.allPlayers; //grab the player objects from the script

        Vector3 vCentre = Vector3.zero; //calc centre of the group
        Vector3 vAvoid = Vector3.zero; //avoid hitting other players near the centre point
        float groupSpeed = 0.1f; //set group speed

        Vector3 goalPos = scr_playerTank.goalPos; //grab the goal position

        float dist;

        int groupSize = 0; //calc groupsize based upon whose within neighbour distance (should be 2 for both players)
        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neighbourDistance)
                {
                    vCentre += go.transform.position; //add all the centres
                    groupSize++; //add all the groupmembers

                    if(dist < 5.0f) //if member was about to collide
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position); //calc a vector thats off in another direction
                        Debug.Log("avoided");
                    }

                    scr_playerToGoal anotherFlock = go.GetComponent<scr_playerToGoal>(); //get the speeds from neighbours
                    groupSpeed = groupSpeed + anotherFlock.speed; //get average speed of the flock
                }
            }
        }

        if (groupSize > 0)
        {
            vCentre = vCentre / groupSize + (goalPos - this.transform.position); // calc average centre of the group
            speed = groupSpeed / groupSize; //calc average speed of the group

            Vector3 direction = (vCentre + vAvoid) - transform.position; //get direction player needs to turn into
            if(direction != Vector3.zero) //if direction !=0, change direction
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, //use this rotation slerp at rotation speed
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
            }
        }
        //print(groupSize);
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
    }
}
