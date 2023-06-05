using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Transform LevelTransform;

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

    int ctr;

    public void OnShot()
    {
        ctr++;
        HUDController.Instance.SetShotCounter(ctr);
    }

    public void SetSkyboxRotation(float newvalue) => RenderSettings.skybox.SetFloat("_Rotation", newvalue);
}
