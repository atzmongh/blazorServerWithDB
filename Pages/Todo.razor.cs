using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blazorServerWithDB.Models;
using Microsoft.AspNetCore.Components;

namespace blazorServerWithDB.Pages
{
    public partial class Todo
    {
        #region ===VARIABLES===
        public List<Todoes> todoList;

        public Todoes aTodo = new Todoes()
        {
            Description = "",
            DueDate = DateTime.Now,
            StartDate = DateTime.Now,
            Status = "Not Started",
            StatusDate = DateTime.Now
        };

        public Todoes originalTodo;

        public string filterStatus = "All";

        public string orderBy = "Id";
        public bool showTodoForm = false;
        public string addOrEdit = "add";   //or "edit"

        #endregion
        #region methods
        protected override async Task OnInitializedAsync()
        {
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public async Task StatusSelected(ChangeEventArgs e)
        {
            filterStatus = e.Value.ToString();
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public async Task orderSelected(ChangeEventArgs e)
        {
            orderBy = e.Value.ToString();
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);

        }

        public void AddTodo()
        {
            aTodo = new Todoes()
            {
                Description = "",
                DueDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = "Not Started",
                StatusDate = DateTime.Now
            };
            addOrEdit = "add";
            showTodoForm = true;
        }

        public async Task TodoAdded()
        {
            showTodoForm = false;
            todoService.AddTodo(aTodo);
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public void EditTodo(int id)
        {
            originalTodo = todoList.Find(oneTodo => oneTodo.Id == id);
            if (originalTodo == null)
            {
                throw new Exception("Todo item with Id:" + id + " was not found. Total todos in the list:" + todoList.Count);
            }
            originalTodo.Update(aTodo);  //We copy the values of original to do into aTodo. Note that if we 
                                         //would use assignment (aTodo = originalTodo), then the UI will 
                                         //update immediately when the user updates any field.
                                         //At this point aTodo is equal to the originalTodo - the item before the update.                         
            addOrEdit = "edit";
            showTodoForm = true;
        }

        public async Task TodoEdited()
        {
            showTodoForm = false;
            if (originalTodo.Status != aTodo.Status)
            {
                //Status has changed - update the status date
                aTodo.StatusDate = DateTime.Now;
            }
            aTodo.Update(originalTodo);  //originalTodo is a tracked item. When updating it - it will be saved to DB
            todoService.EditTodo();
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public void AddEditCancelled()
        {
            showTodoForm = false;
        }
#endregion
    }
}
