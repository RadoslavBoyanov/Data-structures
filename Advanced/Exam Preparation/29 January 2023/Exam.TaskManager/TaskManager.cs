using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.TaskManager
{
    public class TaskManager : ITaskManager
    {
        private LinkedList<Task> taskForExecution = new LinkedList<Task>();
        private Dictionary<string, Task> allTasks = new Dictionary<string, Task>();

        public void AddTask(Task task)
        {
            taskForExecution.AddLast(task);
            allTasks.Add(task.Id, task);
        }

        public bool Contains(Task task) => allTasks.ContainsKey(task.Id);

        public int Size() => taskForExecution.Count;

        public Task GetTask(string taskId)
        {
            if (!allTasks.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            return allTasks[taskId];
        }

        public void DeleteTask(string taskId)
        {
            if (!allTasks.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            var task = allTasks[taskId];
            allTasks.Remove(taskId);
            if (!task.IsExecuted)
            {
                taskForExecution.Remove(task);
            }
        }

        public Task ExecuteTask()
        {
            if (taskForExecution.Count == 0)
            {
                throw new ArgumentException();
            }

            var task = taskForExecution.First.Value;
            taskForExecution.RemoveFirst();
            task.IsExecuted = true;

            return task;
        }

        public void RescheduleTask(string taskId)
        {
            if (!allTasks.ContainsKey(taskId) || !allTasks[taskId].IsExecuted)
            {
                throw new ArgumentException();
            }

            var task = allTasks[taskId];
            taskForExecution.AddLast(task);
            task.IsExecuted = false;
        }

        public IEnumerable<Task> GetDomainTasks(string domain)
        {
            var result = taskForExecution
                .Where(t => t.Domain == domain);

            if (result.Count() == 0)
            {
                throw new ArgumentException();
            }

            return result;
        }

        public IEnumerable<Task> GetTasksInEETRange(int lowerBound, int upperBound)
            => taskForExecution
                .Where(t => t.EstimatedExecutionTime >= lowerBound && t.EstimatedExecutionTime <= upperBound);

        public IEnumerable<Task> GetAllTasksOrderedByEETThenByName()
            => allTasks.Values
            .OrderByDescending(t => t.EstimatedExecutionTime)
            .ThenBy(t => t.Name.Length);
    }
}