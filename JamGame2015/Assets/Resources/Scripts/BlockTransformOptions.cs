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
        menu = Instantiate(OptionsMenu, new Vector3(Screen.width - 100, 100, 0), Quaternion.identity) as GameObject;
        menu.transform.parent = transform;
        menu.GetComponent<BlockTransformActions>().transformBlock = blockObject;
    }
}
