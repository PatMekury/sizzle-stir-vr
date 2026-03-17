using TMPro;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public string userName = "Patrick";
    public GameObject chair;
    public bool isChairActive = false;
    public TMP_Text nameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isChairActive)
        {
            chair.SetActive(false);
        }
        else
        {
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        nameText.text = userName;
    }
}
