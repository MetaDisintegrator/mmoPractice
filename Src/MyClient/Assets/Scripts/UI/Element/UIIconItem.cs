using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIIconItem : MonoBehaviour
    {
        public Image mainImage;
        public Image secondImage;
        public Text mainText;
        int _count;
        int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    mainText.text = value.ToString();
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetMainIcon(string iconName, int count)
        {
            mainImage.overrideSprite = Resloader.Load<Sprite>(iconName);
            Count = count;
        }
        public void SetCount(int count)
        { 
            Count = count;
        }
    }
}