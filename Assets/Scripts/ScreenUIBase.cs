using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Assembly_CSharp
{
    public abstract class ScreenUIBase : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;

        public bool CanClose 
        { 
            get { return _closeButton ? _closeButton.interactable : false; } 
            set { if (_closeButton) { _closeButton.interactable = value; } } 
        }


        public void ShowScreen() => this.gameObject.SetActive(true);
        public void HideScreen() => this.gameObject.SetActive(false);
    }
}
