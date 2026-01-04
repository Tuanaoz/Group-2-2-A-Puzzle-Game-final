/*
Stores which main tab and environment sub-tab
this button belongs to.
*/
using UnityEngine;

public enum MainTab
{
    Environment,
    Characters,
    Triggers,
    Goals,
    Utility
}

public enum EnvironmentGroup
{
    None,
    Trees,
    Rocks,
    Ground,
    Crystals
}

public class ButtonInfo : MonoBehaviour
{
    public MainTab mainTab;
    public EnvironmentGroup environmentSubTab;
}