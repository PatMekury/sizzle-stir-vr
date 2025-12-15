using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public Material newMaterial;
    private Renderer rend;

    private void Awake()
    {
        // Cache the renderer component
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("No Renderer component found on " + gameObject.name);
            return;
        }
    }

    private void Start()
    {
        if (newMaterial != null && rend != null)
        {
            SwapMaterial();
        }
        else
        {
            Debug.LogWarning("New Material is not assigned on " + gameObject.name);
        }
    }

    public void SwapMaterial()
    {
        if (rend != null && newMaterial != null)
        {
            rend.material = newMaterial;
            Debug.Log("Material swapped successfully on " + gameObject.name);
        }
    }
}

