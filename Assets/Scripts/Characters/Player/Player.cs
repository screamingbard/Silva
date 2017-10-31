﻿using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(DistanceJoint2D))]

public class Player : MonoBehaviour {

    public float jumpHeight = 4.0f;
    public float timeToJump = 0.4f;
    float accelerationTimeAirbourne = 0.2f;
    float acceleratinTimeGrounded = 0.1f;
    public float moveSpeed = 6;
    float defaultSpeed;

    //[HideInInspector]
    public bool isDead = false;

    float jumpVelocity;
    float gravity;
    [HideInInspector]
    public Vector3 velocity;
    float XSmoothing;

    public float drag;
    public float playerdirection;
    bool isturned;

    [HideInInspector]
    public Vector2 input;
    [HideInInspector]
    public bool CanJump = false;

    public float fPlayerMaxSpeed = 30.0f;
    [Tooltip("This will increase the speed that the player accelerates at as they swing")]
    public float inAirModifier = 20.0f;
    [Tooltip("This effects the distance the player can swing")]
    public float MaxInAirSpeed = 100.0f;

    float colcd = 0;

    float grapAngle = 1;

    float targetVelocityX = 0;

    bool goingback = false;

    int CurrentDir = 0;

    Controller2D controller;

    ShootOBJ shootOBJ;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJump;

        CanJump = false;

        defaultSpeed = moveSpeed;

        shootOBJ = GetComponentInChildren<ShootOBJ>();
    }

    void Update()
    {
        bool bGrappling = shootOBJ.cBall && shootOBJ.cBall.GetComponent<Grapple>().GrapConnected == true;

        //if (controller.collisions.left && bGrappling && colcd == 0|| controller.collisions.right && bGrappling && colcd == 0)
        //{
        //    float storevel = velocity.x;
        //    velocity.x = -storevel;
        //    colcd += Time.deltaTime;
        //}

        if (colcd >= 1)
        {
            colcd = 0;
        }

        if (shootOBJ.cBall != null)
            grapAngle = Quaternion.Angle(transform.rotation, shootOBJ.cBall.transform.rotation);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            gameObject.GetComponent<PlayerInput>().canInput = true;
        }

        if (CanJump == true && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < -50)
            velocity.y = -50;

        if (controller.collisions.below && !bGrappling)
        {
            moveSpeed = defaultSpeed;

            targetVelocityX = input.x * moveSpeed;
        }
        else if(bGrappling)
        {
            targetVelocityX += (input.x + grapAngle) * moveSpeed * Time.deltaTime * inAirModifier;
        }
        else if (bGrappling && controller.collisions.below)
        {
            targetVelocityX = input.x * moveSpeed;
        }
        else
        {
            targetVelocityX = input.x * moveSpeed;
        }

        //--------------------
        //In air Max Speed
        //--------------------
        if (controller.collisions.below || controller.collisions.above)
        {
            if (velocity.x > MaxInAirSpeed)
                velocity.x = MaxInAirSpeed;
            if (velocity.y > MaxInAirSpeed)
                velocity.y = MaxInAirSpeed;

            if (velocity.x < -MaxInAirSpeed)
                velocity.x = -MaxInAirSpeed;
            if (velocity.y < -MaxInAirSpeed)
                velocity.y = -MaxInAirSpeed;
        }

        //--------------------
        //On Land Max Speed
        //--------------------
        if (controller.collisions.below == false || controller.collisions.above == false)
        {
            if (velocity.x > fPlayerMaxSpeed)
                velocity.x = fPlayerMaxSpeed;
            if (velocity.y > fPlayerMaxSpeed)
                velocity.y = fPlayerMaxSpeed;

            if (velocity.x < -fPlayerMaxSpeed)
                velocity.x = -fPlayerMaxSpeed;
            if (velocity.y < -fPlayerMaxSpeed)
                velocity.y = -fPlayerMaxSpeed;
        }

        if (bGrappling && !gameObject.GetComponent<PlayerInput>().isMoving)
        {
            if (Mathf.Sign(targetVelocityX) < 0)
            {
                targetVelocityX += (moveSpeed + grapAngle) * Time.deltaTime * 10.0f;
                goingback = true;
            }
            else
            {
                targetVelocityX -= (moveSpeed + grapAngle) * Time.deltaTime * 10.0f;
                goingback = false;
            }
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);

        Debug.Log(velocity.x);
        if (!bGrappling)
        {
            if (velocity.x > MaxInAirSpeed)
                velocity.x = MaxInAirSpeed;
            if (velocity.y > MaxInAirSpeed)
                velocity.y = MaxInAirSpeed;

            if (velocity.x < -MaxInAirSpeed)
                velocity.x = -MaxInAirSpeed;
            if (velocity.y < -MaxInAirSpeed)
                velocity.y = -MaxInAirSpeed;
        }

        if (gameObject.GetComponent<PlayerInput>().justReleased)
        {
            if (velocity.x > fPlayerMaxSpeed)
            {
                velocity.x = calcSpeedOutOfJump();
            }
            if (velocity.x < -fPlayerMaxSpeed)
            {
                velocity.x = -calcSpeedOutOfJump();
            }
        }

        controller.Move(velocity * Time.deltaTime);

        //playerdirection = Mathf.Sign(velocity.x);

        //if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0) //&& !isturned)
        //{
        //    transform.Rotate(0, 180, 0);
        //    isturned = true;
        //    CurrentDir = (int)input.x;
        //}

        //if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 && isturned)
        //{
        //    transform.Rotate(0, 180, 0);
        //    isturned = false;
        //    CurrentDir = (int)input.x;
        //}

        //transform.right = new Vector3(CurrentDir, 0, 0);

        controller.HorizontalDeathCollision(ref velocity);
        controller.VerticalDeathCollision(ref velocity);
        //DEBUGGING
        isDead = controller.collisions.IsDying;
    }
    float calcmaxspeed()
    {
        float storeNum = Mathf.Pow(fPlayerMaxSpeed, 2) / 2;
        float res = Mathf.Sqrt(storeNum);
        return res;
    }

    float calcSpeedOutOfJump()
    {
        float speed = 0;
        speed = -(velocity.x / velocity.y) * 10;


        return speed;
    }
}

