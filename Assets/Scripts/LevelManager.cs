using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;
	private int ShotCounter;

	private void Awake()
	{
		if (Loader.Instance == null)
		{
			Loader.ForceLevel = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene("Loader");
			Destroy(gameObject);
		}
		Instance = this;
	}

	private void Start()
	{
		HUDController.Instance.Reset();
	}

	private void Update()
	{
		SetSkyboxRotation(Time.time);
	}

	public void OnShot()
	{
		ShotCounter++;
		Loader.Instance.TotalShot++;
		HUDController.Instance.SetShotCounter(ShotCounter);
	}

	public void SetSkyboxRotation(float newvalue) => RenderSettings.skybox.SetFloat("_Rotation", newvalue);
}