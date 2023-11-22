using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [SerializeField] CustomerMovePathSO _movePathSO;
    public CustomerMovePathSO MovePathSO => _movePathSO;
    public Vector3 position => transform.position;
}
