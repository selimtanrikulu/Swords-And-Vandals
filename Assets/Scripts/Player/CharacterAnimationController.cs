using System;
using UnityEngine;
using Zenject;


public class CharacterAnimationController : MonoBehaviour
{


    private CharacterController _characterController;
    private ControllerBase _controllerBase;
    private CharacterStateController _stateController;
    private CharacterWearer _characterWearer;
    private Animator _animator;
    
    
    
    
    
    private AnimatorOverrider _animatorOverrider;
    private ISkillManager _skillManager;
    private IItemManager _itemManager;
    
    

    [Inject]
    private void Inject(ISkillManager skillManager,IItemManager itemManager)
    {
        _itemManager = itemManager;
        _skillManager = skillManager;
    }

    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _controllerBase = GetComponent<ControllerBase>();
        _stateController = GetComponent<CharacterStateController>();
        _animatorOverrider = GetComponent<AnimatorOverrider>();
        _animator = GetComponent<Animator>();
        _characterWearer = GetComponent<CharacterWearer>();
    }

    void Update()
    {
        HandleAttackAnimation();
        HandleMovementAnimation();
        HandleJumpAnimation();
    }



    public void PlayImpactAnimation()
    {
        _animator.Play("GetImpact", 1, 0f);
    }


    // notifier calls
    private void DodgeStarted()
    {
        _controllerBase.MovementSpeed = MovementConfig.DodgingMovementSpeed;
    }

    // notifier calls
    public void StartWeaponTrail(HeldItemHold heldItemHold)
    {
        _characterWearer.GetWeaponTrail(heldItemHold)?.Play();
    }
    // notifier calls
    public void StopWeaponTrail(HeldItemHold heldItemHold)
    {
        _characterWearer.GetWeaponTrail(heldItemHold)?.Stop();
    }


    
    
    
    // notifier calls
    private void AttackOccurred(HeldItemHold heldItemHold)
    {
        if(_stateController.ActiveSkill == null) return;

        Vector3 hitLocation = _characterWearer.GetHitLocation(heldItemHold).transform.position;
        
        GameObject skillImpactGameObject = Instantiate(GetCurrentImpact(),hitLocation, transform.rotation);


        if (skillImpactGameObject.TryGetComponent(out SkillImpact skillImpact))
        {
            skillImpact.creator = _controllerBase;

            if (skillImpact is Projectile projectile)
            {
                projectile.targetPosition = _controllerBase.enemy.hitTargetLocation.transform.position;
            }

            skillImpact.ScaleDamage(_characterWearer.StatsInstance);
        }
       
        
        _stateController.AttackIntervalState = AttackIntervalState.Occured;
    }


    private GameObject GetCurrentImpact()
    {
        if (_stateController.ActiveSkill is TripleSkill tripleSkill)
        {
            if (_stateController.AttackState == AttackState.Triple1)
            {
                return tripleSkill.skillImpact1;
            }

            if (_stateController.AttackState == AttackState.Triple2)
            {
                return tripleSkill.skillImpact2;
            }

            if(_stateController.AttackState == AttackState.Triple3)
            {
                return tripleSkill.skillImpact3;
            }

            Debug.LogError("Active skill and attack state is not matching");
            return null;
        }

        if (_stateController.ActiveSkill is DoubleSkill doubleSkill)
        {
            if (_stateController.AttackState == AttackState.Double1)
            {
                return doubleSkill.skillImpact1;
            }

            if (_stateController.AttackState == AttackState.Double2)
            {
                return doubleSkill.skillImpact2;
            }

            Debug.LogError("Active skill and attack state is not matching");
            return null;
        }

        if (_stateController.ActiveSkill is SingleSkill singleSkill)
        {
            if (_stateController.AttackState == AttackState.Single)
            {
                return singleSkill.skillImpact;
            }

            Debug.LogError("Active skill and attack state is not matching");
            return null;
        }

        Debug.LogError("There is no active skill");
        return null;
    }
    
    

    private void FillAnimatorAnimations(Skill skill)
    {
        _stateController.ActiveSkill = skill;
        
        if (skill is TripleSkill tripleSkill)
        {
            _animatorOverrider.SetAnimation("Triple-1",tripleSkill.attackAnimation1);
            _animatorOverrider.SetAnimation("Triple-2",tripleSkill.attackAnimation2);
            _animatorOverrider.SetAnimation("Triple-3",tripleSkill.attackAnimation3);
        }
        
        else if (skill is DoubleSkill doubleSkill)
        {
            _animatorOverrider.SetAnimation("Double-1",doubleSkill.attackAnimation1);
            _animatorOverrider.SetAnimation("Double-2",doubleSkill.attackAnimation2);
        }
        
        else if (skill is SingleSkill singleSkill)
        {
            _animatorOverrider.SetAnimation("Single",singleSkill.attackAnimation);
        }
        else
        {
            Debug.LogError("Unknown skill type !");
        }
    }
    
    private void SkillInputArrived(Skill skill)
    {
        if (_stateController.ActiveSkill != null && _stateController.ActiveSkill != skill)
        {
            return;
        }


        if (skill is TripleSkill tripleSkill)
        {
            if (_stateController.AttackState == AttackState.None)
            {
                FillAnimatorAnimations(skill);
                _stateController.AttackState = AttackState.Triple1;
            }
            else if (_stateController.AttackIntervalState == AttackIntervalState.Occured)
            {
                if (_stateController.AttackState == AttackState.Triple1)
                {
                    _stateController.WaitingAttackState = AttackState.Triple2;
                }
                else if (_stateController.AttackState == AttackState.Triple2)
                {
                    _stateController.WaitingAttackState = AttackState.Triple3;
                }
            }
        }
        else if (skill is DoubleSkill doubleSkill)
        {
            if (_stateController.AttackState == AttackState.None)
            {
                FillAnimatorAnimations(skill);
                _stateController.AttackState = AttackState.Double1;
            }

            else if (_stateController.AttackIntervalState == AttackIntervalState.Occured)
            {
                if (_stateController.AttackState == AttackState.Double1)
                {
                    _stateController.WaitingAttackState = AttackState.Double2;
                }
            }
        }
        else if (skill is SingleSkill singleSkill)
        {
            if (_stateController.AttackState == AttackState.None)
            {
                FillAnimatorAnimations(skill);
                _stateController.AttackState = AttackState.Single;
            }
        }
    }
    
    private void HandleAttackAnimation()
    {
        if (_stateController.MovementState != MovementState.Move)
        {
            _stateController.AttackState = AttackState.None;
            return;
        }

        if (_controllerBase.BasicAttackInput)
        {
            SkillInputArrived(_skillManager.GetSkill(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem,SkillType.Basic));
        }
        else if (_controllerBase.Skill1Input)
        {
            SkillInputArrived(_skillManager.GetSkill(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem,SkillType.Skill1));   
        }
        else if (_controllerBase.Skill2Input)
        {
            SkillInputArrived(_skillManager.GetSkill(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem,SkillType.Skill2));
        }


        if (_itemManager.GetCombatClass(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem) == CombatClass.Warrior)
        {
            if (_controllerBase.BlockInput)
            {
                if (_stateController.MovementState == MovementState.Move &&
                    _stateController.AttackState == AttackState.None)
                {
                    _stateController.AttackState = AttackState.Block;
                }
            }
            else
            {
                if (_stateController.AttackState == AttackState.Block)
                {
                    _stateController.AttackState = AttackState.None;
                }
            }
        }
        
    }

    private void HandleJumpAnimation()
    {
        if (_characterController.isGrounded)
        {
            _stateController.JumpState = JumpState.Grounded;


            if (_controllerBase.JumpInput)
            {
                if (//_stateController.AttackState == AttackState.None &&
                    _stateController.MovementState == MovementState.Move)
                {
                    _stateController.JumpState = JumpState.JumpStart;
                    _controllerBase.YVelocity = MovementConfig.JumpStartVelocity;
                }
            }
        }
        else
        {
            if (MovementConfig.FallStartYVelocity > _characterController.velocity.y)
            {
                _stateController.JumpState = JumpState.Fall;
            }
        }

        _animator.SetInteger("JumpState", (int)_stateController.JumpState);
    }

    // notifier calls
    private void DodgeDone()
    {
        _stateController.MovementState = MovementState.Move;
        _controllerBase.MovementSpeed = MovementConfig.RunningMovementSpeed;
    }

    private void HandleMovementAnimation()
    {
        if (_controllerBase.RollInput && _characterController.isGrounded &&
            _stateController.MovementState != MovementState.Stunned &&
            _stateController.MovementState == MovementState.Move)
        {
            if (_controllerBase.VerticalRaw > 0.99f)
            {
                if (_controllerBase.HorizontalRaw > 0.99f)
                {
                    _stateController.MovementState = MovementState.RollForwardRight;
                }
                else if (_controllerBase.HorizontalRaw < -0.99f)
                {
                    _stateController.MovementState = MovementState.RollForwardLeft;
                }
                else
                {
                    _stateController.MovementState = MovementState.RollForward;
                }
            }
            else if (_controllerBase.VerticalRaw < -0.99f)
            {
                if (_controllerBase.HorizontalRaw > 0.99f)
                {
                    _stateController.MovementState = MovementState.RollBackwardRight;
                }
                else if (_controllerBase.HorizontalRaw < -0.99f)
                {
                    _stateController.MovementState = MovementState.RollBackwardLeft;
                }
                else
                {
                    _stateController.MovementState = MovementState.RollBackward;
                }
            }
            else
            {
                if (_controllerBase.HorizontalRaw > 0.99f)
                {
                    _stateController.MovementState = MovementState.RollRight;
                }
                else if (_controllerBase.HorizontalRaw < -0.99f)
                {
                    _stateController.MovementState = MovementState.RollLeft;
                }
                else
                {
                    _stateController.MovementState = MovementState.RollForward;
                }
            }
            

            _stateController.AttackState = AttackState.None;
            _stateController.WaitingAttackState = AttackState.None;
        }


        _animator.SetInteger("MovementState", (int)_stateController.MovementState);
        //used for blend tree
        _animator.SetFloat("x", _controllerBase.Horizontal);
        _animator.SetFloat("y", _controllerBase.Vertical);
        
    }
    

    



    // notifier calls
    private void AttackFinished()
    {
        _stateController.AttackState = _stateController.WaitingAttackState;
        _stateController.WaitingAttackState = AttackState.None;
    }
}