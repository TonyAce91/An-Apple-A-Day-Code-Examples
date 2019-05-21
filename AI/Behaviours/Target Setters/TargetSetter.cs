using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSetter {

    public virtual void Initialise(Agent agent) { }
    public abstract Vector3 UpdateTarget(Agent agent);
    public abstract bool FailureTest();
    public virtual void Reset() { }
}
