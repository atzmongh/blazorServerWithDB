using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using blazorServerWithDB.Models;
using Microsoft.AspNetCore.Components;

namespace blazorServerWithDB.Pages
{
    public partial class Todo
    {
       
        public IEnumerable<Todoes> todoList;

        public Todoes aTodo = new Todoes()
        {
            Description = "",
            DueDate = DateTime.Now,
            StartDate = DateTime.Now,
            Status = "Not Started",
            StatusDate = DateTime.Now
        };

        public string filterStatus = "All";
        public string orderBy = "Id";
        public bool showModalForm = false;
        public string addOrEdit = "add";   //or "edit"
       
        protected override async Task OnInitializedAsync()
        {
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public async Task StatusSelected(string filterStatus)
        {
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public async Task OrderSelected(string orderBy)
        {
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
        }

        public void AddTodo()
        {
            this.aTodo = new Todoes()
            {
                Description = "",
                DueDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = "Not Started",
                StatusDate = DateTime.Now
            };
            this.addOrEdit = "add";
            showModalForm = true;
        }
        

        public void EditTodo(int id)
        {
            aTodo = todoList.Single(oneTodo => oneTodo.Id == id);
            if (aTodo == null)
            {
                throw new Exception($"Todo item with Id:{id} was not found. Total todos in the list: {todoList.Count()}");
            }
           
            addOrEdit = "edit";
            showModalForm = true;
        }

        public async Task ModalFormFinished()
        {
            showModalForm = false;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
            this.StateHasChanged();

        }
    }
}
