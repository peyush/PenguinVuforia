/*==============================================================================
Copyright (c) 2015 PTC Inc. All Rights Reserved.

Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/

using UnityEngine;
using System.Collections;
using Vuforia;

public enum HEADER_MESSAGE
{
    POINT_THE_DEVICE, PULLBACK_SLOWLY, TAP_ICE
}

/// <summary>
/// Displays the header title and buttons
/// </summary>
public class GUIInputManager
{
    #region PUBLIC_MEMBERS

    public event System.Action TappedOnDoneButton;
    public event System.Action TappedOnResetButton;

    #endregion //PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS

    private Texture2D[] m_headerTextures;
    private GUIStyle m_StyleHeader;
    private GUIStyle m_SodaCanOverlay;
    private GUIStyle m_buttonDone;
    private GUIStyle m_buttonReset;

    //to distinguish between single tap anywhere vs on GUI elements
    private static bool m_guiInput;

    #endregion //PRIVATE_MEMBERS

    #region PUBLIC

    public static bool SingleTappedOnScreen
    {
        get
        {
            bool tf = false;
            if (!m_guiInput && Input.GetMouseButtonUp(0))
            {
                tf = true;
            }
            else
            {
                m_guiInput = false;
            }
            return tf;
            
        }
    }

    public GUIInputManager()
    {
        m_StyleHeader = new GUIStyle();
        m_SodaCanOverlay = new GUIStyle();
        m_buttonDone = new GUIStyle();
        m_buttonReset = new GUIStyle();

        m_headerTextures = new Texture2D[3];
        //m_headerTextures[0] = Resources.Load("GUI/header_pointdevice") as Texture2D;
        //m_headerTextures[1] = Resources.Load("GUI/header_pullbackslowly") as Texture2D;
       // m_headerTextures[2] = Resources.Load("GUI/header_tapice") as Texture2D;

        m_headerTextures[0] = Resources.Load("GUI/point") as Texture2D;
        m_headerTextures[1] = Resources.Load("GUI/pullback") as Texture2D;
        m_headerTextures[2] = Resources.Load("GUI/tap") as Texture2D;

        //m_SodaCanOverlay.normal.background = Resources.Load("GUI/outline") as Texture2D;
        m_SodaCanOverlay.normal.background = Resources.Load("GUI/rectOutline") as Texture2D;
        m_buttonDone.normal.background = Resources.Load("GUI/buttonDone") as Texture2D;
        m_buttonDone.active.background = Resources.Load("GUI/buttonDone") as Texture2D;
        m_buttonDone.onActive.background = Resources.Load("GUI/buttonDone") as Texture2D;

        m_buttonReset.normal.background = Resources.Load("GUI/buttonReset") as Texture2D;
        m_buttonReset.active.background = Resources.Load("GUI/buttonReset") as Texture2D;
        m_buttonReset.onActive.background = Resources.Load("GUI/buttonReset") as Texture2D;
    }
    
    public void UpdateTitle(HEADER_MESSAGE message)
    {
        int index = (int) message;
        if (index >= 0 && index < m_headerTextures.Length)
        {
            m_StyleHeader.normal.background = m_headerTextures[index];

            float texWidth = m_StyleHeader.normal.background.width;
            float texHeight = m_StyleHeader.normal.background.height;

            float width = Screen.width/2;
            float height = (width / texWidth) * texHeight;

            GUI.Label(new Rect(Screen.width / 4, 0, width, height), "", m_StyleHeader);
        }
    }

    public void DrawDoneButton()
    {
        float texWidth = m_buttonDone.normal.background.width;
        float texHeight = m_buttonDone.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", m_buttonDone))
        {
            if (this.TappedOnDoneButton != null)
            {
                this.TappedOnDoneButton();
            }
            m_guiInput = true;
        }
    }

    public void DrawResetButton()
    {
        float texWidth = m_buttonReset.normal.background.width;
        float texHeight = m_buttonReset.normal.background.height;

        float width = texWidth * Screen.width / 1920;

        float height = (width / texWidth) * texHeight;
        float y = Screen.height - height;
        float x = Screen.width - width;
        if (GUI.Button(new Rect(x, y, width, height), "", m_buttonReset))
        {
            if (this.TappedOnDoneButton != null)
            {
                this.TappedOnResetButton();
            }
            m_guiInput = true;
        }
    }

    public void DrawCylinderOutline()
    {
        float texWidth = m_SodaCanOverlay.normal.background.width;
        float texHeight = m_SodaCanOverlay.normal.background.height;

        float width = Screen.width;
        float height = (Screen.width / texWidth) * texHeight;

        float y = (Screen.height * 0.5f) - height*0.5f;
        //GUI.Box(new Rect(0, y, width, height), "", m_SodaCanOverlay);
        //GUI.Box(new Rect(0, y, width, height), "",);
    }

    #endregion //PUBLIC

}
