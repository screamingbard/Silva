﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XboxCtrlrInput;

public class MainMenuController : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 0;

    //The Resume Button
    public Button m_btnPlayButton;

    //The credits button
    public Button m_btnCreditsButton;

    //The settings button
    public Button m_btnSettingsButton;

    //The quit button
    public Button m_btnQuitButton;

    //An event system
    public EventSystem m_esEventSysRef;

    //The main menu game object
    public GameObject m_goMainMenu;

    //The settings menu game object
    public GameObject m_goSettingsMenu;
    
    void Awake()
    {
        //On awake make sure the game isn't paused
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        if (m_goMainMenu.activeInHierarchy)
        {
            m_goMainMenu.SetActive(false);
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
        if (XCI.GetButtonDown(XboxButton.Start) || Input.GetKeyDown(KeyCode.Escape))
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
    }

    //Go into the settings menu
    public void GoToSettings()
    {
        m_goSettingsMenu.SetActive(true);
        m_goMainMenu.SetActive(false);
    }

    //Go back to the pause menu from the settings
    public void BackOutOfSettings()
    {
        m_goSettingsMenu.SetActive(false);
        m_goMainMenu.SetActive(true);
    }

    //Pause the game
    public void Pause()
    {
        Time.timeScale = 0;
        m_goMainMenu.SetActive(true);
    }

    //Unpauses the game
    public void Unpause()
    {
        Time.timeScale = 1;
        m_goMainMenu.SetActive(false);
    }
}
