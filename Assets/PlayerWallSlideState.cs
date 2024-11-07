using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) {
            stateMachine.ChangeState(player.wallJump);
            return;
        }
            
        //if (xInput != 0 && player.facingDir != xInput)
        //    stateMachine.ChangeState(player.idleState);

        if(yInput <0 )
            player.rb.velocity = new Vector2(0, player.rb.velocity.y );
        else
            player.rb.velocity = new Vector2(0, player.rb.velocity.y *.4f);
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
