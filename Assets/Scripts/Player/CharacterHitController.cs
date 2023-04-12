using UnityEngine;


public enum ColliderType
{
    Impact,
    
}

public interface ICollider
{
    public ColliderType GetColliderType();
}

public class CharacterHitController : MonoBehaviour
{
    private ControllerBase _controllerBase;
    private CharacterStateController _stateController;
    private CharacterAnimationController _animationController;

    private void Start()
    {
        _controllerBase = GetComponent<ControllerBase>();
        _stateController = GetComponent<CharacterStateController>();
        _animationController = GetComponent<CharacterAnimationController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        SkillImpact skillImpact = other.gameObject.GetComponent<SkillImpact>();
        if(skillImpact == null)return;

        if (skillImpact.creator == null)
        {
            Debug.LogError("Creator is not assigned !");
        }
        
        if (skillImpact.creator != null && skillImpact.creator != _controllerBase)
        {
            if (_stateController.IsRolling())
            {
                //player dodged attack
            }
            else
            {
                skillImpact.HitOccurred();
                _animationController.PlayImpactAnimation();
                _stateController.ImpactReceived(skillImpact);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }



}
