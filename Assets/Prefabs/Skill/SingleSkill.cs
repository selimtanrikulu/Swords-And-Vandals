using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Skill/SingleSkill")]
public class SingleSkill : Skill
{
    [SerializeField] public AnimationClip attackAnimation;
    [SerializeField] public GameObject skillImpact;
    
}
