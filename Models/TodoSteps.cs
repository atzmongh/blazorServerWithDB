using System;
using System.Collections.Generic;

namespace blazorServerWithDB.Models
{
    public partial class TodoSteps
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TodoId { get; set; }

        public virtual Todoes Todo { get; set; }
    }
}
