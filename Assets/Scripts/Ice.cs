/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to Sodacan Ice - plays its animation and particle effects upon detecting the sodacan
/// </summary>
public class Ice : MonoBehaviour
{

    #region PRIVATE_MEMBER_VARIABLES
    
    private bool m_didAnimate;
    private bool m_didFinishPlaying;
    
    #endregion PRIVATE_MEMBER_VARIABLES

    #region PUBLIC_MEMBER_VARIABLES

    public GameObject Iceberg;
    public ParticleSystem particlesSnow;

    public bool DidFinishAnimation
    {
        get
        {
            return m_didFinishPlaying;
        }
    }

    #endregion //PUBLIC_MEMBER_VARIABLES

    #region PUBLIC_METHODS
    
    public void Play()
    {
        if (!m_didAnimate)
        {
            StartCoroutine(DidFinishPlayingCoroutine());
            m_didAnimate = true;
        }
    }

    #endregion //PUBLIC_METHODS

    #region PRIVATE_METHODS

    private IEnumerator DidFinishPlayingCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        Iceberg.SetActive(true);

        particlesSnow.Play();

        yield return new WaitForSeconds(1.6f);
        m_didFinishPlaying = true;
        yield return new WaitForEndOfFrame();
    }

    #endregion //PRIVATE_METHODS
}
