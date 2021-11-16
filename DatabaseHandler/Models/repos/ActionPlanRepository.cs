using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
{
    public class ActionPlanRepository : IRepository<ActionPlan>
    {
        public List<ActionPlan> Items { get; init; }

        public ActionPlanRepository()
        {
            Items = new();
        }

        public void Add(ActionPlan elem)
        {
            Items.Add(elem);
        }

        public bool Exists(int id)
        {
            try
            {
                return Items.Contains(Items[id]);
            }
            catch
            {
                return false;
            }
        }

        public ActionPlan Get(int id)
        {
            foreach (ActionPlan plan in Items)
            {
                if (plan.ActionID == id)
                    return plan;
            }
            return null;
        }

        public List<ActionPlan> GetAll()
        {
            return Items;
        }

        public ActionPlan GetItemByID(int id)
        {
            foreach (ActionPlan a in Items)
                if (a.ActionID == id)
                    return a;
            return null;
        }
    }
}
