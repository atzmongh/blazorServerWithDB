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

        public void EditTodo(int id)
        {
            aTodo = todoList.Find(oneTodo => oneTodo.Id == id);
            if (aTodo == null)
            {
                throw new Exception("Todo item with Id:" + id + " was not found. Total todos in the list:" + todoList.Count);
            }
           
            addOrEdit = "edit";
            showTodoForm = true;
        }

        public async Task TodoFormFinished()
        {
            showTodoForm = false;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
            this.StateHasChanged();

        }
    }
}
