using UnityEngine;
using Zenject;

public class CoolDownPack : MonoBehaviour
{
    [SerializeField] private SpriteRenderer basicAttackImage;
    [SerializeField] private SpriteRenderer skill1Image;
    [SerializeField] private SpriteRenderer skill2Image;
    
    private ISkillManager _skillManager;


    private CharacterWearer _characterWearer;

    [Inject]
    void Inject(ISkillManager skillManager)
    {
        _skillManager = skillManager;
    }
    
    void Start()
    {
        _characterWearer = GetComponentInParent<CharacterWearer>();
        
        basicAttackImage.sprite = _skillManager.GetSkill(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem,SkillType.Basic).skillIcon;
        skill1Image.sprite = _skillManager.GetSkill(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem,SkillType.Skill1).skillIcon;
        skill2Image.sprite = _skillManager.GetSkill(_characterWearer.leftHeldItem,_characterWearer.rightHeldItem,SkillType.Skill2).skillIcon;
    }
    

}
