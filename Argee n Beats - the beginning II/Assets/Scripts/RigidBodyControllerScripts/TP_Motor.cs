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

    private bool m_isJumping = false;
    private float m_slideYReducer = 1;
    private float slideIncreaser = 1;
    public float slidingForceMultiplier = 5;
    public Vector3 m_moveVector { get; set; }
	public float m_verticalVel { get; set; }
    public Vector3 m_dashDirection { get; set; }
    private Vector3 m_previousMovingPLatVelo = Vector3.zero;
    private bool m_slidedLastFrame = false;

    Vector3 playerVelocity = new Vector3(0, 0, 0);

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

    void HandleDash(ref Vector3 t_moveVector)
    {
        if (m_dashCooldownTimer > 0f)
        {
            m_dashCooldownTimer -= Time.fixedDeltaTime;
        }
        if (!m_isDashing)
        {
            //not dashing
            return;
        }
        t_moveVector += m_dashDirection * m_currentDashSpeed;
        m_currentDashSpeed -= m_dashSpeedFalloff * Time.fixedDeltaTime;
        if (m_currentDashSpeed < m_forwardSpeed)
        {
            m_isDashing = false;
        }

    }

    void ProcessMotion()
    {
        // Transform MoveVector to Wolrd Space
        Vector3 t_moveVector = transform.TransformDirection(m_moveVector);
        //m_moveVector = transform.TransformDirection(m_moveVector);

        // Normalize movevec if mag > 1
        if (t_moveVector.magnitude > 1)
        {
            t_moveVector = Vector3.Normalize(t_moveVector);
        }

            // Apply Sliding if applicable
        //bool test = ApplySlide();

        // multiply normalised movevec with movespeed
        if (!m_isDashing)
        {
            t_moveVector *= MoveSpeed();
        }
        HandleDash(ref t_moveVector);
        //Reapply Vertical Vel MoveVector.y
        t_moveVector = new Vector3(t_moveVector.x, m_verticalVel, t_moveVector.z);
        Vector3 t_test = AddPlatformMovementVelocity();
        Vector3 xzmovement =  new Vector3(t_moveVector.x, 0, t_moveVector.z);
        TP_Controller.m_rigidBodyController.velocity -= m_previousMovingPLatVelo;
        HandleDrag(ref t_moveVector);
        //xzmovement += t_test;
        TP_Controller.m_rigidBodyController.AddForce(xzmovement * Time.fixedDeltaTime, ForceMode.VelocityChange);
        //TP_Controller.m_rigidBodyController.AddForce(xzmovement * Time.fixedDeltaTime, ForceMode.VelocityChange);
        TP_Controller.m_rigidBodyController.velocity += t_test;
        // New stuff
        playerVelocity += xzmovement * Time.fixedDeltaTime;
        m_previousMovingPLatVelo = t_test;



        Debug.DrawLine(transform.position, transform.position + t_moveVector * 100);
        if(m_isJumping)
        {
            TP_Controller.m_rigidBodyController.AddForce(0, t_moveVector.y, 0, ForceMode.VelocityChange);
            m_isJumping = false;
        }
        else
        {
            TP_Controller.m_rigidBodyController.AddForce(0, t_moveVector.y * Time.fixedDeltaTime, 0, ForceMode.VelocityChange);
        }
    }

    Vector3 AddPlatformMovementVelocity()
    {
        Vector3 r_platformVelocity = Vector3.zero;
        RaycastHit t_info;
        if (Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x - 0.2f, Vector3.down, out t_info, (GetComponent<Collider>().bounds.extents.y + 0.1f)))
        {
            if (!t_info.rigidbody)
            {
                return r_platformVelocity;
            }
            if (t_info.rigidbody.gameObject.tag == "XZMovingPlatform")
            {
                print("Onmovingplatform");
                r_platformVelocity = Vector3.ProjectOnPlane(t_info.rigidbody.velocity, new Vector3(0,1,0));
            }
        }
        return r_platformVelocity;

    }

    float GetNewPlayerVelAxisVal(float oldVal, float playerVel)
    {
        float returnVal = oldVal;
        if (Mathf.Sign(oldVal) != Mathf.Sign(playerVel))
        {
            returnVal = 0;
        }
        else if (Mathf.Abs(oldVal) > Mathf.Abs(playerVel))
        {
            returnVal = playerVel;
        }
        return returnVal;
    }

    void HandleDrag(ref Vector3 t_moveVector)
    {
        //IF du slidar ner för en vägg. Return. How fix?

        Vector3 t_collisionNormal = Vector3.zero;
        if (TP_Controller.Instance.IsSliding(ref t_collisionNormal))
        {
            
            RaycastHit t_info;
            LayerMask skipme = LayerMask.NameToLayer("Player"); // kanske ta med fiender etc
            LayerMask skipmeToo = LayerMask.NameToLayer("IgnoreSlideCheck");
            int layer = int.MaxValue;
            int test = 1 << skipme;
            int test2 = 1 << skipmeToo;
            //layer &=~test;
            layer -= test;
            layer -= test2;
            bool hit = Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x -0.2f, Vector3.down, out t_info, (GetComponent<Collider>().bounds.extents.y + 0.2f), layer);
            //bool hit = Physics.Raycast(transform.position, Vector3.down * (GetComponent<Collider>().bounds.extents.y + 0.2f), out t_info, layer);
            //bool hit = Physics.SphereCast(transform.position, 1f, -transform.up, out t_info, 0.6f, layer);
            //m_slideDirection = new Vector3(t_collisionNormal.x, -t_collisionNormal.y, t_collisionNormal.z);
            //print(t_collisionNormal);
            if (t_collisionNormal.y < m_slideThreshold)
            {
                //Project movevector on plane that we collided with. If slope (normal y<slidevaluethign)
                //Multiply Y component of the projected movevector with a variable that is reduced every frame we are not grounded.
                print("Imsliddddiinnng on :" + t_info.transform.name);
                //If on ground. Reset variable that reduces the Y component
                
                Vector3 t_projectedMoveVec = Vector3.ProjectOnPlane(t_moveVector, t_info.normal);

                t_moveVector = new Vector3(t_projectedMoveVec.x, t_projectedMoveVec.y * m_slideYReducer, t_projectedMoveVec.z);
                //print("Im Sliding " + t_moveVector.y);

                if (m_slideYReducer > 0.0001)
                {
                    m_slideYReducer -= 0.1f;
                }
                else
                {
                    m_slideYReducer = 0f;
                }
                float slidingForce = (1 - (t_info.normal.y / m_slideThreshold)) * slidingForceMultiplier;
                Vector3 temp = TP_Controller.m_rigidBodyController.velocity * -1 * TP_Motor.m_instance.m_linearDrag;
                Vector3 temp2 =  Vector3.down* Time.fixedDeltaTime * slidingForce *slideIncreaser;
                TP_Controller.m_rigidBodyController.AddForce(TP_Controller.m_rigidBodyController.velocity*-1 * TP_Motor.m_instance.m_linearDrag,ForceMode.VelocityChange);
                TP_Controller.m_rigidBodyController.AddForce(Vector3.down * Time.fixedDeltaTime * slidingForce * slideIncreaser);
                slideIncreaser += 5;
                
                Debug.DrawRay(t_info.point, m_slideDirection, Color.blue);
                Debug.DrawRay(transform.position, TP_Controller.m_rigidBodyController.velocity * 100, Color.yellow);
                Debug.DrawRay(t_info.point, t_info.normal, Color.red);
                m_slidedLastFrame = true;
                return;
            }
            else
            {
                m_slideYReducer = 1f; slideIncreaser = 1f;
                if (m_slidedLastFrame == true)
                {
                    m_slidedLastFrame = false;
                    TP_Controller.m_rigidBodyController.velocity = new Vector3(TP_Controller.m_rigidBodyController.velocity.x, 0, TP_Controller.m_rigidBodyController.velocity.z);
                }
            }



        }
        //print("NotSliding");


        // Get player velocity part
        // if sing(pX) != sign(x) pX = 0
        // else if abs(pX) > abs(x), pX = X

        Vector3 velocityInPlane = Vector3.ProjectOnPlane(TP_Controller.m_rigidBodyController.velocity, new Vector3(0, 1, 0));

        // Update player vel
        playerVelocity.x = GetNewPlayerVelAxisVal(playerVelocity.x, velocityInPlane.x);
        playerVelocity.y = GetNewPlayerVelAxisVal(playerVelocity.y, velocityInPlane.y);
        playerVelocity.z = GetNewPlayerVelAxisVal(playerVelocity.z, velocityInPlane.z);



        Vector3 extVel = velocityInPlane - playerVelocity;


        //TP_Controller.m_rigidBodyController.AddForce(-velocityInPlane * TP_Motor.m_instance.m_linearDrag, ForceMode.VelocityChange);
        TP_Controller.m_rigidBodyController.AddForce(-extVel * 0.05f, ForceMode.VelocityChange);
        TP_Controller.m_rigidBodyController.AddForce(-playerVelocity * TP_Motor.m_instance.m_linearDrag, ForceMode.VelocityChange);
        playerVelocity += (-playerVelocity * TP_Motor.m_instance.m_linearDrag);

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
                //print(m_slideDirection + " " + m_slideDirection.magnitude + " " + t_hitInfo.normal);
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
        TP_Controller.m_rigidBodyController.velocity = new Vector3(TP_Controller.m_rigidBodyController.velocity.x, 0, TP_Controller.m_rigidBodyController.velocity.z);
        m_verticalVel = m_jumpSpeed;
        m_isJumping = true;
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
