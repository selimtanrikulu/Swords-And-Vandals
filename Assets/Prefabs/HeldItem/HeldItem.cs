using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeldItemStats
{
   public StatType statType;
   public int amount;
}

public abstract class HeldItem : ScriptableObject
{
   [SerializeField] public GameObject heldItemPrefab;
   [SerializeField] public List<HeldItemStats> heldItemStats = new List<HeldItemStats>();
}
