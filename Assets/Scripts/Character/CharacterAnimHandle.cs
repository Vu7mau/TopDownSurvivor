using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimHandle : CharacterCtrlAbstract
{
    [Space]
    [Header("Character AnimHandle")]
    [SerializeField] protected Animator _animator;
    public Animator ChracterAnimator=>_animator;

    protected override void LoadComponents()
    {
        base.LoadComponents();
       this.LoadCharacterAnimator();
    }
    protected virtual void LoadCharacterAnimator()
    {
        if( _animator != null ) return;

        _animator=this.transform.GetComponentInChildren<Animator>();
        Debug.Log("Load Character Animator Success " + _animator.transform.name);
    }

    protected virtual void Update()
    {
        this.AnimationCtrl();
    }
    protected virtual void AnimationCtrl()
    {
        _animator.SetFloat("Forward", _characterCtrl.InputManager.MoveInput.y);
        _animator.SetFloat("Sideways", _characterCtrl.InputManager.MoveInput.x);
    }
}
