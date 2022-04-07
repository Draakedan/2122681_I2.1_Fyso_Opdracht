using System.Collections.Generic;
using DomainModels.Models;

namespace DomainServices.Repos
{
    public interface IActionPlan
    {
        void AddActionPlan(ActionPlan actionPlan);
        bool ActionPlanExists(int id);
        List<ActionPlan> GetAllActionPlans();
        ActionPlan GetActionPlanByID(int id);
        void UpdateActionPlan(ActionPlan plan);
    }
}