using UnityEngine;
using System.Collections;

public class BlockTransformOptions : MonoBehaviour
{
    public GameObject OptionsMenu;
    GameObject menu;

    void OnEnable()
    {
        InteractiveCursor.OpenTransformOptions += OpenTransformOptions;
        InteractiveCursor.CloseTransformOptions += CancelTransformation;
    }

    void OnDisable()
    {
        InteractiveCursor.OpenTransformOptions -= OpenTransformOptions;
        InteractiveCursor.CloseTransformOptions -= CancelTransformation;
    }

    void OpenTransformOptions(GameObject blockObject)
    {
        if (menu)
            Destroy(menu);
        menu = Instantiate(OptionsMenu, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity) as GameObject;
        menu.transform.parent = transform;
    }

    void CancelTransformation(GameObject obj)
    {
        Destroy(menu);
        if (InteractiveCursor.InteractionObject.GetComponent<BlockTransformer>())
        {
            InteractiveCursor.InteractionObject.GetComponent<BlockTransformer>().ResetChildrenColors();
            InteractiveCursor.InteractionObject = null;
        }
    }

    void Cancel()
    {
        Destroy(menu);
        InteractiveCursor.InteractionObject.GetComponent<BlockTransformer>().ResetChildrenColors();
        InteractiveCursor.InteractionObject = null;
    }
}
