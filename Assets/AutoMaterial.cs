using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public string myMaterial;
    [HideInInspector] public GameObject matrial;
    public string b;

    public Material LoadMaterial()
    {
        b = myMaterial.Replace(@"Assets/Resources\", "");
        b = b.Replace(".mat", "");
        
        if(b!="SecondaryBodyColor")
        {
            b = "SecondaryBodyColor";
        }
        
        Material yourMaterial = Resources.Load(b, typeof(Material)) as Material;
        return yourMaterial;
    }

    void Start()
    {
        LoadMaterial();
        ColorChanger();
    }

    private void ColorChanger()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material = LoadMaterial(); //Set material.
        }
    }

    // Update is called once per frame
    void Update()
    {
        LoadMaterial();
        ColorChanger();
    }
}
