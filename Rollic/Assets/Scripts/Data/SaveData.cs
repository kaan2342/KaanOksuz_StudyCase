using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public bool IsAudioOn { get; set; }
    public bool IsVibrationOn { get; set; }
    public int Level { get; set; }
    public int EquippedSkinId { get; set; }
    public int KeyCount { get; set; }
    public double SoftCurrency { get; set; }
    public List<int> UnlockedSkinIds { get; set; }
}