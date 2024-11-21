using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

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
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        yInput = UnityEngine.Input.GetAxisRaw("Vertical");
        //Debug.Log("i in " + animBoolName);
        player.anim.SetFloat("yVelocity", player.rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        Debug.Log("i exit " + animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
