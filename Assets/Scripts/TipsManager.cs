using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public GameObject[] TipsPage;
    private bool OpenTipPage;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        OpenTipPage = false;
        currentIndex = 0;
    }


    void HandleOpenTipPage()
    {
        OpenTipPage = true;
        if(TipsPage.Length!=0)
        {
            TipsPage[currentIndex].SetActive(true);
        }
    }

    void HandleCloseTipPage()
    {
        OpenTipPage = false;
        if (TipsPage.Length != 0)
        {
            TipsPage[currentIndex].SetActive(false);
        }
    }

    public void HandleShowNextTip()
    {
        TipsPage[currentIndex].SetActive(false);
        currentIndex++;
        if(currentIndex>=TipsPage.Length)
        {
            currentIndex = 0;
        }
        TipsPage[currentIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (!OpenTipPage) HandleOpenTipPage();
            else HandleCloseTipPage();
        }
    }
}
