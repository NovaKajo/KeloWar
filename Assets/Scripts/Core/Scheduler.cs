using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kelo.Core
{
public class Scheduler : MonoBehaviour
{
     IAction currentAction;

    public void StartAction(IAction action)
    {
        if (currentAction == action) return;
        if (currentAction != null)
        {
            currentAction.Disengage();

        }


        currentAction = action;

    }
    public void CancelCurrentAction()
    {
        StartAction(null);
    }
}
}