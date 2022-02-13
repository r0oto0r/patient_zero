using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour {
    public abstract List<Command> GetCommands(Vector3Int cellPos);
}
