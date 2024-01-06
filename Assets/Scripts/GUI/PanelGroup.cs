using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGroup : MonoBehaviour
{
    public List<GameObject> panels;
    public List<GameObject> buttons;

    public void Show(int idPanel)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(i == idPanel)
            {
                buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                buttons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                buttons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == idPanel);
        }
    }
}
