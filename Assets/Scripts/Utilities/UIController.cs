﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XboxCtrlrInput;

public class UIController : MonoBehaviour {
    
    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 0;

    //
    // public Button m_buRstartButton;

    //
    // public EventSystem m_esEventSysRef;

    //
    public float m_fMenuXMax;

    //
    public float m_fMenuYMax;

    //
    public GameObject m_goPauseMenu;

    Vector2 m_vMenuPosition;


    bool m_bMenuActive;


    bool m_bCanInteract = true;


    float m_fTimer;

    void Awake()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        if (m_bMenuActive || m_goPauseMenu.activeInHierarchy)
        {
            m_bMenuActive = false;
            m_goPauseMenu.SetActive(false);
        }
    }
    public void Quit()
    //On call will close the game
    {
        Application.Quit();
    }

    public void LoadLevel()
    //On call will load a specified scene
    {
        SceneManager.LoadScene(m_iSceneIndex);
    }

    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Start))
        {
            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }

        if (m_bMenuActive)
        {
            if (m_bCanInteract)
            {

                if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0)
                {
                    m_bCanInteract = false;
                    //Change selected menu item
                    m_vMenuPosition.x--;
                }
                else if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0)
                {
                    m_bCanInteract = false;
                    //Change selected menu item
                    m_vMenuPosition.x++;
                }
                else if (XCI.GetAxisRaw(XboxAxis.LeftStickY) < 0)
                {
                    m_bCanInteract = false;
                    //Change selected menu item
                    if (m_vMenuPosition.y <= 0)
                        m_vMenuPosition.y = m_fMenuYMax;
                    else
                        m_vMenuPosition.y--;
                }
                else if (XCI.GetAxisRaw(XboxAxis.LeftStickY) > 0)
                {
                    m_bCanInteract = false;
                    //Change selected menu item
                    m_vMenuPosition.y++;
                }
            }
            else
            {
                if (m_fTimer <= 0)
                {
                    m_bCanInteract = true;
                    m_fTimer = 1;
                }
                else
                {
                    m_fTimer -= Time.unscaledDeltaTime;
                }
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        m_goPauseMenu.SetActive(true);
    }

    void Unpause()
    {
        Time.timeScale = 1;
        m_goPauseMenu.SetActive(false);
    }
}
