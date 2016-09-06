using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    public float daySpeed;
	
	void Update () {
        transform.RotateAround(Vector3.zero, new Vector3(-10,0,5), daySpeed * Time.deltaTime);
        transform.LookAt(Vector3.zero);
	}
}
