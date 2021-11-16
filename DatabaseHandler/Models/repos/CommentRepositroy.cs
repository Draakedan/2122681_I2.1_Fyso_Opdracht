using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseHandler.Models
{
    public class CommentRepositroy : IRepository<Comment>
    {
        public List<Comment> Items { get; init; }

        public CommentRepositroy()
        { 
            Items = new();
        }

        public void Add(Comment elem)
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

        public Comment Get(int id)
        {
            foreach (Comment c in Items)
                if (c.CommenterID == id)
                    return c;
            return null;
        }

        public List<Comment> GetAll()
        {
            return Items;
        }

        public Comment GetItemByID(int id)
        {
            foreach (Comment c in Items)
                if (c.CommentId == id)
                    return c;
            return null;
        }
    }
}
