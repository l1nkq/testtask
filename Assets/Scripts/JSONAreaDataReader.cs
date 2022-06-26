using System.Collections.Generic;
using UnityEngine;

public class JSONAreaDataReader : MonoBehaviour
{
    public Dictionary<int, AreaData> Read(TextAsset areaDataFile)
    {
        Dictionary<int, AreaData> areasDataDic = new Dictionary<int, AreaData>(); 
        AreasData areasData = JsonUtility.FromJson<AreasData>("{\"data\":" + areaDataFile.text + "}");
        
        foreach (AreaData areaData in areasData.data)
        {
            areasDataDic.Add(areaData.id, areaData);
        }

        return areasDataDic;
    }
}
