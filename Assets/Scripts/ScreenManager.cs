using Assembly_CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private ScreenUIBase[] _screens;

    private Stack<ScreenUIBase> _stack;
    public static ScreenManager Instance {get; private set;}
    private void Awake()
    {
        Instance = this;
    }

    public T GetScreen<T>() where T : ScreenUIBase
    {
        foreach(var screen in _screens)
        {
            if(screen.GetType() == typeof(T))
            {
                return (T)screen;
            }
        }
        return null;
    }

    public ScreenUIBase Push(ScreenUIBase screen)
    { 
        _stack.Push(screen);
        return screen;
    }

    public ScreenUIBase Pop() 
    { 
        if(_stack.TryPop(out ScreenUIBase screen))
        {
            return screen;
        }
        return null;
    }
}
