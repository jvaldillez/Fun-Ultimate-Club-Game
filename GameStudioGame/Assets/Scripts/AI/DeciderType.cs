using System;
using UnityEngine;

public enum DeciderType
{
    /// <summary>
    /// Always willing to run
    /// </summary>
    Always,
    
    /// <summary>
    /// Only runs when we have a direct path to the player
    /// </summary>
    LineOfSightToPlayer,
   
}

public static class DeciderImplementation
{    
    const float DistanceThreshold = 20f;

    /// <summary>
    /// Run the specified decider and returns its value
    /// </summary>
    /// <param name="d">Decider to run</param>
    /// <param name="tank">Tank being controlled</param>
    /// <returns>True if decider wants to run</returns>
    public static bool Decide(this DeciderType d, Enemy baddie)
    {
        switch (d)
        {
            case DeciderType.Always:
                return true;           

            case DeciderType.LineOfSightToPlayer:
                return Vector3.Distance(BehaviorTreeNode.Player.position, baddie.transform.position) < DistanceThreshold;   

            default:
                throw new ArgumentException("Unknown Decider: " + d);
        }
    }
}