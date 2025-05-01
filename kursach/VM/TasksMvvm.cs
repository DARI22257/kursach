using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kursach.Model;

namespace kursach.VM
{
    internal class TasksMvvm : BaseVM
    {
        private Task newTask = new();

        public Task NewTask
        {
            get => newTask;
            set
            {
                newTask = value;
                Signal();
            }
        }

        private Task selectedTask = new();

        public Task SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                Signal();
            }
        }

        private ObservableCollection<Task> tasks = new();

        public ObservableCollection<Task> Tasks
        {
            get => tasks;
            set
            {
                tasks = value;
                Signal();
            }
        }


        public CommandMvvm InsertTask { get; set; }

        public CommandMvvm NextPage { get; set; }
        public TasksMvvm()
        {
            SelectAll();
            InsertTask = new CommandMvvm(() =>
            {
                TaskDB.GetDb().Insert(NewTask);
                NewTask = new();
                SelectAll();
            },
                () =>
                !string.IsNullOrEmpty(newTask.Title) &&
                !string.IsNullOrEmpty(newTask.Description) &&
                DateOnly.MinValue != newTask.Term &&
                !string.IsNullOrEmpty(newTask.Assigne) &&
                !string.IsNullOrEmpty(newTask.Status));

            NextPage = new CommandMvvm(() =>
            {
                Task tasks = new Task(SelectedTask);
                tasks.Show();
                close?.Invoke();
            },
                () => SelectedTask != null);
        }

        private void SelectAll()
        {
            Tasks = new ObservableCollection<Task>(TaskDB.GetDb().SelectAll());
        }
        Action close;
        internal void SetClose(Action close)
        {
            this.close = close;
        }
    }
}
