using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class myThreadClass : MonoBehaviour
{
    bool _threadRunning;
    Thread _thread;

    int maxValue=0;

    void Start()
    {
        // Begin our heavy work on a new thread.
        _thread = new Thread(ThreadedWork);
        _thread.Start();
    }

    void Update(){
        Debug.Log("---------Main Thread-------------------");
    }


    void ThreadedWork()
    {
        _threadRunning = true;
        bool workDone = false;
        int x=0;

        Debug.Log("Iniciando trabajo");

        // This pattern lets us interrupt the work at a safe point if neeeded.
        while(_threadRunning && !workDone)
        {
            if(x>=10000){
                workDone=true;
            }
            // Do Work...
            Debug.Log("valor"+x);
            x++;
        }
        Debug.Log("Trabajo finalizado");
        maxValue=x;
        _threadRunning = false;
    }

    void OnDisable()
    {
        // If the thread is still running, we should shut it down,
        // otherwise it can prevent the game from exiting correctly.
        if(_threadRunning)
        {
            // This forces the while loop in the ThreadedWork function to abort.
            _threadRunning = false;

            // This waits until the thread exits,
            // ensuring any cleanup we do after this is safe. 
            _thread.Join();
        }

        // Thread is guaranteed no longer running. Do other cleanup tasks.
    }
}
