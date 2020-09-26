using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movePlayers : MonoBehaviour
{
    CharacterController controller;
    public scr_playerTank playerTankScript;
    public float speed = 0.5f;
    public float rotationSpeed = 20.0f;
    public float neighbourDistance = 4.0f;
    public GameObject playerObj;
    public GameObject[] players;
    public float moveSpeedMultiplier = 2.0f;
    public Vector3 desiredMoveDirection;
    public bool isGrounded;
    private float verticalVel;
    private Vector3 moveVector;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyRules();
    }

    void ApplyRules()
    {
        
        GameObject[] gos;
        gos = playerTankScript.allPlayers;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        Vector3 goalPos = playerTankScript.movementGoalPos;


        float dist;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                //Debug.Log(dist);
                if (dist <= neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;
                    if (dist < 3.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                        Debug.Log(go.transform.position);
                        //Debug.Log(vAvoid);
                    }
                }
            }
        }

        vCentre = vCentre / groupSize + (goalPos - this.transform.position); //calc average centre of the group
        
        //print(goalpos);
        Vector3 direction = (vCentre + (vAvoid * 1.5f )) - transform.position; //get the direction the character needs to turn into
        direction.y = 0; //make sure no vertical allignment is taken into account
        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);
        }

        //transform.Translate(0, 0, Time.deltaTime * speed);
        //desiredMoveDirection = forward;
        //ground
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 2;
        }
        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector);
        controller.Move(direction * Time.deltaTime * moveSpeedMultiplier);
        //print(groupSize);
    }
}
