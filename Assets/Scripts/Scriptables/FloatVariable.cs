using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    [Multiline]
    public string DeveloperDescription = "";

    public float value; 
}
