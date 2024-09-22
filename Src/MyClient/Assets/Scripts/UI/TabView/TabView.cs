using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TabView : MonoBehaviour
    {
        public List<TabButton> tabButtons;
        public List<GameObject> tabScvs;

        private void Start()
        {
            StartCoroutine(Init());
        }
        
        public void ChangeTab(int index)
        {
            for (int i = 0; i < tabButtons.Count; i++)
            {
                tabButtons[i].SetActive(i==index);
                tabScvs[i].SetActive(i==index);
            }
        }
        IEnumerator Init()
        {
            for (int i = 0; i < tabButtons.Count; i++)
            {
                tabButtons[i].Init(this, i);
            }
            yield return null;
            ChangeTab(0);
        }
    }
}