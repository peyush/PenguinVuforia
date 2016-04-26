/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using Vuforia;
using UnityEngine.UI;

/// <summary>
/// Slightly different implementation than the DefaultTrackableEventHandler class in two ways:
/// 1. We turn off all its children components only when SmartTerrain Trackable is lost - not here
/// 2. We reset everything to identity (position and rotation) when this trackable is lost as a 'pose correction' mechanism
/// </summary>
public class ImageTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PUBLIC_MEMBERS

    public event System.Action ImageTrackableFoundFirstTime;
    public Text textbox;
    #endregion //PUBLIC_METHODS


    #region PRIVATE_MEMBER_VARIABLES

    private TrackableBehaviour mTrackableBehaviour;
    private bool m_TrackableDetectedForFirstTime;

    #endregion // PRIVATE_MEMBER_VARIABLES



    #region UNTIY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS


    private void OnTrackingFound()
    {
       
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
        textbox.text = "tracking found";
        //for UIStateManager to know when to play iceberg animation around soda can
        if (!m_TrackableDetectedForFirstTime)
        {
            if (this.ImageTrackableFoundFirstTime != null)
            {
                this.ImageTrackableFoundFirstTime();
                Debug.Log("First Time Trackable Found at [" + Time.time + "]");
                textbox.text = Time.time.ToString();
            }
            else
            {
                textbox.text = "null";
            }
            m_TrackableDetectedForFirstTime = true;
        }
        

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        //textbox.text = "Trackable";
    }

    //On TrackingLost, we reset the position and rotation of the soda can. 
    //This corrects any pose shifting that may have taken place
    private void OnTrackingLost()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        //textbox.text =  mTrackableBehaviour.TrackableName ;
    }

    #endregion // PRIVATE_METHODS
}
