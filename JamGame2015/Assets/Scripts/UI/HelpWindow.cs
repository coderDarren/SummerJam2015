using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpWindow : MonoBehaviour {

    public delegate void HandleViewability(string window, bool viewable);
    public static event HandleViewability SetViewable;
    public delegate void PopUpHandler(bool paused);
    public static PopUpHandler PausePlayer;

    [System.Serializable]
    public class Page
    {
        public GameObject PageLayout;
        public string title;
        public Sprite image;
        public string description;

        GameObject page;

        [HideInInspector]
        public Text[] texts;
        [HideInInspector]
        public Image[] images;

        public void InitializeAtRootPage(GameObject rootPage)
        {
            page = Instantiate(PageLayout as GameObject);
            page.transform.parent = rootPage.transform;
            page.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            page.GetComponent<RectTransform>().localScale = Vector3.one;

            texts = page.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if (text.name.Equals("Title"))
                    text.text = title;
                if (text.name.Equals("Description"))
                    text.text = description;

                text.text = text.text.Replace("NEWLINE", "\n");
            }
            images = page.GetComponentsInChildren<Image>();
            foreach (Image i in images)
            {
                if (i.name.Equals("MainImage"))
                    i.sprite = image;
            }
        }

        public void DestroyPage()
        {
            Destroy(page);
        }
    }


    public Page[] pages;
    public string windowName = "Name This Help Window";

    [HideInInspector]
    public bool showAgain = true; //just for this help window

    int pageIndex = 0;

    void Start()
    {
        //bring up the first page
        pages[pageIndex].InitializeAtRootPage(gameObject);
        PausePlayer(true);
    }

    public void GoToNextPage()
    {
        pages[pageIndex].DestroyPage();

        if (pageIndex < pages.Length - 1)
            pageIndex++;

        pages[pageIndex].InitializeAtRootPage(gameObject);
    }

    public void GoToPreviousPage()
    {
        pages[pageIndex].DestroyPage();

        if (pageIndex > 0)
            pageIndex--;

        pages[pageIndex].InitializeAtRootPage(gameObject);
    }

    public void CloseHelpWindow()
    {
        Destroy(gameObject);
        PausePlayer(false);
    }

    public void ToggleShowAgain()
    {
        showAgain = !showAgain;
        SetViewable(windowName, showAgain); //HelpWindowManager.cs listens tot his
    }
}
