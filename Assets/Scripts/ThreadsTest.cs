using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


//Instruction Notes-------
//0.  Read uncommented code that will be executed prior to running.
//1.  Attach ThreadsTest to any GameObject.  Run, view results.  Investigate parallel, asynchronous, processing via debug log output.
//2.  Uncomment the code within the thread joining region.  Investigate results via debug log output.
//3.  For clarity moving forward, comment out all existing code within the start function.
//4.  Uncomment the code with the Data Access Bug region.  Investigate bug.
//5.  Comment out the code within the Data Access Bug region.
//6.  Uncomment out the code within the Threaded Data Access Bug region.  Investigate bug.
//7.  Uncomment out the lock (numbers) lines of code withing the DataProblemThread functions.  Investigate results via debug log output.


public class ThreadsTest : MonoBehaviour
{

    static LinkedList<int> numbers;

    Thread dataProblemThread;
    Thread dataProblemThread2;

    void Start()
    {
        // Thread sampleThread = new Thread(new ThreadStart(ThreadProcess));

        // sampleThread.Start();

        // for (int i = 0; i < 50; i++)
        // {
        //     Debug.Log("Main thread.... " + i);


        //     #region Thread Joining
        //    //Joining a previously started thread to our current one will hold our current thread until the ThreadProcess finishes
        //     if(i == 15)
        //         sampleThread.Join();
        //     #endregion
        
        // }


        #region Data Access Bug

        numbers = new LinkedList<int>();

        // numbers.AddLast(99);
        // numbers.AddLast(99);
        // numbers.AddLast(99);

        // //This will cause a bug, it is not possible to add to a collection while looping through it.
        // foreach(int n in numbers)
        //     numbers.AddLast(n);
        
        #endregion


        #region Threaded Data Access Bug 

        dataProblemThread = new Thread(new ThreadStart(DataProblemThread));
        dataProblemThread2 = new Thread(new ThreadStart(DataProblemThread2));

        dataProblemThread.Start();
        dataProblemThread2.Start();
        
        #endregion


    }

    public static void ThreadProcess()
    {
        for (int i = 0; i < 50; i++)
            Debug.Log("Threaded Process... " + i);
    }

    public static void DataProblemThread()
    {
        for (int i = 0; i < 10000; i++)
        {
            lock (numbers)
            {
                Debug.Log("adding new number");
                numbers.AddLast(99);
            }
        }
    }

    public static void DataProblemThread2()
    {
        for (int i = 0; i < 10000; i++)
        {
            lock (numbers)
            {
                Debug.Log("Looping through numbers");

                foreach (int n in numbers)
                {
                    //just a dummmy operation
                    int x = n;
                }
            }
        }
    }

}