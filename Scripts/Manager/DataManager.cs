using UnityEngine;

public class DataManager : GenericSingleton<DataManager>
{
    public string playerName;

    public void SetPlayerName(string name)
    {
        playerName = name;
    }
}