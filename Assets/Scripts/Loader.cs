using Gitmanik.Logging;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
	public static Loader Instance;
	public static string ForceLevel;

	private int CurrentLevelIdx = -1;
	private readonly string[] Levels = { "DyniowyPoziom", "Level1", "Level2", "KaczyPoziom" };
	private string CurrentLevel => Levels[CurrentLevelIdx];

	private void Start()
	{
		Instance = this;

		Application.targetFrameRate = Screen.currentResolution.refreshRate;
		SceneManager.LoadScene("Player", LoadSceneMode.Additive);
		Log.Info($"Quality: {QualitySettings.names[QualitySettings.GetQualityLevel()]}, Refresh rate: {Screen.currentResolution.refreshRate}");

		if (ForceLevel != null)
		{
			LoadLevel(ForceLevel);
			ForceLevel = null;
		}
		else
		{
			LoadLevel(Levels[0]);
		}
	}

	public void LoadLevel(string name) => StartCoroutine(IELoadLevel(name));

	private IEnumerator IELoadLevel(string name)
	{
		Log.Info($"Loading Level: {name}");
		AsyncOperation x = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
		if (CurrentLevelIdx >= 0)
		{
			yield return SceneManager.UnloadSceneAsync(CurrentLevel);
		}

		yield return x;
		CurrentLevelIdx = Array.IndexOf(Levels, name);

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(CurrentLevel));
		BallController.Instance.Setup(GameObject.Find("SpawnPosition").transform.position + Vector3.up * 10);
	}

	public void LoadNextLevel()
	{
		if (CurrentLevelIdx == Levels.Length - 1)
		{
			Log.Info("koniecc");
			return;
		}
		LoadLevel(Levels[CurrentLevelIdx + 1]);
	}

	public void RestartLevel()
	{
		LoadLevel(CurrentLevel);
	}

	internal void OnFinishedLevel()
	{
		Log.Info("Finished level");
		StartCoroutine(WaitFinished(2f));
	}

	private IEnumerator WaitFinished(float amount)
	{
		yield return new WaitForSeconds(amount);
		LoadNextLevel();
	}
}