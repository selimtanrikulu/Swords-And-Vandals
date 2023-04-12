using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


[Serializable]
public struct SkillPack
{
    public SkillSet warriorSkillSet;
    public SkillSet berserkerSkillSet;
    public SkillSet battleMageSkillSet;
    public SkillSet battlePriestSkillSet;
    public SkillSet duelistSkillSet;
    public SkillSet assassinSkillSet;
    public SkillSet crossbowmanSkillSet;
    public SkillSet archerSkillSet;
    public SkillSet mageSkillSet;
    public SkillSet priestSkillSet;
}


[Serializable]
public struct SkillSet
{
    public Skill basicAttack;
    public Skill skill1;
    public Skill skill2;
}


[Serializable]
public struct HeldItemPack
{
    public List<HeldItem> heldItems;
}


[Serializable]
public struct PrefabPack
{
    public List<GamePrefab> prefabs;
}


[Serializable]
public struct GamePrefab
{
    public PrefabType prefabType;
    public GameObject prefab;
}

public class GameInstaller : MonoInstaller
{

    [SerializeField] private SkillPack skillPack;
    [SerializeField] private HeldItemPack heldItemPack;
    [SerializeField] private PrefabPack prefabPack;
    public override void InstallBindings()
    {
        Container.BindInstance(skillPack);
        Container.BindInstance(heldItemPack);
        Container.BindInstance(prefabPack);
        
        Container.Bind<IItemManager>().To<ItemManager>().AsSingle();
        Container.Bind<ISkillManager>().To<SkillManager>().AsSingle();
        Container.Bind<IPrefabCreator>().To<PrefabCreator>().AsSingle();
        Container.Bind<IGearManager>().To<GearManager>().AsSingle();
    }
}