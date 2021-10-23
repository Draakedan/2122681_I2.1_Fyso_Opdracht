using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHandler.Models;


namespace DatabaseHandler.Data
{
    public class DataLoader
    {
        public DataLoader()
        {
            using FysioDataContext context = new();
            var ActionPlans = new ActionPlanRepository();
            foreach (ActionPlan a in context.ActionPlans)
            {
                ActionPlans.Add(a);
            }
        }
    }
}
