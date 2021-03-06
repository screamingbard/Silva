﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class PlayerRespawn : MonoBehaviour {

    //Vision cone tag
    public string m_stVisionConeTag;
    //Death trap tag
    public string m_stDeathTrapTag;
    //New respawn point tag
    public string m_stNewRespawn;
    //The respawning point of the player
    public Transform m_tfRespawnPoint;
    //The time before you can move
    public float m_fSetMovementTimer = 0.1f;
    //The time before you can move
    [HideInInspector]
    public float m_fMovementTimer;
    //The Particles that play where the player respawns when they respawn
    public ParticleSystem m_psSpawnParticles;
    //The Particles that play where the player dies when they die
    public ParticleSystem m_psDeathParticles;
    //Reference to the camera
    public GameObject m_goStoreCam;
    //Reference to the gameobject controlling the death particles
    public GameObject m_goDeathParticles;

    private GameObject spawnedParticle;
    void Start()
    {
        spawnedParticle = Instantiate(m_goDeathParticles, transform);
        spawnedParticle.SetActive(false);
    }

    void Update()
    {
        if (spawnedParticle.activeInHierarchy)
        {
            if (!spawnedParticle.GetComponent<ParticleSystem>().isPlaying)
            {
                spawnedParticle.SetActive(false);
            }
        }
    }

    public void Respawn(){
        //Play the death particles
        if (m_psDeathParticles != null)
        {
            spawnedParticle.SetActive(true);
            spawnedParticle.transform.position = transform.position;
        }
        //Set the players velocity to zero
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //Stop grappling
        gameObject.GetComponent<Player>().OnGrappleUp();

        //start the timer to disallow movement for a split second
        m_fMovementTimer = m_fSetMovementTimer;

        //Set the players position to the respawn points
        gameObject.transform.SetPositionAndRotation(m_tfRespawnPoint.position, m_tfRespawnPoint.rotation);

        //Play the respawn animation


        //Play the respawn particles
        if (m_psSpawnParticles != null)
            m_psSpawnParticles.Play();

        //Reset the camera position
        //Camera.main.GetComponent<CameraFollow>().ResetCamera();
        m_goStoreCam.GetComponent<CameraFollow>().ResetCamera();
    }

    void OnCollisionEnter2D(Collision2D a_col2DCollider){
        //If the player enters the collider of an object DeathTrap
        if (a_col2DCollider.gameObject.tag == m_stDeathTrapTag){
            //Call the respawn funtion
            Respawn();
        }
    }
    void OnTriggerEnter2D(Collider2D a_trTrigger2D){
        //If the player enters the trigger zone of a new respawn point
        if (a_trTrigger2D.gameObject.tag == m_stNewRespawn){
			//set the new respawn point
			m_tfRespawnPoint = a_trTrigger2D.gameObject.transform;
        }
        //If the player enters the collider of an object DeathTrap
        else if (a_trTrigger2D.gameObject.tag == m_stDeathTrapTag)
        {
            //Call the respawn funtion
            Respawn();
        }
    }
}
