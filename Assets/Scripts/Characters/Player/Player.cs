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

    float targetVelocityX = 0;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJump;

        CanJump = false;

        defaultSpeed = moveSpeed;
    }

    void Update()
    {
        Debug.Log(velocity.x + " ," + velocity.y);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (CanJump == true && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        
        velocity.y += gravity * Time.deltaTime;

        if (controller.collisions.below && gameObject.GetComponentInChildren<ShootOBJ>().grappleObj.GetComponent<Grapple>().GrapConnected == false)
        {
            targetVelocityX = input.x * moveSpeed;

            moveSpeed = defaultSpeed;

            if (velocity.x > fPlayerMaxSpeed)
                velocity.x = fPlayerMaxSpeed;
            if (velocity.y > fPlayerMaxSpeed)
                velocity.y = fPlayerMaxSpeed;

            if (velocity.x < -fPlayerMaxSpeed)
                velocity.x = -fPlayerMaxSpeed;
            if (velocity.y < -fPlayerMaxSpeed)
                velocity.y = -fPlayerMaxSpeed;
        }
        else if(gameObject.GetComponentInChildren<ShootOBJ>().grappleObj.GetComponent<Grapple>().GrapConnected == true)
        {
            targetVelocityX += input.x * moveSpeed * Time.deltaTime;
        }
        else
        {
            targetVelocityX = input.x * moveSpeed;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);

        controller.Move(velocity * Time.deltaTime);

        //playerdirection = Mathf.Sign(velocity.x);

        //if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0 && !isturned)
        //{
        //    transform.Rotate(0, 180, 0);
        //    isturned = true;
        //}

        //if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 && !isturned)
        //{
        //    transform.Rotate(0, 180, 0);
        //    isturned = false;
        //}

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
}
