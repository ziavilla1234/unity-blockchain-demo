using System.Collections;
using System.Collections.Generic;
using TABBB.Tools.Console;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class BCConsole : MonoBehaviour
{
    ValueNode<string> _public_key;
    ValueNode<string> _credits;
    // Start is called before the first frame update
    void Start()
    {
        Panel panel = Console.I.AddPanel("Wallet");
        // Basic text display
        panel.AddInfo("Login", "To update info");
        _public_key = panel.AddInfo("PublicKey", "0");
        _credits = panel.AddInfo("Credits", "0");

        panel.AddButton("Wallet", "Connect", (x) => { _public_key.SetValue("GEOWNGONEGONON"); _credits.SetValue("10000"); });

        //panel.AddInputField("Input Field", new InputValue("Initial"), InputFieldCallback);

        // Toggles expand and collapse their children by default, similar to the idea of toggling a feature and all 
        // the variables associated to it becoming irrelevant
        //Node toggleNode = panel.AddToggle("Toggle", false, ToggleCallback);
        //toggleNode.AddInfo("Toggle group", "#1");
        //toggleNode.AddInfo("Toggle group", "#2");
        //toggleNode.AddInfo("Toggle group", "#3");
        //toggleNode.AddInfo("Toggle group", "#4");
        //panel.AddSlider("Slider Example", -2f, 4f, 0f, SliderCallback);
        // Filled bars use a color code red-yellow-green based on fill amount (defined on palette)
        //Node nodeHPBars = panel.AddInfo("Enemy Health Bars", "");
        //nodeHPBars.AddFillBar("Enemy #1", 0f, 100f, 10);
        //nodeHPBars.AddFillBar("Enemy #2", 0f, 100f, 30);
        //nodeHPBars.AddFillBar("Enemy #3", 0f, 100f, 50);
        //nodeHPBars.AddFillBar("Enemy #4", 0f, 100f, 70);
        //nodeHPBars.AddFillBar("Enemy #5", 0f, 100f, 90);
        //nodeHPBars.AddFillBar("Enemy #6", 0f, 100f, 100);

        //panel.AddButton("Button", "Press", ButtonCallback);
        //panel.AddVector2("Vector2", new Vector2(Random.value, Random.value), Vector2Callback);
        //panel.AddVector3("Vector3", new Vector3(Random.value, Random.value, Random.value), Vector3Callback);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
