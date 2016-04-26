/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections;

/// <summary>
/// This component is attached to every iceberg that is cloned for each prop after scanning phase
/// It merely manupulates the shader to give a ice-growing effect
/// </summary>
public class Iceberg : MonoBehaviour
{
    #region PUBLIC_MEMBERS

    public float Speed = 1f;
    public float OffsetTime = 0f;
    public float MaxTime = 3f;
    public GameObject m_maskObject;

    #endregion //PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS
    
    private float _startTime;
    private Material _meshMaterial;

    #endregion //PRIVATE_MEMBERS

    #region UNITY_MONOBEHAVIOUR

    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        _meshMaterial = GetComponent<Renderer>().material;
        ResetStartTime();
        StartCoroutine(RevealChildAfterSecs(0.7f));
    }

    void Update()
    {
        float delta = (Time.time - _startTime) * Speed;
        if (MaxTime != 0f)
        {
            delta = Mathf.Min(delta, MaxTime);
        }
        _meshMaterial.SetFloat("_DeltaTime", delta);
    }

    #endregion //UNITY_MONOBEHAVIOUR

    #region PRIVATE_METHODS
    
    private void ResetStartTime()
    {
        GetComponent<Renderer>().enabled = true;
        _startTime = Time.time + OffsetTime;
    }

    private IEnumerator RevealChildAfterSecs(float secs)
    {
        if (m_maskObject != null)
        {
            m_maskObject.SetActive(false);

            yield return new WaitForSeconds(secs);

            m_maskObject.SetActive(true);
            m_maskObject.GetComponent<Renderer>().enabled = true;
        }

        yield return null;
    }

    #endregion //PRIVATE_METHODS
}
