using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Controller : MonoBehaviour {

    public static Rigidbody m_rigidBodyController;
    public static TP_Controller Instance;


	// Use this for initialization
	void Awake () {
        m_rigidBodyController = GetComponent<Rigidbody>();
        Instance = this;
        TP_Camera.UseExistingOrCreateNewMainCamera();
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main == null)
        {
            return;
        }
        GetLocomotionInput();
        HandleActionInput();
        TP_Motor.m_instance.UpdateMotor();
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
            }
            if (t_hori > deadZone || t_hori < -deadZone)
            {
                TP_Motor.m_instance.m_moveVector += new Vector3(t_hori, 0, 0);
            }
        }
        TP_Animator.m_instance.DetermineCurrentMoveDirection();
    }
    void HandleDrag()
    {
        Vector3 velocityInPlane = Vector3.ProjectOnPlane(m_rigidBodyController.velocity, new Vector3(0, 1, 0));
        m_rigidBodyController.AddForce(-velocityInPlane * TP_Motor.m_instance.m_linearDrag, ForceMode.VelocityChange);

    }
    void HandleActionInput()
    {
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
        if (Input.GetButton("Dash"))
        {
            Dash();
        }
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButton(1))
            {
                ShootArrow();
            }
        }
    }

    public bool IsGrounded()
    {
        RaycastHit t_info;
        return Physics.Raycast(transform.position, Vector3.down, out t_info, GetComponent<Collider>().bounds.extents.y + 0.1f);
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
