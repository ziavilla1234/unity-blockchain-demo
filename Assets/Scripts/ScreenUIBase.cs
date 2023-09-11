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

        public const string MessageDisplayScreenId = "msgdsp";

        public string Id { get; protected set; }

        public Button CloseButton;

        public bool CanClose 
        { 
            get { return CloseButton ? CloseButton.interactable : false; } 
            set { if (CloseButton) { CloseButton.interactable = value; } } 
        }


        public bool ShowScreen() => GameManager.Instance.ShowScreen(this);
        public bool HideScreen()
        {
            if (this == GameManager.Instance.CurrentlyOpenedScreen)
            {
                GameManager.Instance.HideScreen();
                return true;
            }
            return false;
        }
    }
}
