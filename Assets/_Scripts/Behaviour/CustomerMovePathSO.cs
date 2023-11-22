using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerMovePathSO", menuName = "ScriptableObjects/CustomerMovePathSO", order = 1)]
public class CustomerMovePathSO : ScriptableObject
{
    [SerializeField] List<Vector3> outPath;
    public List<Vector3> OutPath => outPath;
    [SerializeField] List<Vector3> inPath;
    [SerializeField] List<Sprite> _sprites;
    public List<Sprite> CustomerSprites => _sprites;
    public List<Vector3> InPath => inPath;
    public  Vector3[] GetINPathasArray => inPath.ToArray();
    public Vector3[] GetOutPathasArray => outPath.ToArray();
    public void AddOutPath(Vector3 point)
    {
        if (outPath == null)
        {
            outPath = new List<Vector3>();
        }
        outPath.Add(point);
    }
    public void AddInPath(Vector3 point)
    {
        if (inPath == null)
        {
            inPath = new List<Vector3>();
        }
        inPath.Add(point);
    }
    
}
