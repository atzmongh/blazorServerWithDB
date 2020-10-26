using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace blazorServerWithDB.Models
{
    public partial class Todoes
    {
        public Todoes()
        {
            TodoSteps = new HashSet<TodoSteps>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter something in the description")]
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

        public virtual ICollection<TodoSteps> TodoSteps { get; set; }
    }
}
