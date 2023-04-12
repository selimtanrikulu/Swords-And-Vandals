using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Skill/TripleSkill")]
public class TripleSkill : Skill
{
    [SerializeField] public AnimationClip attackAnimation1;
    [SerializeField] public GameObject skillImpact1;
    
    [SerializeField] public AnimationClip attackAnimation2;
    [SerializeField] public GameObject skillImpact2;
    
    [SerializeField] public AnimationClip attackAnimation3;
    [SerializeField] public GameObject skillImpact3;

}
