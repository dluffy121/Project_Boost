using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoMaterial))]
public class UnitEditor : Editor
{
    public List<string> _mychoice = new List<string>(Directory.GetFiles("Assets/Resources", "*.mat"));
    public List<string> _choices = new List<string>();
    public string[] _choice;
    public void addintoArray()
    {
        foreach (string i in _mychoice)
        {
            _choices.Add(i);
        }
        _choice = _choices.ToArray();
    }
    int _choiceIndex;

    // Start is called before the first frame update

    public override void OnInspectorGUI()
    {
        AutoMaterial myTarget = (AutoMaterial)target;
        addintoArray();
        DrawDefaultInspector();

        _choiceIndex = EditorGUILayout.Popup("Material", _choiceIndex, _choice);
        // Update the selected choice in the underlying object
        myTarget.myMaterial = _choice[_choiceIndex];
        if(myTarget.myMaterial==null)
            myTarget.myMaterial = _choice[0];
        myTarget.matrial = GameObject.Find(myTarget.myMaterial);
    }
}
