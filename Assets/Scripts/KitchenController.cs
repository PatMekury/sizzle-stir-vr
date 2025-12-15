using UnityEngine;

public class KitchenController : MonoBehaviour
{
    public GameObject mug;
    public bool mugEnabled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        
    }

    void Start()
    {
       mugEnabled = true;
        if (mugEnabled)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hideMug()
    {
        mug.SetActive(false);
    }
}
