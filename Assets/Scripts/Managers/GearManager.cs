using UnityEngine;


public enum StatType
{
    Int,
    Str,
    Dex,
}

public class CharacterStats
{
    public int Int;
    public int Str;
    public int Dex;


    public CharacterStats GetCopy()
    {
        CharacterStats copy = new CharacterStats
        {
            Int = Int,
            Str = Str,
            Dex = Dex
        };
        return copy;
    }
}


public interface IGearManager
{
    
    //returns a copy
    public CharacterStats GetPlayerStats();
    public void ChangeStat(StatType statType, int amount);
    public int GetPlayerStat(StatType statType);


    //for testing
    public CharacterStats GetRandomCharacterStats();
}

public class GearManager : IGearManager
{
    private CharacterStats _playerStats = new CharacterStats();



    GearManager()
    {
        ChangeStat(StatType.Str, 10);
        ChangeStat(StatType.Int, 5);
        ChangeStat(StatType.Dex, 3);
    }
    
    public CharacterStats GetPlayerStats()
    {
        return _playerStats.GetCopy();
    }

    public void ChangeStat(StatType statType, int amount)
    {
        switch (statType)
        {
            case StatType.Int:
                _playerStats.Int += amount;
                break;
            
            case StatType.Str:
                _playerStats.Str += amount;
                break;
            
            case StatType.Dex:
                _playerStats.Dex += amount;
                break;
            
        }
        
    }

    public int GetPlayerStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.Int:
                return _playerStats.Int;

            case StatType.Str:
                return _playerStats.Str;

            case StatType.Dex:
                return _playerStats.Dex;
            
            default:
                Debug.LogError("Unknown stat");
                return 0;
        }
    }

    public CharacterStats GetRandomCharacterStats()
    {
        CharacterStats result = new CharacterStats()
        {
            Str = Random.Range(0,10),
            Int = Random.Range(0,10),
            Dex = Random.Range(0,10)
            
        };
        return result;
    }
}
