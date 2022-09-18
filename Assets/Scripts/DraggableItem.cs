using System.Linq;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    public void CenterOnChildren()
    {
        var children = GetComponentsInChildren<Transform>().ToList();
        children.Remove(transform);
        var pos = Vector3.zero;
        foreach(var c in children)
        {
            pos += c.position;
            c.parent = null;
        }
        
        pos /= children.Count;
        transform.position = pos;
        foreach(var c in children)
            c.parent = transform;
    }    
}
