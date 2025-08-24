using TMPro;
using UnityEngine;

public class DBDebugText : MonoBehaviour
{
    public static DBDebugText Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    TextMeshProUGUI label;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();

    }
    public void SetText(string text)
    {
        if (label != null)
        {
            label.text = text;
        }
        else
        {
            Debug.LogWarning("DBDebugText label is not set.");
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
