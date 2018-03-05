using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private const string playerPrefix = "Player ";
    private static Dictionary<string, Player> playerCollection = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _playerObj)
    {
        string _playerID = playerPrefix + _netID;
        playerCollection.Add(_playerID, _playerObj);
        _playerObj.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        playerCollection.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return playerCollection[_playerID];
    }
}
