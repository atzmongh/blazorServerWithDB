using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;

namespace blazorServerWithDB.Models
{
    
    public partial class Todoes
    {
        public static readonly string[] statusList = new[] 
        {"Not Started", "Started","Completed","Suspended","Cancelled"};
        public static readonly string[] statusFilterList = new[]
            {"All","Not Started", "Started","Completed","Suspended","Cancelled","Active"};

        /// <summary>
        /// Contains the list of column names (found by reflection on the Todoes properties)
        /// </summary>
        public static readonly List<string> ColumnNameList = new List<string>()
        {
            "Id", "Description", "Due Date", "Start Date", "Status", "Status Date"

        };
        /// <summary>
        /// if true - it means we wish to show the "todo steps" of this todo item
        /// </summary>
        public bool ShowTodoSteps = false;
        /// <summary>
        /// if we show the todo steps, then the arrow will be a "top" arrow (we will use oi-chevron-top icon)
        /// Otherwise it will be a oi-chevron-bottom arrow
        /// </summary>
        public string bottomOrTop => ShowTodoSteps ? "top" : "bottom";
        
        public string DueDateSt => DueDate != null ? ((DateTime)DueDate).ToShortDateString() : "";

        public void Update(Todoes updatedTodo)
        {

            updatedTodo.Description = this.Description;
            updatedTodo.DueDate = this.DueDate;
            updatedTodo.Id = this.Id;
            updatedTodo.StartDate = this.StartDate;
            updatedTodo.Status = this.Status;
            updatedTodo.StatusDate = this.StatusDate;
        }
    }
}
