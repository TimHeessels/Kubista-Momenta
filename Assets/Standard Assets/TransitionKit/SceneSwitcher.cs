using UnityEngine;
using Prime31.TransitionKit;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	public Texture2D maskTexture;

	void Awake()
	{
		DontDestroyOnLoad( gameObject );
	}

	public void ResetScene(int levelName)
	{
			var mask = new ImageMaskTransition()
			{
				maskTexture = maskTexture,
				backgroundColor = Color.yellow,
				nextScene = SceneManager.GetActiveScene().buildIndex == 1 ? 2 : 1
			};
			TransitionKit.instance.transitionWithDelegate( mask );
	}
}
