﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : AbstractBehaviour
{
	public float jumpSpeed = 200f;
	public float jumpDelay = 0.1f;
	public int jumpCount = 2;
	public GameObject dustEffectPrefab;

	protected float lastJumpTime = 0f;
	protected int jumpsRemaining = 0;
     


    protected virtual void Update () 
	{
		var canJump = inputState.GetButtonValue(inputButtons[0]);
		var holdTime = inputState.GetButtonHoldTime(inputButtons[0]);

        if(collisionState.standing)
        {
            if(canJump && holdTime < 0.1f)
            {
                jumpsRemaining = jumpCount - 1;
                OnJump();
            }
        }
        else
        {
            if(canJump && holdTime < 0.1f && Time.time - lastJumpTime > jumpDelay)
            {
                if(jumpsRemaining > 0)
                {
                    OnJump();
                    jumpsRemaining--;
                    var clone = Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
                }
            }
        }
	}

	protected virtual void OnJump()
	{
		var vel = body2D.velocity;
		lastJumpTime = Time.time;
		body2D.velocity = new Vector2(vel.x, jumpSpeed);
	}

}
