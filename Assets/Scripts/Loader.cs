using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class Loader : MonoBehaviour
{
	public static Loader Instance;
	public static string ForceLevel;

	[SerializeField] private TMP_Text worldText;
	public int TotalShot = 0;
	
	private int CurrentLevelIdx = -1;
	private readonly string[] Levels = { "DyniowyPoziom", "Level1", "Level2", "KaczyPoziom", "Finished" };
	private string CurrentLevel => Levels[CurrentLevelIdx];

	private void Start()
	{
		Instance = this;

		Application.targetFrameRate = Screen.currentResolution.refreshRate;
		SceneManager.LoadScene("Player", LoadSceneMode.Additive);
		
		worldText.text = "Welcome\nto\n<b>Gitmanik's Golf!</b>\n\n<I>Created by Pawel Reich, ~2020,2025";

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
		Debug.Log($"Loading Level: {name}");
				
		if (name == "Finished")
			worldText.text = $"<b>Thanks for playing!</b>\nTotal score: {TotalShot}";
		
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
			return;
		
		worldText.text = "";
		
		LoadLevel(Levels[CurrentLevelIdx + 1]);
	}

	public void RestartLevel()
	{
		LoadLevel(CurrentLevel);
	}

	internal void OnFinishedLevel()
	{
		Debug.Log("Finished level");
		StartCoroutine(WaitFinished(2f));
	}

	private IEnumerator WaitFinished(float amount)
	{
		yield return new WaitForSeconds(amount);
		LoadNextLevel();
	}
}