/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
/// Slightly different implementation than the DefaultTrackableEventHandler class:
/// In addition to its children, we turn on/off components of CylinderTrackable and its children here. 
/// </summary>
public class SmartTerrainTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PUBLIC_MEMBERS

    //a way for the StateManager to know if the SmartTerrainTrackable was lost or found most recently
    //Accordingly, show/hide the surface based on what state the app is in.
    public bool m_trackablesFound;

    #endregion //PUBLIC_MEMBERS
    
    #region PRIVATE_MEMBER_VARIABLES

    private CylinderTargetAbstractBehaviour m_CylinderTarget;
    private ImageTargetAbstractBehaviour m_ImageTarget;
    private TrackableBehaviour mTrackableBehaviour;

    //only required to hide the surface mesh the first time it's detected
    private bool m_TrackableDetectedForFirstTime = true;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS
    
    void Start()
    {
        m_CylinderTarget = FindObjectOfType(typeof(CylinderTargetAbstractBehaviour)) as CylinderTargetAbstractBehaviour;
        m_ImageTarget = FindObjectOfType(typeof(ImageTargetAbstractBehaviour)) as ImageTargetAbstractBehaviour;
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
		WireframeBehaviour[] wireframeComponents = GetComponentsInChildren<WireframeBehaviour>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;

            //We don't want to show surface before the soda can animation is done playing.
            if (m_TrackableDetectedForFirstTime)
            {
                if (component.name == "Primary Surface")
                {
                    component.enabled = false;
                }
                m_TrackableDetectedForFirstTime = false;
                
            }
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
		
		// Enable wireframe rendering:
        foreach (WireframeBehaviour component in wireframeComponents)
        {
            component.enabled = true;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

        //we want to show the soda can and iceberg at all times even when cylinder trackable is lost. The following makes sure they show
        //as long as smart terrain is tracking
        if (m_CylinderTarget != null)
        {
            Renderer[] rendererComponentsOfCylinder = m_CylinderTarget.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponentsOfCylinder)
            {
                component.enabled = true;
            }
        }

        if (m_ImageTarget != null)
        {
            Renderer[] rendererComponentsOfImage = m_ImageTarget.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponentsOfImage)
            {
                component.enabled = true;
            }
        }

        m_trackablesFound = true;
    }


    private void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
		WireframeBehaviour[] wireframeComponents = GetComponentsInChildren<WireframeBehaviour>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }
		
		// Disable wireframe rendering:
        foreach (WireframeBehaviour component in wireframeComponents)
        {
            component.enabled = false;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

        //hide the soda can and iceberg only when smart terrain tracking is lost.

        if (m_CylinderTarget != null)
        {
            Renderer[] rendererComponentsOfCylinder = m_CylinderTarget.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponentsOfCylinder)
            {
                component.enabled = false;
            }
        }


        //hide the soda can and iceberg only when smart terrain tracking is lost.

        if (m_ImageTarget != null)
        {
            Renderer[] rendererComponentsOfImage = m_ImageTarget.gameObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer component in rendererComponentsOfImage)
            {
                component.enabled = false;
            }
        }
        m_trackablesFound = false;
    }

    #endregion // PRIVATE_METHODS
}
