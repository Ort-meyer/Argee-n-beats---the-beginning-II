﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Controller : MonoBehaviour {

    public static Rigidbody m_rigidBodyController;
    public static TP_Controller Instance;

    public float m_groundedDelay = 0.5f;
    private float m_timeSinceGrounded = 0f;
    public float m_jumpCoolDown = 1.0f;
    private float m_jumpTimer = -0.1f;
    AudioSource audioSourceWalkSound;
    public AudioClip m_walkSound;
    // Use this for initialization
    public AudioClip m_jumpSound;
	// Use this for initialization
	void Awake () {
        m_rigidBodyController = GetComponent<Rigidbody>();
        Instance = this;
        TP_Camera.UseExistingOrCreateNewMainCamera();
        audioSourceWalkSound = gameObject.AddComponent<AudioSource>();
        audioSourceWalkSound.clip = m_walkSound;
        audioSourceWalkSound.volume = 0.4f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }
	
	// Update is called once per frame
	void Update () {
        if (Camera.main == null)
        {
            return;
        }
        GetLocomotionInput();
        HandleActionInput();
        UpdateCooldowns();
	}
    private void FixedUpdate()
    {
        TP_Motor.m_instance.UpdateMotor();
    }
    void UpdateCooldowns()
    {
        if (m_jumpTimer > 0f)
        {
            m_jumpTimer -= Time.deltaTime;
        }
        UpdateGrounded();
    }
    void GetLocomotionInput()
    {
        float deadZone = 0.1f;

        //TP_Motor.m_instance.m_verticalVel = TP_Motor.m_instance.m_moveVector.y;
        TP_Motor.m_instance.m_dashDirection = TP_Motor.m_instance.m_moveVector.normalized;
        TP_Motor.m_instance.m_moveVector = Vector3.zero;
        TP_Motor.m_instance.m_verticalVel = 0;
        float t_vert = Input.GetAxis("Vertical");
        float t_hori = Input.GetAxis("Horizontal");
        if (TP_Motor.m_instance.m_isDashing)
        {
            TP_Motor.m_instance.m_moveVector = TP_Motor.m_instance.m_dashDirection;
        }
        else
        {
            if (t_vert > deadZone || t_vert < -deadZone)
            {
                TP_Motor.m_instance.m_moveVector += new Vector3(0, 0, t_vert);
                if (IsGrounded())
                {
                    if (!audioSourceWalkSound.isPlaying)
                    {
                        audioSourceWalkSound.clip = m_walkSound;
                        audioSourceWalkSound.volume = 0.4f;
                        audioSourceWalkSound.Play();
                    }
                }
            }
            if (t_hori > deadZone || t_hori < -deadZone)
            {
                TP_Motor.m_instance.m_moveVector += new Vector3(t_hori, 0, 0);
                if (IsGrounded())
                {
                    if (!audioSourceWalkSound.isPlaying)
                    {
                        audioSourceWalkSound.clip = m_walkSound;
                        audioSourceWalkSound.volume = 0.4f;
                        audioSourceWalkSound.Play();
                    }
                }
            }
        }
        TP_Animator.m_instance.DetermineCurrentMoveDirection();
    }
    void HandleActionInput()
    {
       if (Input.GetButton("Jump") && m_timeSinceGrounded > 0)
       {
            if (m_jumpTimer < 0f)
            {
                Jump();
                m_timeSinceGrounded = 0f;
                m_jumpTimer = m_jumpCoolDown;

                audioSourceWalkSound.Stop();
                audioSourceWalkSound.clip = m_jumpSound;
                audioSourceWalkSound.volume = 0.2f;
                audioSourceWalkSound.Play();
            }
        }
        if (Input.GetButton("Dash"))
        {
            audioSourceWalkSound.clip = m_walkSound;
            audioSourceWalkSound.volume = 0.4f;
            audioSourceWalkSound.Play();
            Dash();
        }
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButton(1))
            {
                ShootArrow();
            }
        }
        if (Input.GetKeyUp("y"))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Cursor.visible = !Cursor.visible;
        }
    }

    private void UpdateGrounded()
    {
        bool r_hit = IsGrounded();
        if (r_hit)
        {
            m_timeSinceGrounded = m_groundedDelay;
            //print("Grounded");
        }
        else
        {
            //print("NotGroudned");
            m_timeSinceGrounded -= Time.deltaTime;
        }
    }

    public bool IsGrounded()
    {
        RaycastHit t_info;

        LayerMask skipme = LayerMask.NameToLayer("IgnoreCameraOcclusion"); // kanske ta med fiender etc

        LayerMask skipmeToo = LayerMask.NameToLayer("IgnoreSlideCheck");

        int layer = int.MaxValue;
        int test = 1 << skipme;
        int test2 = 1 << skipmeToo;
        //layer &=~test;
        layer -= test;
        layer -= test2;
        bool r_hit = Physics.Raycast(transform.position, Vector3.down, out t_info, GetComponent<Collider>().bounds.extents.y + 0.2f, layer);
        if (t_info.normal.y > TP_Motor.m_instance.m_slideThreshold)
        {
            r_hit = true;
        }
        
        return r_hit;
    }

    public bool IsSliding(ref Vector3 o_collisionNormal)
    {
        RaycastHit t_info;
        LayerMask skipme = LayerMask.NameToLayer("Player"); // kanske ta med fiender etc

        LayerMask skipmeToo = LayerMask.NameToLayer("IgnoreSlideCheck");
        LayerMask skipmeToo2 = LayerMask.NameToLayer("IgnoreCameraOcclusion");

        int layer = int.MaxValue;
        int test = 1 << skipme;
        int test2 = 1 << skipmeToo;
        int test3 = 1 << skipmeToo2;
        //layer &=~test;
        layer -= test;
        layer -= test2;
        layer -= test3;
        bool hit = Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x - 0.02f, Vector3.down, out t_info, (GetComponent<Collider>().bounds.extents.y + 0.2f), layer);

        //bool hit = Physics.Raycast(transform.position, Vector3.down, out t_info, GetComponent<Collider>().bounds.extents.y + 0.2f, layer);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * (GetComponent<Collider>().bounds.extents.y + 0.2f), Color.blue);
        o_collisionNormal = t_info.normal;
        return hit;
    }
    void ShootArrow()
    {
        //RangeCombatScript.m_instance.ShootArrow();
    }
    void Jump()
    {
        TP_Motor.m_instance.Jump();
    }
    void Dash()
    {
        TP_Motor.m_instance.StartDashing();
    }
}
