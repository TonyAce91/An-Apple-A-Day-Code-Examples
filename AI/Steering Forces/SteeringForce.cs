using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for all steering forces used in game
/// 
/// Code written by Antoine Kenneth Odi in 2017.
/// </summary>

public abstract class SteeringForce
{
    public abstract Vector3 GetForce(Agent agent);
}
