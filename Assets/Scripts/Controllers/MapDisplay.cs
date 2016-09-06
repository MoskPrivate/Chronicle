using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRenderer;

    void Start()
    {
        textureRenderer = GetComponent<Renderer>();
    }

    public void DrawTexture(Texture2D texture)
    {
        

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
}
