using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    [Multiline]
    public string DeveloperDescription = "";

    public int value;
}
