using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assembly_CSharp
{
    public abstract class ScreenUIBase : MonoBehaviour
    {

        public const string MessageDisplayScreenId = "msgdsp";

        public string Id { get; protected set; }

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
