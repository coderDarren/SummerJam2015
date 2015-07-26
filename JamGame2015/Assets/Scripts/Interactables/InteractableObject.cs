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
    bool mouseOver = false;

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

    void Update()
    {
        if (mouseOver)
        {
            
            //Why not put this block of code in OnMouseEnter? 
            //OnMouseEnter is called once if the mouse enters this object - so it only checks to interact once
            //If we enter the game object while not in range - it will not continue checking for interaction
            //This may cause the object to seem non-interactable at times.
            if (GetPlayerDistance() < interactableDistance)
            {
                InteractWith(true, gameObject);
                SetMaterialColors(interactColor);
                
            }
        }
    }

    void OnMouseEnter()
    {
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
        InteractWith(false, gameObject);
        if (!selected) //if we didn't just click this object
            ResetMaterialColors(); //then we can reset its color
        else
        {
            if (GetComponent<BlockTransformer>())
                SetMaterialColors(LevelManager.Instance.GetComponent<BlockTransformOptions>().transformationUnblockedColor);
        }
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
