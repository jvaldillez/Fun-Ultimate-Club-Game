using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Behavior tree node that choses between child nodes
/// and recursive runs the selected child.
/// </summary>
[Serializable]
public class GroupDecider : BehaviorTreeNode
{
    /// <summary>
    /// Children of this node.  When we run, we recursively run one of them.
    /// </summary>
    public List<BehaviorTreeNode> Children = new List<BehaviorTreeNode>();
    /// <summary>
    /// Child currently selected to run.  We continue to run it until its Tick() method returns false.
    /// </summary>
    private BehaviorTreeNode selected = null;


    /// <summary>
    /// Policy to use in selecting child to run
    /// </summary>
    public SelectionPolicy Policy = SelectionPolicy.Prioritized;

    public enum SelectionPolicy
    {
        /// <summary>
        /// Take the first child whose Decide() method returns true.
        /// </summary>
        Prioritized,
        /// <summary>
        /// Randomly choose a child whose Decide method returns true.
        /// </summary>
        Random,
        /// <summary>
        /// Run children in order
        /// </summary>
        Sequential,
        /// <summary>
        /// Run children in order, looping forever.
        /// </summary>
        Loop
    }

    /// <summary>
    /// We're not running anymore; recursively deactivate our selected child.
    /// </summary>
    /// <param name="baddie">baddie being controlled</param>
    public override void Deactivate(Enemy baddie)
    {
        if (selected)
        {
            selected.Deactivate(baddie);
            selected = null;
        }
    }

#if DEBUG
    public override void Activate(Enemy baddie)
    {
        // Check to make sure the subset property is satisfied
        if (!Children.Any(c => c.Decide(baddie)))
            Debug.Log(name + " activated without runnable child");
    }
#endif

    /// <summary>
    /// Run our selected child.  If no child is selected, select one.  If can't select one, return false.
    /// </summary>
    /// <param name="baddie">baddie being controlled.</param>
    /// <returns>Whether we want to continue running.</returns>
    public override bool Tick(Enemy baddie)
    {

        var newSelected = SelectChild(baddie);
        if (newSelected == null)
        {
            Deactivate(baddie);
            return false;
        }
        //if new child is different deactivate old one and activate new one
        if (newSelected != selected)
        {
            Deactivate(baddie);
            selected = newSelected;
            selected.Activate(baddie);
        }

        if (!selected.Tick(baddie))
            Deactivate(baddie);

        return true;
    }

    /// <summary>
    /// Select a child to run based on the policy.
    /// </summary>
    /// <param name="baddie">baddie being controlled</param>
    /// <returns>Child to run, or null if no runnable children.</returns>
    private BehaviorTreeNode SelectChild(Enemy baddie)
    {
        switch (Policy)
        {
            case SelectionPolicy.Prioritized:

                //return first child that returns true Decide()
                foreach (BehaviorTreeNode c in Children)
                    if (c == selected || c.Decide(baddie))
                        return c;

                //if no child wants to run 
                return null;

            default:
                throw new NotImplementedException("Unimplemented policy: " + Policy);
        }
    }

}
