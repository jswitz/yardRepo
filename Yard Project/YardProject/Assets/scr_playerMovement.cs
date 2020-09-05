using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerMovement : MonoBehaviour
{

    public Camera cam;

    private float moveSpeed_now, moveSpeed_normal = 180.0f;
    private string moveAxis_x, moveAxis_z;
    public bool isWalking = false;
    private Vector3 characterVelocity;

    //public Rigidbody rb;
  

    private void Start()
    {
        AssignControls();
        //cam = Camera.main;
        moveSpeed_now = moveSpeed_normal;
    }

    private void FixedUpdate()
    {
        CharacterMovement();
        //Debug.Log(characterVelocity);
    }

    private void CharacterMovement()
    {

        float xVelocity = Input.GetAxisRaw(moveAxis_x) * moveSpeed_now * Time.deltaTime;
        Vector3 xVector = cam.transform.right * xVelocity;
        float zVelocity = Input.GetAxisRaw(moveAxis_z) * moveSpeed_normal * Time.deltaTime;
        Vector3 zVector = cam.transform.forward * zVelocity;

        characterVelocity = xVector + zVector;

        //_vector.y = rb.velocity.y;


        if ((Mathf.Abs(xVelocity) > 1.0f || Mathf.Abs(zVelocity) > 1.0f))// && anim.GetCurrentAnimatorStateInfo(0).IsTag("normal"))//&& !playerScript.isInMenu)
        {
            isWalking = true;
            transform.Translate(characterVelocity * Time.deltaTime, Space.World); //WORKS
            //transform.Translate(characterVelocity * Time.deltaTime, cam.position); DOESNT WORK
            //rb.velocity = characterVelocity;

            //character rotation
            Quaternion _rotationToGo = Quaternion.LookRotation(characterVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotationToGo, 0.3f);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        else
        {
            isWalking = false;
            characterVelocity = new Vector3(0, characterVelocity.y, 0);
            transform.Translate(characterVelocity * Time.deltaTime, Space.World);
            //rb.velocity = characterVelocity;
        }
    }

        private void AssignControls()
    {
        moveAxis_x = "MoveAxis X KM";
        moveAxis_z = "MoveAxis Z KM";
    }
    
}
