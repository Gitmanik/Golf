using TMPro;
using UnityEngine;

public class fpsmeter : MonoBehaviour
{
    private TMP_Text t;
    void Start()
    {
        t = GetComponent<TMP_Text>();
    }

    void Update()
    {
        t.text = (int) (1f / Time.deltaTime) + "FPS";
    }
}
