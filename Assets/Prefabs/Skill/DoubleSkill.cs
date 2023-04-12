using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skill/DoubleSkill")]
public class DoubleSkill : Skill
{
    [SerializeField] public AnimationClip attackAnimation1;
    [SerializeField] public GameObject skillImpact1;
    
    [SerializeField] public AnimationClip attackAnimation2;
    [SerializeField] public GameObject skillImpact2;
    
}
