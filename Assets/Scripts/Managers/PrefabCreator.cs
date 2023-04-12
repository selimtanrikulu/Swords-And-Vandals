using UnityEngine;
using Zenject;


public enum PrefabType
{
    Character,
 
}


public interface IPrefabCreator
{
    GameObject CreatePrefab(PrefabType prefabType);
}

public class PrefabCreator : IPrefabCreator
{
    private readonly DiContainer _diContainer;
    private readonly PrefabPack _prefabPack;

    PrefabCreator(DiContainer diContainer,PrefabPack prefabPack)
    {
        _prefabPack = prefabPack;
        _diContainer = diContainer;
    }

    public GameObject CreatePrefab(PrefabType prefabType)
    {
        GameObject prefab = _prefabPack.prefabs.Find(x => x.prefabType == prefabType).prefab;
        GameObject obj = _diContainer.InstantiatePrefab(prefab);
        return obj;
    }
}
