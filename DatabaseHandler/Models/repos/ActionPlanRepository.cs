using System.Collections.Generic;
using DomainModels.Models;
using DomainServices.Repos;
using DatabaseHandler.Data;

namespace DatabaseHandler.Models
{
    public class ActionPlanRepository : IActionPlan
    {
        private readonly FysioDataContext Context;

        public ActionPlanRepository(FysioDataContext context) => Context = context;

        public void AddActionPlan(ActionPlan actionPlan)
        {
            Context.ActionPlans.Add(actionPlan);
            Context.SaveChanges();
        }

        public bool ActionPlanExists(int id) => GetActionPlanByID(id) != null;

        public List<ActionPlan> GetAllActionPlans()
        {
            List<ActionPlan> actionPlanList = new();
            foreach (ActionPlan actionPlan in Context.ActionPlans)
                actionPlanList.Add(actionPlan);
            return actionPlanList;
        }

        public ActionPlan GetActionPlanByID(int id)
        {
            foreach (ActionPlan a in Context.ActionPlans)
                if (a.ActionID == id)
                    return a;
            return null;
        }

        public void UpdateActionPlan(ActionPlan plan)
        {
            Context.ActionPlans.Update(plan);
            Context.SaveChanges();
        }
    }
}