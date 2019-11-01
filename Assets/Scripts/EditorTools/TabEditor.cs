using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabEditor : MonoBehaviour
{
    [Rename("Human Walking speeed")]
    public string stringVar1_group1;
    [Rename("Fancy String2")]
    public string stringVar2_group1;
    [Rename("Fancy String3")]
    public string stringVar3_group2;
    [Rename("Fancy String4")]
    public string stringVar4_group3;
    [Rename("Fancy String5")]
    public string stringVar5_group4;
    [Rename("Fancy Float")]
    public float floatVar1_group3;
    [Rename("Fancy bool")]
    public bool boolVar1_group2;


    [Rename("Fancy Int")]
    public int intVar1_group1;
    [Rename("Fancy Int2")]
    public int intVar2_group2;
    [Rename("Fancy Int3")]
    public int intVar3_group3;
    [Rename("Fancy Int4")]
    public int intVar4_group1;

    [HideInInspector]
    public int toolbarTab;
    [HideInInspector]
    public string currentTab; 


}
