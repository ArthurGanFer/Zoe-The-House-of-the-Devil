using UnityEngine;

public class PossessableHighlight : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material originalMaterial;
    public Material glowMaterial; // Assign the gold glow material in the inspector

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }

    public void EnableGlow()
    {
        if (objectRenderer != null && glowMaterial != null)
        {
            objectRenderer.material = glowMaterial;
        }
    }

    public void DisableGlow()
    {
        if (objectRenderer != null && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }
}
