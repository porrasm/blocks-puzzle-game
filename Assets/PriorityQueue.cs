using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PriorityQueue<T> {

    #region fields
    // accuracy at 0.01 seemed OK (LIMIT / QUEUES)
    private const int QUEUES = 10000;
    private const double LIMIT = 100;

    public Queue<T>[] queues;

    public int Count { get; private set; }
    #endregion

    public PriorityQueue() {
        queues = new Queue<T>[QUEUES];
        for (int i = 0; i < QUEUES; i++) {
            queues[i] = new Queue<T>();
        }
    }

    public void Enqueue(T value, double priority) {

        int index = Index(priority);

        queues[index].Enqueue(value);
        Count++;
    }
    public T Dequeue() {
        for (int i = 0; i < QUEUES; i++) {
            if (queues[i].Count > 0) {
                Count--;
                return queues[i].Dequeue();
            }
        }
        throw new Exception("Empty queue");
    }

    private int Index(double priority) {

        if (priority > LIMIT) {
            priority = LIMIT;
        }

        double factor = priority / LIMIT;

        int index = (int)Math.Round(factor * QUEUES);

        if (index >= QUEUES) {
            index = QUEUES - 1;
        }

        return index;
    }
}