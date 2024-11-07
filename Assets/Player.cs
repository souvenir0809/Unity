using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForace=12;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;    //固定冷却时间
    private float dashUsageTimer;                   //倒数冷却时间
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;        //地面检测坐标点
    [SerializeField] private float groundCheckDistance;     //地面检测距离
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;        //地面图层

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    #region components
    public Animator anim {  get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState {  get; private set; }

    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }

    public PlayerDashState dashState { get; private set; }

    public PlayerWallSlideState wallSlide { get; private set; }

    public PlayerWallJumpState wallJump { get; private set; }
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this,stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState =  new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "WallJump");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>(); 
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }


    private void Update()
    {
        
        stateMachine.currentState.Update();
        FlipController();
        CheckForDashInput();
    }

    private void CheckForDashInput() 
    {
        

        dashUsageTimer -= Time.deltaTime;

        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
                
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity) 
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() {
        if(facingDir > 0)
           return  Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
        else 
           return Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, whatIsGround);
    } 
        

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance,  wallCheck.position.y));
    }

    public void Flip() 
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController() 
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
}
