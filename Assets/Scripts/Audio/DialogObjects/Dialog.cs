using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog_", menuName = "ScriptableObjects/Dialog")]
public class Dialog : ScriptableObject
{
    public Speech[] Speeches;
}
