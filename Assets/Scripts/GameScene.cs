using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameScene : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController playerAoc;
    [SerializeField] private AnimatorOverrideController aiAoc;

    private IPrefabCreator _prefabCreator;
    private DiContainer _diContainer;
    private IItemManager _itemManager;
    private IGearManager _gearManager;

    [Inject]
    void Inject(IPrefabCreator prefabCreator,DiContainer diContainer,IItemManager itemManager,IGearManager gearManager)
    {
        _gearManager = gearManager;
        _itemManager = itemManager;
        _diContainer = diContainer;
        _prefabCreator = prefabCreator;
    }


    private void Start()
    {
        CreateCharacters();
    }

    private void CreateCharacters()
    {
        //Create player
        GameObject playerGameObject = _prefabCreator.CreatePrefab(PrefabType.Character);
        _diContainer.InstantiateComponent<PlayerControl>(playerGameObject);
        playerGameObject.GetComponent<Animator>().runtimeAnimatorController = playerAoc;
        CharacterWearer playerCharacterWearer = playerGameObject.GetComponent<CharacterWearer>();
        playerCharacterWearer.StatsInstance = _gearManager.GetPlayerStats();
        playerCharacterWearer.WearHeldItem(_itemManager.GetWearedHeldItem(HeldItemHold.Left),_itemManager.GetWearedHeldItem(HeldItemHold.Right));
        //------
        


        //Create ai
        GameObject aiGameObject = _prefabCreator.CreatePrefab(PrefabType.Character);
        _diContainer.InstantiateComponent<AIController>(aiGameObject);
        aiGameObject.GetComponent<Animator>().runtimeAnimatorController = aiAoc;
        CharacterWearer aiCharacterWearer = aiGameObject.GetComponent<CharacterWearer>();
        aiCharacterWearer.StatsInstance = _gearManager.GetPlayerStats();
        aiCharacterWearer.WearHeldItem(null,_itemManager.GetHeldItemByIndex(5));
        //--------
    }
}
