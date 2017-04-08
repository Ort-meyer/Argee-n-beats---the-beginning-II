using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Motor : MonoBehaviour {

    public static TP_Motor m_instance;

    public float m_jumpSpeed = 6f;
    public float m_forwardSpeed = 10f;
    public float m_backwardSpeed = 2f;
    public float m_strafingSpeed = 5f;
    public float m_slideSpeed = 8f;
    public float m_gravity = 21f;
    public float m_terminalVel = 20f;
    public float m_slideThreshold = 0.6f;
    public float m_maxControllableSlideMagnitude = 0.4f;
    public float m_dashCoolDown = 1.0f;
    public float m_dashSpeedImpulse = 50f;
    public float m_dashSpeedFalloff = 100f;
    public bool m_isDashing = false;
    public float m_linearDrag = 0.9f;
    private float m_currentDashSpeed = 0f;
    private float m_dashCooldownTimer = 0;
    private Vector3 m_slideDirection;
    
    public float slidingForceMultiplier = 5;
    public Vector3 m_moveVector { get; set; }
	public float m_verticalVel { get; set; }
    public Vector3 m_dashDirection { get; set; }
    // Use this for initialization
	void Awake () {
        m_instance = this;
	}
	
	// Update is called once per frame
	public void UpdateMotor () {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
	}

    public void StartDashing()
    {
        if (m_dashCooldownTimer > 0f || m_isDashing)
        {
            return;
        }
        m_currentDashSpeed = m_dashSpeedImpulse;
        m_dashCooldownTimer = m_dashCoolDown;
        m_isDashing = true;

    }

    void HandleDash()
    {
        if (m_dashCooldownTimer > 0f)
        {
            m_dashCooldownTimer -= Time.deltaTime;
        }
        if (!m_isDashing)
        {
            //not dashing
            return;
        }
        m_moveVector += m_dashDirection * m_currentDashSpeed;
        m_currentDashSpeed -= m_dashSpeedFalloff * Time.deltaTime;
        if (m_currentDashSpeed < m_forwardSpeed)
        {
            m_isDashing = false;
        }

    }

    void ProcessMotion()
    {
        // Transform MoveVector to Wolrd Space
        m_moveVector = transform.TransformDirection(m_moveVector);

        // Normalize movevec if mag > 1
        if (m_moveVector.magnitude > 1)
        {
            m_moveVector = Vector3.Normalize(m_moveVector);
        }

            // Apply Sliding if applicable
        //bool test = ApplySlide();

        // multiply normalised movevec with movespeed
        if (!m_isDashing)
        {
            m_moveVector *= MoveSpeed();
        }
        HandleDash();
        //Reapply Vertical Vel MoveVector.y
        m_moveVector = new Vector3(m_moveVector.x, m_verticalVel, m_moveVector.z);

        //if (test)
        //{
        //
        //    //Apply Gravity
        //    ApplyGravity();
        //
        //}

        // Move the Character in World space
        //TP_Controller.m_rigidBodyController.velocity = (m_moveVector * Time.deltaTime);
        Vector3 xzmovement =  new Vector3(m_moveVector.x, 0, m_moveVector.z);
        HandleDrag();
        TP_Controller.m_rigidBodyController.AddForce(xzmovement * Time.deltaTime, ForceMode.VelocityChange);
        TP_Controller.m_rigidBodyController.AddForce(0, m_moveVector.y, 0, ForceMode.VelocityChange);
    }
    void HandleDrag()
    {
        //IF du slidar ner för en vägg. Return. How fix?

        Vector3 t_collisionNormal = Vector3.zero;
        if (TP_Controller.Instance.IsSliding(ref t_collisionNormal))
        {
            
            RaycastHit t_info;
            LayerMask skipme = LayerMask.NameToLayer("Player"); // kanske ta med fiender etc

            int layer = int.MaxValue;
            int test = 1 << skipme;
            //layer &=~test;
            layer -= test;
            bool hit = Physics.Raycast(transform.position, -transform.up, out t_info, 4.6f, layer);
            //bool hit = Physics.SphereCast(transform.position, 1f, -transform.up, out t_info, 0.6f, layer);
            //m_slideDirection = new Vector3(t_collisionNormal.x, -t_collisionNormal.y, t_collisionNormal.z);
            if (t_collisionNormal.y < m_slideThreshold)
            {
                //if (m_moveVector.magnitude < 0.01f)
                //{
                //    return;
                //
                //}

                float slidingForce = (1 - (t_info.normal.y / 0.7f)) * slidingForceMultiplier;
                TP_Controller.m_rigidBodyController.AddForce(Vector3.down * slidingForce);
                print("Im sliding");
                //Vector3 t_velInPlane = Vector3.ProjectOnPlane(TP_Controller.m_rigidBodyController.velocity, t_info.normal);
                //TP_Controller.m_rigidBodyController.AddForce(-1*Vector3.up* 5,ForceMode.Force);
                if (TP_Controller.m_rigidBodyController.velocity.y < 0)
                {
                    return;
                }
                TP_Controller.m_rigidBodyController.AddForce(TP_Controller.m_rigidBodyController.velocity*-1 * TP_Motor.m_instance.m_linearDrag,ForceMode.VelocityChange);

                //TP_Controller.m_rigidBodyController.AddForce(-TP_Controller.m_rigidBodyController.velocity * TP_Motor.m_instance.m_linearDrag, ForceMode.VelocityChange);
                //m_slideDirection = new Vector3(t_info.normal.x, -t_info.normal.y, t_info.normal.z);
                //m_slideDirection = Vector3.ProjectOnPlane(m_slideDirection, t_hitInfo.normal);
                Debug.DrawRay(t_info.point, m_slideDirection, Color.blue);
                Debug.DrawRay(transform.position, TP_Controller.m_rigidBodyController.velocity * 100, Color.yellow);
                Debug.DrawRay(t_info.point, t_info.normal, Color.red);
                return;
            }
        }
        Vector3 velocityInPlane = Vector3.ProjectOnPlane(TP_Controller.m_rigidBodyController.velocity, new Vector3(0, 1, 0));
        TP_Controller.m_rigidBodyController.AddForce(-velocityInPlane * TP_Motor.m_instance.m_linearDrag, ForceMode.VelocityChange);

        //if (TP_Controller.Instance.IsGrounded() && !m_jumping)
        //{
        //    TP_Controller.m_rigidBodyController.AddForce(-Vector3.up * m_verticalVel * TP_Motor.m_instance.m_linearDrag, ForceMode.VelocityChange);
        //}
        // Jumping is not gonna be used after this so reset it
        //m_jumping = false;

    }
    void ApplyGravity()
    {
        //if (m_moveVector.y > -m_terminalVel)
        //{
        //    m_moveVector = new Vector3(m_moveVector.x, m_moveVector.y - m_gravity * Time.deltaTime, m_moveVector.z);
        //}
        //if (TP_Controller.Instance.IsGrounded() && m_moveVector.y < -1)
        //{
        //    m_moveVector = new Vector3(m_moveVector.x, -1, m_moveVector.z);
        //}
    }

    bool ApplySlide()
    {
        if (!TP_Controller.Instance.IsGrounded())
        {
            return true;
        }
        m_slideDirection = Vector3.zero;

        RaycastHit t_hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up *0.5f , Vector3.down, out t_hitInfo))
        {
            //print(t_hitInfo.transform.name);
            if (t_hitInfo.normal.y < m_slideThreshold)
            {
                //m_slideDirection = Vector3.Reflect(t_hitInfo.normal, Vector3.up);
                m_slideDirection = new Vector3(t_hitInfo.normal.x, -t_hitInfo.normal.y, t_hitInfo.normal.z);
                //m_slideDirection = Vector3.ProjectOnPlane(m_slideDirection, t_hitInfo.normal);
                Debug.DrawRay(t_hitInfo.point, m_slideDirection, Color.blue);
                Debug.DrawRay(t_hitInfo.point,t_hitInfo.normal, Color.red);
                print(m_slideDirection + " " + m_slideDirection.magnitude + " " + t_hitInfo.normal);
            }
            else
            {
                return true;
            }
        }
        // OM du kan röra dig medan du slidar
        if (m_slideDirection.magnitude < m_maxControllableSlideMagnitude)
        {
            //transform.TransformDirection(m_slideDirection);
            m_moveVector += m_slideDirection;
        }
        else
        {
            //transform.TransformDirection(m_slideDirection);
            m_moveVector = m_slideDirection;
        }
        return false;
    }

    public void Jump()
    {
        if (TP_Controller.Instance.IsGrounded())
        {
            m_verticalVel = m_jumpSpeed;
        }
    }
    void SnapAlignCharacterWithCamera()
    {
        //if (m_moveVector.x != 0 || m_moveVector.z!=0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);

            

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);

        }
    }

    float MoveSpeed()
    {
        var t_moveSpeed = 0f;

        switch (TP_Animator.m_instance.m_moveDirection)
        {
            case TP_Animator.Direction.Stationary:
                t_moveSpeed = 0f;
                break;
            case TP_Animator.Direction.Forward:
                t_moveSpeed = m_forwardSpeed;
                break;
            case TP_Animator.Direction.Backward:
                t_moveSpeed = m_backwardSpeed;
                break;
            case TP_Animator.Direction.Left:
                t_moveSpeed = m_strafingSpeed;
                break;
            case TP_Animator.Direction.Right:
                t_moveSpeed = m_strafingSpeed;
                break;
            case TP_Animator.Direction.LeftForward:
                t_moveSpeed = m_forwardSpeed;
                break;
            case TP_Animator.Direction.RightForward:
                t_moveSpeed = m_forwardSpeed;
                break;
            case TP_Animator.Direction.LeftBackward:
                t_moveSpeed = m_backwardSpeed;
                break;
            case TP_Animator.Direction.RightBackward:
                t_moveSpeed = m_backwardSpeed;
                break;
            default:
                break;
        }

        if (m_slideDirection.magnitude > 0f)
        {
            t_moveSpeed = m_slideSpeed;
        }
        return t_moveSpeed;

    }
}
