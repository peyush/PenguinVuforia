/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections;

/// <summary>
/// Displays Splash for a few seconds before entering the main scene
/// </summary>
public class SplashManager : MonoBehaviour
{
    #region PRIVATE_MEMBERS

    private GUIStyle m_SplashStyle;
    private Texture2D m_SplashImage;
    private float textureWidth;
    private float textureHeight;

    #endregion //PRIVATE_MEMBERS
    
    #region UNITY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        //Smart Terrain is best experienced in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        m_SplashStyle = new GUIStyle();

#if (UNITY_IPHONE || UNITY_IOS)
       m_SplashStyle.normal.background = Resources.Load("SplashScreen/Splash_landscape_iPad") as Texture2D;
#else
       m_SplashStyle.normal.background = Resources.Load("SplashScreen/Splash_landscape") as Texture2D;
       
#endif

        textureWidth = m_SplashStyle.normal.background.width;
        textureHeight = m_SplashStyle.normal.background.height;

        StartCoroutine(LoadPenguin(5));
    }

    void OnGUI()
    {
        float width = Screen.width;
        float height = (width * textureHeight) / textureWidth ;
        float y = (Screen.height - height) / 2;
        GUI.Box(new Rect(0, y, width, height), "", m_SplashStyle);

    }

    private IEnumerator LoadPenguin(float secs)
    {
        yield return new WaitForSeconds(secs);

        Application.LoadLevel("1-Loading");
    }
    #endregion UNITY_MONOBEHAVIOUR_METHODS
}
