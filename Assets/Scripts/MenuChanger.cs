using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuChanger : MonoBehaviour {
    
	public void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}
