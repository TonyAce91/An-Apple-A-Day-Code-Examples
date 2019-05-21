using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used for playing animation which can be added as part of behaviour tree
/// 
/// Code written by Antoine Kenneth Odi in 2018.
/// </summary>

public class ChangeScene : IBehaviour {

    private string m_sceneName;

    public void SetParameters(string sceneName)
    {
        m_sceneName = sceneName;
    }

    // Updates the behaviour
    public BehaviourResult UpdateBehaviour(Agent agent)
    {
        SceneManager.LoadScene(m_sceneName);
        return BehaviourResult.SUCCESS;
    }

    public void Exit() { }
}
