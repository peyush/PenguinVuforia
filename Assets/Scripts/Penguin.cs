/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;
/// <summary>
/// Manages movement and rotation of the character (Penguin) around the scene
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Penguin : MonoBehaviour
{

    #region PUBLIC_MEMBERS
    public float maxHealth = 100f;
    public float curHealth;
    public float m_movementSpeed = 3.0f;
    public Text textBoxScore;
    public static int score;
    public AudioClip collectClip;                          // An audio clip to play when a shot collected.
    public AudioClip deathPeng;
    public bool DidAppear
    {
        get
        {
            return m_appeared;
        }
    }
    #endregion //PUBLIC_MEMBERS

    #region PRIVATE_MEMBES

    private bool m_appeared;
    private bool m_interactive;
    private CharacterController m_characterController;
    private Vector3 m_movementTarget;
    private const float MIN_MOVEMENT_DISTANCE = 0.05f;
    private bool m_CharacterCurrentlyMoving = false;
    private Vector3 m_lookRotationPoint;
    private Quaternion m_rotation;
    private bool m_moveBegin;
    private Vector3 m_lookRotationDir;
    public Text deathTextBox;
    #endregion //PRIVATE_MEMBERS

    #region UNITY_MONOBEHAVIOUR

    //For simplicity in this sample app, we set it to inactive in the scene hierarchy and keep the animation to 'play-on-awake'
    //On the appropriate phase in the scene, we simply make it active, so that the animation plays automatically.
    void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_appeared = true;
        curHealth = maxHealth;
        deathTextBox.enabled = false;
    }

    void Update()
    {
        if (GUIInputManager.SingleTappedOnScreen)
        {
            HandleSingleTap();
        }

        if (m_appeared && m_interactive)
        {
            Move();
        }
    }

    #endregion  //UNITY_MONOBEHAVIOUR

    #region PRIVATE_METHODS

    private void RotateUpdate()
    {
        m_rotation = Quaternion.LookRotation(m_lookRotationDir.normalized);
        m_rotation = Quaternion.Euler(0, m_rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_rotation, Time.deltaTime * 8);
    }

    private void HandleSingleTap()
    {
        GameObject go = VuforiaManager.Instance.ARCameraTransform.gameObject;
        Camera[] cam = go.GetComponentsInChildren<Camera>();
        Ray ray = cam[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            // move the character towards the new target
            m_movementTarget = hitInfo.point;
            m_lookRotationDir = hitInfo.point - transform.position;
        }

        if (m_appeared)
        {
            m_interactive = true;
        }

    }

    private void Move()
    {
        // calculate the (2D) distance of the character from its target
        Vector2 currentVectorToTarget =
            (new Vector2(m_movementTarget.x, m_movementTarget.z) -
             new Vector2(transform.position.x, transform.position.z));

        // determine if character should start moving
        if (!m_CharacterCurrentlyMoving && currentVectorToTarget.magnitude >= MIN_MOVEMENT_DISTANCE)
        {
            m_CharacterCurrentlyMoving = true;
        }

        // determine if character should stop moving
        if (m_CharacterCurrentlyMoving && currentVectorToTarget.magnitude < MIN_MOVEMENT_DISTANCE)
        {
            m_CharacterCurrentlyMoving = false;
        }

        // move character
        if (m_CharacterCurrentlyMoving)
        {
            Vector2 movementVector = currentVectorToTarget.normalized * m_movementSpeed * Time.deltaTime;
            Vector3 dir = new Vector3(movementVector.x, 0, movementVector.y);
            m_characterController.Move(dir);
            RotateUpdate();
        }

        transform.localPosition = new Vector3(transform.localPosition.x, 0.25f, transform.localPosition.z);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            score++;
            textBoxScore.text = "Score: " + score;
            AudioSource.PlayClipAtPoint(collectClip, other.gameObject.transform.position);
        }
    }

    public void takedamage(float damage)
    {
        curHealth = curHealth - damage;

        if (curHealth == 0)
        {
            //Let penguin die
            Destroy(this);
            deathTextBox.enabled = true;
            AudioSource.PlayClipAtPoint(deathPeng, transform.position);
        }
    }
    #endregion //PRIVATE_METHODS

}
