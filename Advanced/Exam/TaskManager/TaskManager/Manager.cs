using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager
{
    public class Manager : IManager
    {
        private Dictionary<string, Task> tasksById;

        public Manager()
        {
            this.tasksById = new Dictionary<string, Task>();
        }

        public void AddTask(Task task)
        {
            if (this.tasksById.ContainsKey(task.Id))
            {
                throw new ArgumentException();
            }

            this.tasksById.Add(task.Id, task);
        }

        public void RemoveTask(string taskId)
        {
            if (!this.tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            var task = this.tasksById[taskId];
            this.tasksById.Remove(taskId);


            //foreach (var dependecy in task.Dependents)
            //{
            //    dependecy.Dependecies.Remove(task);
            //}

            //foreach (var dependent in task.Dependecies)
            //{
            //    dependent.Dependents.Remove(task);
            //}
        }

        public bool Contains(string taskId) => this.tasksById.ContainsKey(taskId);

        public Task Get(string taskId)
        {
            if (!this.tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            return this.tasksById[taskId];
        }

        public int Size() => this.tasksById.Count;

        public void AddDependency(string taskId, string dependentTaskId)
        {
            if (!this.tasksById.ContainsKey(taskId) || !this.tasksById.ContainsKey(dependentTaskId))
            {
                throw new ArgumentException();
            }

            var task = this.tasksById[taskId];
            var dependentTask = this.tasksById[dependentTaskId];

            if (task.Dependecies.Contains(dependentTask) && dependentTask.Dependents.Contains(task))
            {
                throw new ArgumentException();
            }

            task.Dependecies.Add(dependentTask);
            foreach (var c in dependentTask.Dependecies)
            {
                if (task.Dependents.Count >=1)
                {
                    task.Dependents.Add(c);
                }
                
                task.Dependecies.Add(c);
            }

            foreach (var c in task.Dependents)
            {
                dependentTask.Dependents.Add(c);
                c.Dependecies.Add(dependentTask);
            }
            dependentTask.Dependents.Add(task);
        }

        public void RemoveDependency(string taskId, string dependentTaskId)
        {
            if (!this.tasksById.ContainsKey(taskId) || !this.tasksById.ContainsKey(dependentTaskId))
            {
                throw new ArgumentException();
            }

            var task = this.tasksById[taskId];
            var dependentTask = this.tasksById[dependentTaskId];

            task.Dependecies.Remove(dependentTask);
            foreach (var c in task.Dependents)
            {
                //c.Dependents.Remove(task);
                c.Dependecies.Remove(dependentTask);
            }
            dependentTask.Dependents.Remove(task);
        }

        public IEnumerable<Task> GetDependencies(string taskId)
        {
            var task = this.tasksById[taskId];

            return task.Dependecies;
        }

        public IEnumerable<Task> GetDependents(string taskId)
        {
            var task = this.tasksById[taskId];
            return task.Dependents;
        }
    }
}