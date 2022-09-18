using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Letter")]
public class Letter : ScriptableObject
{
    public List<PuzzlePart> letterParts;
}
