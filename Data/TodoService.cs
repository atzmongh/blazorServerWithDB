using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blazorServerWithDB.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace blazorServerWithDB.Data
{
    public interface ITodoService
    {
        /// <summary>
        /// returns a list of to do items, filtered by status, and ordered by the
        /// requested field
        /// </summary>
        /// <param name="theStatus">can have any value from the list of values in statusFilterList
        /// (see Todoes2.cs). The list contains all valid values of status, plus: "All" and "active"
        /// (active statuses are "not started" and "started")</param>
        /// <param name="orderBy">any field name. For a list of fields see in Todoes2.cs
        /// (ColumnNameList)</param>
        /// <returns>List of Todoes - filtered and ordered</returns>
        Task<List<Todoes>> TodoListByStatus(string theStatus, string orderBy);

        void AddTodo(Todoes theTodo);
        void EditTodo();
    }

    /// <summary>
    /// The service is responsible for sending To do data to the application (the client side) and to
    /// receive new or updated data from the application
    /// </summary>
    public class TodoService : ITodoService
    {
        private TodoDBContext db = new TodoDBContext();

        public TodoService()
        {
        }
        /// <summary>
        /// returns a list of to do items, filtered by status, and ordered by the
        /// requested field
        /// </summary>
        /// <param name="theStatus">can have any value from the list of values in statusFilterList
        /// (see Todoes2.cs). The list contains all valid values of status, plus: "All" and "active"
        /// (active statuses are "not started" and "started")</param>
        /// <param name="orderBy">any field name. For a list of fields see in Todoes2.cs
        /// (ColumnNameList)</param>
        /// <returns>List of Todoes - filtered and ordered</returns>
        public Task<List<Todoes>> TodoListByStatus(string theStatus, string orderBy)
        {
            if (!Todoes.statusFilterList.Contains(theStatus))
            {
                //Invalid status value
                throw new Exception("Invalid status value:"+theStatus);
            }
            IEnumerable<Todoes> filteredTodos;

            if (theStatus == "All")
            {
                //show to dos with any status
                filteredTodos = db.Todoes.OrderBy(columns => columns.DueDate).ToList();
            }
            else if (theStatus == "Active")
            {
                //show only active statuses (not started and started)
                filteredTodos =
                    (from aTodo in db.Todoes
                    where aTodo.Status == "Not Started" || aTodo.Status == "Started"
                    orderby aTodo.DueDate descending
                    select aTodo);
            }
            else
            {
                //show a specific status
                filteredTodos =
                    (from aTodo in db.Todoes
                    where aTodo.Status == theStatus
                    orderby aTodo.DueDate descending 
                    select aTodo);
            }

           
            //sort order
            switch (orderBy)
            {
                case "Start Date":
                    filteredTodos = filteredTodos.OrderBy(todos => todos.StartDate);
                    break;
                case "Id":
                    filteredTodos = filteredTodos.OrderBy(todos => todos.Id);
                    break;
                case "Due Date":
                    filteredTodos = filteredTodos.OrderBy(todos => todos.DueDate);
                    break;
                case "Status":
                    filteredTodos = filteredTodos.OrderBy(todos => todos.Status);
                    break;
                case "Status Date":
                    filteredTodos = filteredTodos.OrderBy(todos => todos.StatusDate);
                    break;
                case "Description":
                    filteredTodos = filteredTodos.OrderBy(todos => todos.Description);
                    break;
                default:
                    throw new Exception("Invalid order by value:" + orderBy);
            }
            return Task.FromResult(filteredTodos.ToList());
        }

        public void AddTodo(Todoes theTodo)
        {
            theTodo.StartDate = DateTime.Now;
            theTodo.StatusDate = DateTime.Now;
            db.Todoes.Add(theTodo);
            db.SaveChanges();
        }

        public void EditTodo()
        {
            db.SaveChanges();
        }
    }
}
 