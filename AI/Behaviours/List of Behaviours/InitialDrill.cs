using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Code written by Antoine Kenneth Odi in 2018
/// </summary>

public class InitialDrill : IBehaviour {

    private List<GameObject> m_panels;
    private float m_sightRange = 1f;
    InitialLeben lebenScript = null;
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
        if (!lebenScript && (lebenScript = agent as InitialLeben) == null)
            return BehaviourResult.ERROR;

        if (!m_rage && lebenScript.drillingStart)
        {
            Debug.Log("Drilling door");
            lebenScript.navAgent.updatePosition = false;
            lebenScript.navAgent.ResetPath();
            return BehaviourResult.SUCCESS;
        }
        else if (m_rage && lebenScript.securityDrilling)
        {
            lebenScript.navAgent.updatePosition = false;
            Debug.Log("Security Drilling");
            lebenScript.navAgent.ResetPath();
            return BehaviourResult.SUCCESS;
        }
        //if (lebenScript.panels.Count > 0)
        //{
        //    // Goes through each panel in the list and search for one that Dr. Leben can drill
        //    foreach (GameObject panel in m_panels)
        //    {
        //        if (LineOfSightCheck(panel, agent))
        //            return BehaviourResult.SUCCESS;
        //    }
        //}
        return BehaviourResult.FAILURE;
    }

    public void Exit() { }
}
