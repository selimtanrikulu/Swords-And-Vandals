
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum HeldItemHold
{
    Right,
    Left,
}


public enum CombatClass
{
    Warrior,
    Berserker,
    BattleMage,
    BattlePriest,
    Duelist,
    Assassin,
    Crossbowman,
    Archer,
    Mage,
    Priest,
    Error
}


public interface IItemManager
{
    void SetWearedHeldItem(HeldItem heldItem, HeldItemHold heldItemHold);

    void ResetWearedHeldItem(HeldItemHold heldItemHold);
    
    HeldItem GetWearedHeldItem(HeldItemHold heldItemHold);
    CombatClass GetCombatClass(HeldItem left,HeldItem right);


    //for testing
    HeldItem GetHeldItemByIndex(int index);

}

public class ItemManager : IItemManager
{
    private readonly HeldItemPack _heldItemPack;

    private HeldItem _wearedLeftHandHeldItem;
    private HeldItem _wearedRightHandHeldItem;

    ItemManager(HeldItemPack heldItemPack)
    {
        _heldItemPack = heldItemPack;
        SetWearedHeldItem(_heldItemPack.heldItems[0],HeldItemHold.Left);
        SetWearedHeldItem(_heldItemPack.heldItems[1],HeldItemHold.Right);
    }

    public void SetWearedHeldItem(HeldItem heldItem, HeldItemHold heldItemHold)
    {
        switch (heldItemHold)
        {
            case HeldItemHold.Left:
                _wearedLeftHandHeldItem = heldItem;
                break;
            
            
            case HeldItemHold.Right:
                _wearedRightHandHeldItem = heldItem;
                break;
        }
    }

    public void ResetWearedHeldItem(HeldItemHold heldItemHold)
    {
        switch (heldItemHold)
        {
            case HeldItemHold.Left:
                _wearedLeftHandHeldItem = null;
                break;
            
            case HeldItemHold.Right:
                _wearedRightHandHeldItem = null;
                break;
            
        }
    }

    public HeldItem GetWearedHeldItem(HeldItemHold heldItemHold)
    {
        switch (heldItemHold)
        {
            case HeldItemHold.Left:
                return _wearedLeftHandHeldItem;

            case HeldItemHold.Right:
                return _wearedRightHandHeldItem;
            
        }

        
        Debug.LogError("Unknow HeldItemHold");
        return null;
    }

    public CombatClass GetCombatClass(HeldItem left,HeldItem right)
    {
        if (left is OneHanded && right is OneHanded) return CombatClass.Duelist;
        if (left is null && right is TwoHanded) return CombatClass.Berserker;
        if (left is Shield or null && right is OneHanded) return CombatClass.Warrior;
        if (left is Dagger && right is Dagger) return CombatClass.Assassin;
        if (left is Crossbow && right is null) return CombatClass.Crossbowman;
        if (left is SpellBook && right is OneHanded) return CombatClass.BattleMage;
        if (left is HolySymbol && right is OneHanded) return CombatClass.BattlePriest;
        if (left is Bow && right is null) return CombatClass.Archer;
        if (left is SpellBook or null && right is Wand) return CombatClass.Mage;
        if (left is HolySymbol && right is Wand) return CombatClass.Priest;
      
        return CombatClass.Error;

    }

    public HeldItem GetHeldItemByIndex(int index)
    {
        return _heldItemPack.heldItems.Count > index ? _heldItemPack.heldItems[index] : null;
    }
}
