using UnityEngine;
using System.Collections;

public class BlockTransformOptions : MonoBehaviour
{
    public GameObject OptionsMenu;
    GameObject menu;

    void OnEnable()
    {
        InteractiveCursor.OpenTransformOptions += OpenTransformOptions;
    }

    void OnDisable()
    {
        InteractiveCursor.OpenTransformOptions -= OpenTransformOptions;
    }

    void OpenTransformOptions(GameObject blockObject)
    {
        if (menu)
            Destroy(menu);
        menu = Instantiate(OptionsMenu, new Vector3(Input.mousePosition.x + 100, Input.mousePosition.y - 100, 0), Quaternion.identity) as GameObject;
        menu.transform.parent = transform;
    }
}
