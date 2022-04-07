using DatabaseHandler.Models;
using DomainModels.Models;

namespace DatabaseHandler.Data
{
    public class DataLoader
    {
        public DataLoader(FysioDataContext context)
        {
            var ActionPlans = new ActionPlanRepository(context);
            foreach (ActionPlan a in context.ActionPlans)
                ActionPlans.AddActionPlan(a);
        }
    }
}