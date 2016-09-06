using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    float min = 4;
    float max = 10;
    public float zoomMultiplier = 3;
    public GameObject player;
	void Update () {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomMultiplier;
        Vector3 dist = -(transform.position - player.transform.position);

        if (dist.magnitude < min  && zoom > 0 || dist.magnitude > max && zoom < 0)
        {
        }
        else
        { 
            transform.Translate(dist.normalized * zoom);
        }
	}
}
