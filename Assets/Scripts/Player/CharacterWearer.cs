using System;
using UnityEngine;
using Zenject;


[Serializable]
public struct CharacterWearings
{
    public GameObject rightHandHeldItemSocket;
    public GameObject leftHandHeldItemSocket;
    public GameObject spellBookSocket;
}


public class CharacterWearer : MonoBehaviour
{

    [SerializeField] private CharacterWearings characterWearings;
    private IItemManager _itemManager;


    [HideInInspector]public GameObject rightHandHeldItemGameObject;
    [HideInInspector]public GameObject leftHandHeldItemGameObject;


    [HideInInspector]public HeldItem leftHeldItem;
    [HideInInspector]public HeldItem rightHeldItem;

    public CharacterStats StatsInstance;

    [Inject]
    void Inject(IItemManager itemManager)
    {
        _itemManager = itemManager;
    }


    public GameObject GetHitLocation(HeldItemHold heldItemHold)
    {
        switch (heldItemHold)
        {
            case HeldItemHold.Left:
                if (leftHandHeldItemGameObject)
                {
                    return leftHandHeldItemGameObject.transform.Find("HitLocation").gameObject;
                }
                Debug.LogError("Held Item not exists");
                break;
            
            case HeldItemHold.Right:
                if (rightHandHeldItemGameObject)
                {
                    return rightHandHeldItemGameObject.transform.Find("HitLocation").gameObject;
                }
                break;
        }
        return null;
    }

    public ParticleSystem GetWeaponTrail(HeldItemHold heldItemHold)
    {
        switch (heldItemHold)
        {
            case HeldItemHold.Left:
                if (leftHandHeldItemGameObject)
                {
                    return leftHandHeldItemGameObject.GetComponentInChildren<ParticleSystem>();
                }
                break;
            
            case HeldItemHold.Right:
                if (rightHandHeldItemGameObject)
                {
                    return rightHandHeldItemGameObject.GetComponentInChildren<ParticleSystem>();
                }
                break;
        }

        return null;
    }

    public void WearHeldItem(HeldItem left,HeldItem right)
    {
        leftHeldItem = left;
        rightHeldItem = right;
        
        if (right != null)
        {
            rightHandHeldItemGameObject = Instantiate(right.heldItemPrefab, characterWearings.rightHandHeldItemSocket.transform);
        }
        if( left != null)
        {
            Transform parent = left is SpellBook
                ? characterWearings.spellBookSocket.transform
                : characterWearings.leftHandHeldItemSocket.transform;
            
            leftHandHeldItemGameObject = Instantiate(left.heldItemPrefab, parent);
        }


        ApplyStats(left);
        ApplyStats(right);
    }


    private void ApplyStats(HeldItem heldItem)
    {
        if(heldItem == null) return;

        foreach (HeldItemStats heldItemStats in heldItem.heldItemStats)
        {
            switch (heldItemStats.statType)
            {
                case StatType.Int:
                    StatsInstance.Int += heldItemStats.amount;
                    break;
            
                case StatType.Str:
                    StatsInstance.Str += heldItemStats.amount;
                    break;
            
                case StatType.Dex:
                    StatsInstance.Dex += heldItemStats.amount;
                    break;
                
                
                default:
                    Debug.LogError("Unknown stat type");
                    break;
            }
        }
        
    }
    
    


    
    
}
