using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ContinueAttack : Condition {
    private Scarab m_scarab = null;

    public override BehaviourResult UpdateBehaviour(Agent agent)
    {
        if (m_scarab == null && (m_scarab = agent as Scarab) == null)
            return BehaviourResult.ERROR;


		if (m_scarab.stopAttacking == false) {
            //NavMeshAgent navMesh = agent.gameObject.GetComponent<NavMeshAgent> ();
            //if (navMesh)
            //	navMesh.enabled = true;

            // Returns the scarab layer to normal when attacking is possible
            m_scarab.gameObject.layer = 14;
			return BehaviourResult.SUCCESS;
		}
        return BehaviourResult.FAILURE;
    }
}
