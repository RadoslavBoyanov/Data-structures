using System;
using System.Collections.Generic;
using System.Linq;

namespace Kubernetes
{
    public class Controller : IController
    {
        private Dictionary<string, Pod> podsById;

        public Controller()
        {
            this.podsById = new Dictionary<string, Pod>();
        }

        public bool Contains(string podId) => this.podsById.ContainsKey(podId);

        public void Deploy(Pod pod)
        {
            if (this.podsById.ContainsKey(pod.Id))
            {
                throw new ArgumentException();
            }

            this.podsById.Add(pod.Id, pod);
        }

        public int Size() => this.podsById.Count;

        public Pod GetPod(string podId)
        {
            if (!this.podsById.ContainsKey(podId))
            {
                throw new ArgumentException();
            }

            var pod = this.podsById[podId];
            return pod;
        }

        public void Uninstall(string podId)
        {
            if (!this.podsById.ContainsKey(podId))
            {
                throw new ArgumentException();
            }

            this.podsById.Remove(podId);
        }

        public void Upgrade(Pod pod)
        {
            if (!this.podsById.ContainsKey(pod.Id))
            {
                this.podsById.Add(pod.Id, pod);
            }
            else
            {
                var oldPod = this.podsById[pod.Id];
                oldPod.Namespace = pod.Namespace;
                oldPod.Port = pod.Port;
                oldPod.ServiceName = pod.ServiceName;
            }
        }

        public IEnumerable<Pod> GetPodsBetweenPort(int lowerBound, int upperBound)
            => this.podsById.Values
                .Where(p => p.Port >= lowerBound && p.Port <= upperBound);

        public IEnumerable<Pod> GetPodsInNamespace(string @namespace)
            => this.podsById.Values
                .Where(p => p.Namespace == @namespace);

        public IEnumerable<Pod> GetPodsOrderedByPortThenByName()
            => this.podsById.Values
                .OrderByDescending(p => p.Port)
                .ThenBy(p => p.Namespace);
    }
}