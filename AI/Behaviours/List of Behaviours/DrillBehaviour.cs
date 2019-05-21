using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Code written by Antoine Kenneth Odi in 2018
/// </summary>

public class DrillBehaviour : IBehaviour {

    private List<GameObject> m_panels;
    private float m_sightRange = 1f;
    DrLeben lebenScript = null;
    private GameObject m_eyes;
    private bool m_rage = true;

    // Sets the parameters needed for analysis of the drill behaviour
    public void SetParameters (bool rage = true, List<GameObject> panels = null, float sightRange = 0, GameObject eyes = null)
    {
        m_rage = rage;
        m_panels = panels;
        m_sightRange = sightRange;
        m_eyes = eyes;
    }

    // Updates Dr. Leben drill behaviour
    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        // Checks if Dr Leben script can be detected
        if (!lebenScript && (lebenScript = agent as DrLeben) == null)
            return BehaviourResult.ERROR;

        // Checks whether to rage because door is a security door or just drill normally
        if (!m_rage && lebenScript.drillingStart)
        {
            // Temporarily stops Dr Leben from moving due to navAgent so that he won't slide
            lebenScript.navAgent.updatePosition = false;
            lebenScript.navAgent.ResetPath();
            return BehaviourResult.SUCCESS;
        }

        else if (m_rage && lebenScript.securityDrilling)
        {
            // Temporarily stops Dr Leben from moving due to navAgent so that he won't slide
            lebenScript.navAgent.updatePosition = false;
            lebenScript.navAgent.ResetPath();
            return BehaviourResult.SUCCESS;
        }
        return BehaviourResult.FAILURE;
    }
    public void Exit() { }
}
