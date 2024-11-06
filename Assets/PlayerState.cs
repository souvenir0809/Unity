using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected float xInput;
    private string animBoolName;

    // Start is called before the first frame update

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    { 
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        Debug.Log("i enter " + animBoolName);
    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        Debug.Log("i in " + animBoolName);
        player.anim.SetFloat("yVelocity", player.rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        Debug.Log("i exit " + animBoolName);
    }
}
