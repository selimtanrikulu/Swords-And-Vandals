using UnityEngine;


public enum SkillType
{
    Basic,
    Skill1,
    Skill2,
}



public interface ISkillManager
{
    Skill GetSkill(HeldItem left,HeldItem right,SkillType skillType);
}




public class SkillManager : ISkillManager
{
    private SkillPack _skillPack;
    private readonly IItemManager _itemManager;


    SkillManager(SkillPack skillPack,IItemManager itemManager)
    {
        _itemManager = itemManager;
        _skillPack = skillPack;
    }



    public Skill GetSkill(HeldItem left,HeldItem right,SkillType skillType)
    {
        switch (_itemManager.GetCombatClass(left,right))
         {
             case CombatClass.Warrior:
                 return GetSkillBySkillType(_skillPack.warriorSkillSet, skillType);
 
             case CombatClass.Berserker:
                 return GetSkillBySkillType(_skillPack.berserkerSkillSet, skillType);
             
             case CombatClass.BattleMage:
                 return GetSkillBySkillType(_skillPack.battleMageSkillSet, skillType);
             
             case CombatClass.BattlePriest:
                 return GetSkillBySkillType(_skillPack.battlePriestSkillSet, skillType);
             
             case CombatClass.Duelist:
                 return GetSkillBySkillType(_skillPack.duelistSkillSet, skillType);
             
             case CombatClass.Assassin:
                 return GetSkillBySkillType(_skillPack.assassinSkillSet, skillType);
             
             case CombatClass.Crossbowman:
                 return GetSkillBySkillType(_skillPack.crossbowmanSkillSet, skillType);

             case CombatClass.Archer:
                 return GetSkillBySkillType(_skillPack.archerSkillSet, skillType);
             
             case CombatClass.Mage:
                 return GetSkillBySkillType(_skillPack.mageSkillSet, skillType);
             
             case CombatClass.Priest:
                 return GetSkillBySkillType(_skillPack.priestSkillSet, skillType);
 
             
             case CombatClass.Error:
                 Debug.LogError("Combat class is not valid !");
                 return null;
             
             default:
                 Debug.LogError("Unknown weapon type !");
                 return null;
         }
    }

    
    //for internal usage
    private Skill GetSkillBySkillType(SkillSet skillSet, SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Basic:
                return skillSet.basicAttack;
            
            case SkillType.Skill1:
                return skillSet.skill1;
            
            case SkillType.Skill2:
                return skillSet.skill2;
            
            default:
                Debug.LogError("Unknown skill type");
                return null;
        }
    }
    
}
