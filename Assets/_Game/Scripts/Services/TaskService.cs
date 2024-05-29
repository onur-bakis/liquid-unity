using System.Collections;
using UnityEngine;

namespace Scripts.Services
{ 
    public class TaskService
    {
        public delegate void Task();
        private MonoBehaviour _behaviour;
        
        public void Add(MonoBehaviour behaviour, float delay, Task task)
        {
            _behaviour = behaviour;
            _behaviour.StartCoroutine(DoTask(task, delay));
        }
        
        private static IEnumerator DoTask(Task task, float delay)
        {
            yield return new WaitForSeconds(delay);
            task();
        }
    }
}