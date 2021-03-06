﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class AnimationManager : MonoBehaviour {

    //The animator
    public Animator m_animator;

    //The string for use to connect and control mechanim specifically for the walking animation
    public string m_stIsWalking;

    //The string for use to connect and control mechanim specifically for the jumping animation
    public string m_stIsJumping;

    //The string for use to connect and control mechanim specifically for the swinging animation
    public string m_stIsSwinging;

    //The string for use to connect and control machanim specifically for the falling animation
    public string m_stIsFalling;
    
    //Reference to the player controller
    public Player m_plPlayer;
    
    //Check for player states and set appropriate bools to control the animations in mechanim
    void Update () {
        //If the player is on the ground
        if (m_plPlayer.IsGrounded)
        {
            if (!m_plPlayer.IsIdle)
            {
                //If the player is not idle play the walking animation
                m_animator.SetBool(m_stIsWalking, true);
            }
            else
            {
                //If the player is idle play the idle animation
                m_animator.SetBool(m_stIsWalking, false);
            }
            m_animator.SetBool(m_stIsSwinging, false);
            m_animator.SetBool(m_stIsJumping, false);
            m_animator.SetBool(m_stIsFalling, false);
        }
        //If the player is swinging with the grapple play the swing animation
        else if (m_plPlayer.IsGrappling)
        {
            m_animator.SetBool(m_stIsWalking, false);
            m_animator.SetBool(m_stIsSwinging, true);
            m_animator.SetBool(m_stIsJumping, false);
            m_animator.SetBool(m_stIsFalling, false);
        }
        //If the player is jumping play the jump animation
        else if (m_plPlayer.IsJumping)
        {
            m_animator.SetBool(m_stIsWalking, false);
            m_animator.SetBool(m_stIsSwinging, false);
            m_animator.SetBool(m_stIsJumping, true);
            m_animator.SetBool(m_stIsFalling, false);
        }
        //If the player is falling play the fall animation
        else if (m_plPlayer.IsFalling)
        {
            m_animator.SetBool(m_stIsWalking, false);
            m_animator.SetBool(m_stIsSwinging, false);
            m_animator.SetBool(m_stIsJumping, false);
            m_animator.SetBool(m_stIsFalling, true);
        }
    }
}
