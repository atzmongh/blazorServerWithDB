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
        public bool showModalForm = false;
        public string addOrEdit = "add";   //or "edit"
        public Modal modalRef = new Modal();
       
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
            this.aTodo = new Todoes()
            {
                Description = "",
                DueDate = DateTime.Now,
                StartDate = DateTime.Now,
                Status = "Not Started",
                StatusDate = DateTime.Now
            };
            this.addOrEdit = "add";
            // this.showTodoForm = true;
            showModalForm = true;
            this.StateHasChanged();
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
        public async Task ModalFormFinished()
        {
            showModalForm = false;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy);
            this.StateHasChanged();

        }
    }
}
