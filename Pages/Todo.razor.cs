using blazorServerWithDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Todoes originalTodo = new Todoes();

        public string filterStatus = "All";
        public string orderBy = "Id";
        public bool showModalForm = false;
        public string addOrEdit = "add";   //or "edit"
        //paginator variables
        public int pageSize = 10;
        public int currentPage = 1;
        public int totalTodoes;
        public Dictionary<int, string> descDict = new Dictionary<int, string>();

        protected override void OnInitialized()
        {
            todoList = todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
        }

        public void StatusSelected(string filterStatus)
        {
            this.filterStatus = filterStatus;  //update the filter status field, so that the <Select> element will keep the updated
                                               //valued
            currentPage = 1;
            todoList = todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
        }

        public void OrderSelected(string orderBy)
        {
            this.orderBy = orderBy;
            todoList = todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
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
            aTodo = todoList.Single(oneTodo => oneTodo.Id == id);
            if (aTodo == null)
            {
                throw new Exception($"Todo item with Id:{id} was not found. Total todos in the list: {todoList.Count()}");
            }
            aTodo.Update(originalTodo);  //Keep the original todo here, so if the user cancels his changes - we can revert

            addOrEdit = "edit";
            showModalForm = true;
        }

        public void ShowTodoSteps(int id)
        {
            Todoes clickedTodo = todoList.Single(oneTodo => oneTodo.Id == id);
            if (clickedTodo == null)
            {
                throw new Exception($"Todo item with Id:{id} was not found. Total todos in the list: {todoList.Count()}");
            }
            clickedTodo.ShowTodoSteps = !clickedTodo.ShowTodoSteps;//toggle its state
            if (clickedTodo.ShowTodoSteps)
            {
                //The user wants to see the todo steps - read them
                clickedTodo.TodoSteps = getTodoStepsOf(id).ToList();
            }
            else
            {
                clickedTodo.TodoSteps.Clear();
            }
            this.StateHasChanged();
        }
        public void ModalFormFinished()
        {
            showModalForm = false;
            todoList = todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
            this.StateHasChanged();

        }
        public void PageClicked()
        {

        }
        public void OnPaginatorChanged(int currentPage, int pageSize)
        {
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            todoList = todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
            this.StateHasChanged();
        }
        public IEnumerable<TodoSteps> getTodoStepsOf(int id)
        {
            return todoService.TodoStepsOf(id);
        }
        public void OnAddStep(int id)
        {
            if (!descDict.ContainsKey(id))
            {
                throw new Exception(@"The description dictionary does not contains key:{id}");
            }
            string desc = descDict[id];
            descDict[id] = "";
            if (desc.Trim() != "")
            {
                todoService.AddTodoStep(id, desc);
            }
            todoList = todoService.TodoListByStatus(filterStatus, orderBy, currentPage, pageSize, ref totalTodoes);
            this.ShowTodoSteps(id);
        }
    }
}
