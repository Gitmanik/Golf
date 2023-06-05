using System;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text ShotCounter;

    public static HUDController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetShotCounter(int newvalue)
    {
        ShotCounter.text = "" + newvalue;
    }

    public void Reset()
    {
        SetShotCounter(0);
    }
}
