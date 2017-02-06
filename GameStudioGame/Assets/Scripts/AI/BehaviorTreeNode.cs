using System;
using System.Text;
using UnityEngine;

/// <summary>
/// Base class for behavior tree nodes
/// </summary>
[Serializable]
public abstract class BehaviorTreeNode : ScriptableObject
{
    /// <summary>
    /// Which decision procedure to use to decide whether to fire the behavior
    /// </summary>
    public DeciderType Decider = DeciderType.Always;
    
    /// <summary>
    /// Whether our parent should redecide whether to run when we finish running
    /// </summary>
    public bool ParentRedecidesOnExit;
    
    /// <summary>
    /// Beahvior won't run unless the baddie's AI level is at least this high
    /// </summary>
    public int MinimumLevel;

    /// <summary>
    /// Check if we want to run
    /// </summary>
    /// <param name="baddie">The baddie being controlled</param>
    /// <returns>True if we want to run</returns>
    public bool Decide(Enemy baddie)
    {
        return baddie.AILevel >= MinimumLevel && Decider.Decide(baddie);
    }

    /// <summary>
    /// Control the baddie.  Called only if this node has been chosen to run.
    /// </summary>
    /// <param name="baddie">baddie to control</param>
    /// <returns>True if we want to keep running.</returns>
    public abstract bool Tick(Enemy baddie);

    /// <summary>
    /// Called when node is selected by parent, and before Tick().
    /// Not called again until node is deselected and then later reselected.
    /// </summary>
    /// <param name="baddie">baddie being controlled</param>
    public virtual void Activate(Enemy baddie) { }

    /// <summary>
    /// Called when node is deselected.
    /// Not called again, until it is reselected and then deselected again.
    /// </summary>
    /// <param name="baddie">baddie being controlled</param>
    public virtual void Deactivate(Enemy baddie) { }

    #region Initialize Player global
    /// <summary>
    /// Just here to make sure the Player global is initialized.
    /// </summary>
    internal virtual void OnEnable()
    {
        if (Player == null)
            Player = GameObject.FindWithTag("Player").transform;
    }
    public static Transform Player;
    #endregion

}
