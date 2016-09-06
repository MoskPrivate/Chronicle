using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour {

    private static EnvironmentManager _environmentManager;

    public static EnvironmentManager environmentManager {
        get
        {
            if(_environmentManager == null)
            {
                _environmentManager = GameObject.FindObjectOfType<EnvironmentManager>();
            }
            return _environmentManager;
        }
    }
    public List<StaticEntitiy> staticEntList = new List<StaticEntitiy>();

}
[System.Serializable]
public class StaticEntitiy
{
    public string name;
    public Color color;
    public GameObject prefab;
    public float quantity;
}
