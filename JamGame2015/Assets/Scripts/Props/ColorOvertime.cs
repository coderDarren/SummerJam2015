using UnityEngine;
using System.Collections;

public class ColorOvertime : MonoBehaviour {

    public int materialIndex = 1; //light mat
    public float colorSmooth = 5;
    public bool inOrder = false;
    public Color[] colors;

    Material mat;
    Color toColor;
    int colorIndex = 0;
    float _colorSmooth;

    void Start()
    {
        _colorSmooth = colorSmooth;
        mat = GetComponent<Renderer>().materials[materialIndex];
        toColor = mat.GetColor("_EmissionColor");
    }

    void Update()
    {
        if (ColorsMatch(colors[colorIndex]))
        {
            GetNextColor();
        }
    }

    void GetNextColor()
    {
        if (inOrder)
        {
            if (colorIndex < colors.Length - 1)
                colorIndex++;
            else
                colorIndex = 0;
        }
        else
        {
            colorIndex = Random.Range(0, colors.Length - 1);
        }
    }

    bool ColorsMatch(Color c)
    {
        toColor = Color.Lerp(mat.GetColor("_EmissionColor"), c, _colorSmooth * Time.deltaTime);

        mat.SetColor("_EmissionColor", toColor);
        mat.SetColor("_Color", toColor);

        if (Mathf.Abs(mat.GetColor("_EmissionColor").r - c.r) < 0.1f &&
            Mathf.Abs(mat.GetColor("_EmissionColor").g - c.g) < 0.1f &&
            Mathf.Abs(mat.GetColor("_EmissionColor").b - c.b) < 0.1f &&
            Mathf.Abs(mat.GetColor("_EmissionColor").a - c.a) < 0.1f)
            return true;
        return false;
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    void AlterSpeed(float speedFactor)
    {
        _colorSmooth = (colorSmooth * speedFactor);
    }

}
