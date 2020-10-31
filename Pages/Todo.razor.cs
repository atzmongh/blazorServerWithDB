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
        //paginator variables
        public int pageSize = 10;
        public int currentPage = 1;
        public int totalTodoes;


        protected override async Task OnInitializedAsync()
        {
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
        }

        public async Task StatusSelected(string filterStatus)
        {
            this.filterStatus = filterStatus;  //update the filter status field, so that the <Select> element will keep the updated
                                               //valued
            currentPage = 1;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
        }

        public async Task OrderSelected(string orderBy)
        {
            this.orderBy = orderBy;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
        }

        public void AddTodo()
        {
            //Not that we have to clear the EXISTING object - not to create a new object. When creating a new object
            // (aTodo = new Todoes(){...} the binding to TodoModalForm does not work as expected.
            this.aTodo.Description = "";
            this.aTodo.DueDate = DateTime.Now;
            this.aTodo.StartDate = DateTime.Now;
            this.aTodo.Status = "Not Started";
            this.aTodo.StatusDate = DateTime.Now;

            this.addOrEdit = "add";
            showModalForm = true;
        }


        public void EditTodo(int id)
        {
            Todoes updatedTodo = todoList.Single(oneTodo => oneTodo.Id == id);
            if (updatedTodo == null)
            {
                throw new Exception($"Todo item with Id:{id} was not found. Total todos in the list: {todoList.Count()}");
            }
            updatedTodo.Update(aTodo);

            addOrEdit = "edit";
            showModalForm = true;
        }

        public async Task ModalFormFinished()
        {
            showModalForm = false;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
            this.StateHasChanged();

        }
        public void PageClicked()
        {

        }
        public async Task OnPaginatorChanged(int currentPage, int pageSize)
        {
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            todoList = await todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
            this.StateHasChanged();
        }
    }
}
