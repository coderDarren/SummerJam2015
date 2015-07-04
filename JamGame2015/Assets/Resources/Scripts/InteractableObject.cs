using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

	public delegate void InteractNotify(bool canInteract, GameObject interactObject);
    public static event InteractNotify InteractWith;

    public Color interactColor;
    public float interactableDistance = 10;
    public bool selected = false;

    Material[] materials;
    Color[] initialColors;
    Transform player;

    void Start()
    {
        materials = GetComponent<Renderer>().materials;
        initialColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
            initialColors[i] = materials[i].color;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    float GetPlayerDistance()
    {
        return Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
    }

    public void SetMaterialColors(Color toColor)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            SetMaterialBlendMode(materials[i], "Transparent");
            materials[i].SetColor("_Color", toColor);
        }
    }

    public void ResetMaterialColors()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            SetMaterialBlendMode(materials[i], "Opaque");
            materials[i].color = initialColors[i];
        }
    }

    void OnMouseEnter()
    {
        if (GetPlayerDistance() < interactableDistance)
        {
            InteractWith(true, gameObject);
            SetMaterialColors(interactColor);
        }
    }

    void OnMouseExit()
    {
        InteractWith(false, gameObject);
        if (!selected) //if we didn't just click this object
            ResetMaterialColors(); //then we can reset its color
    }

    void SetMaterialBlendMode(Material material, string BLEND_MODE)
    {
        switch (BLEND_MODE)
        {
            case "Opaque":
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case "Fade":
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case "Transparent":
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
