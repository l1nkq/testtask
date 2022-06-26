using System.Collections.Generic;
using UnityEngine;

public class JSONAreaConnectionReader : MonoBehaviour
{
    public Dictionary<int, List<int>> Read(TextAsset areaConnectionsFile)
    {
        Dictionary<int, List<int>> connectionsDict = new Dictionary<int, List<int>>(); 
        AreaConnections areaConnections = JsonUtility.FromJson<AreaConnections>("{\"connections\":" + areaConnectionsFile.text + "}");

        foreach(AreaConnection connection in areaConnections.connections)
        {
            if (!connectionsDict.ContainsKey(connection.source_id))
            {
                connectionsDict.Add(connection.source_id, new List<int>{connection.target_id});
            }
            else
            {
                connectionsDict[connection.source_id].Add(connection.target_id);
            }
        }
        
        return connectionsDict;
    }
}
