using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject dinningLight1;
    public GameObject dinningLight2;
    public Renderer light1;
    public Renderer light2;
    public Material mat1;
    public Material mat2;

    private bool isOn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPoked()
    {
        if (isOn)
        {
            dinningLight1.SetActive(false);
            dinningLight2.SetActive(false);
            isOn = false;
            
        }
        else
        {
            dinningLight1.SetActive(true);
            dinningLight2.SetActive(true);
            isOn = true;
        }

    }
}
