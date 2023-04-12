using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum MovementState
{
    //Includes idle
    Move = 0,
    RollForward = 1,
    RollBackward = 2,
    RollRight = 3,
    RollLeft = 4,
    RollForwardLeft = 5,
    RollForwardRight = 6,
    RollBackwardLeft = 7,
    RollBackwardRight = 8,
    Stunned = -1,
    Died = -2,
}

public enum AttackState
{
    None = 0,
    Triple1 = 31,
    Triple2 = 32,
    Triple3 = 33,
    Double1 = 21,
    Double2 = 22,
    Single = 1,
    Block = -1,
}


public enum JumpState
{
    Grounded = 0,
    JumpStart = 1,
    Fall = 2,
}

public enum AttackIntervalState
{
    None, //Not attacking
    PreOccurrence, //in attack animation, but impact not created yet
    Occured, //in attach animation, impact occurred, animation did not finish yet
}


public class CharacterStateController : MonoBehaviour
{
    private Animator _animator;

    private CharacterAnimationController _animationController;
    
    public GameObject hitTargetLocation;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animationController = GetComponent<CharacterAnimationController>();
        
        _hpBar = GetComponentInChildren<FillBar>();
        _maxHp = 100;
        _hp = _maxHp;
        _hpBar.UpdateBar(_hp,_maxHp);
    }


    private const float AttackStateChangeDelay = 0.1f;
    private float _attackStateChangeDelayCounter;
    
    public AttackState AttackState 
    {
        get => (AttackState)_animator.GetInteger("AttackState");
        set
        {
            if (_attackStateChangeDelayCounter > 0)
            {
                return;
            }

            _attackStateChangeDelayCounter = AttackStateChangeDelay;
            _animator.SetInteger("AttackState",(int)value);
            
            //Debug.Log("Attack state to : " +value);
            
            if (value == AttackState.None)
            {
                AttackIntervalState = AttackIntervalState.None;
                ActiveSkill = null;
                _animationController.StopWeaponTrail(HeldItemHold.Left);
                _animationController.StopWeaponTrail(HeldItemHold.Right);
            }
            else
            {
                AttackIntervalState = AttackIntervalState.PreOccurrence;
            }
        }
    }

    

    public AttackIntervalState AttackIntervalState{ get; set; }

    public Skill ActiveSkill { get; set; }
    
    public JumpState JumpState { get; set; }
    public AttackState WaitingAttackState { get; set; }
    public MovementState MovementState { get; set; }


    private float _maxHp;
    private float _hp;


    private FillBar _hpBar;


    private List<SkillEffect> _currentEffects = new List<SkillEffect>();

    public void ChangeHp(float amount)
    {
        _hp = Mathf.Max(0, _hp + amount);
        if(_hpBar)_hpBar.UpdateBar(_hp,_maxHp);


        //Character died
        if (_hp < 0.01f)
        {
            MovementState = MovementState.Died;
        }
    }

    public bool IsRolling()
    {
        return MovementState is
            MovementState.RollForward or
            MovementState.RollBackward or
            MovementState.RollLeft or
            MovementState.RollRight or
            MovementState.RollBackwardLeft or
            MovementState.RollBackwardRight or
            MovementState.RollForwardRight or
            MovementState.RollForwardLeft;
    }

    
    private void Update()
    {
        if (_attackStateChangeDelayCounter > 0) _attackStateChangeDelayCounter -= Time.deltaTime;


        HandleEffects();

    }

    

    public void ImpactReceived(SkillImpact skillImpact)
    {


        foreach(SkillEffect skillEffect in skillImpact.effects)
        {
            SkillEffect copy = skillEffect.GetCopy();
            ApplyEffect(copy);
        }
        
        ChangeHp(-skillImpact.scaledDamage);
    }
    



    private void ApplyEffect(SkillEffect skillEffect)
    {
        _currentEffects.RemoveAll(x => x.effectType ==skillEffect.effectType);
        _currentEffects.Add(skillEffect);
    }

    private void HandleEffects()
    {
        foreach (SkillEffect skillEffect in _currentEffects)
        {
            skillEffect.duration -= Time.deltaTime;
            switch (skillEffect.effectType)
            {
                case EffectType.Stun:
                    if (skillEffect.duration <= 0)
                    {
                        MovementState = MovementState.Move;
                    }
                    else
                    {
                        MovementState = MovementState.Stunned;
                        AttackState = AttackState.None;
                    }
                    break;
            }
        }


        _currentEffects.RemoveAll(x => x.duration <= 0);

    }



    private void GetStunned2(float duration)
    {
        MovementState = MovementState.Stunned;
        AttackState = AttackState.None;
    }

    
}