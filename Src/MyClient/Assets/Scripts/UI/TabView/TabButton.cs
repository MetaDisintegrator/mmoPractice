using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TabButton : MonoBehaviour
    {
        public Image render;
        public Sprite activeSprite;
        Sprite commonSprite;

        TabView owner;
        public int index;

        private void Start()
        {
            commonSprite = render.sprite;
        }

        public void Init(TabView owner, int index)
        { 
            this.owner = owner;
            this.index = index;
        }
        public void SetActive(bool active)
        {
            render.overrideSprite = active ? activeSprite : commonSprite;
        }

        public void onTabClick()
        { 
            owner.ChangeTab(index);
        }
    }
}