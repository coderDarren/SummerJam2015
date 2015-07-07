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
        menu = Instantiate(OptionsMenu, new Vector3(Screen.width - 100, Screen.height - 100, 0), Quaternion.identity) as GameObject;
        menu.transform.parent = transform;
    }

    void CancelTransformation()
    {
        Destroy(menu);
        InteractiveCursor.InteractionObject.GetComponent<BlockTransformer>().ResetChildrenColors();
        InteractiveCursor.InteractionObject = null;
    }
}
