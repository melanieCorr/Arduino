using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports;
using System.Threading;
using System;

using System.Text.RegularExpressions;
using System.Linq;

public class ArduinoReader : MonoBehaviour
{
    // Start is called before the first frame update
    SerialPort serialPort;
    //public int[] coordXYZ = { 0, 0, 0};
    public int[] coordXYZ = { 500, 500, 10}; // 500 is the state where the joystick is not moved 
    public int[] coefXYZ  = {0, 0, 0}; // represents the direction to take
    int[] newCoordXYZ = { 0, 0, 0 } ;

    public void Start()
    {        
        serialPort = new SerialPort("COM3", 9600);
        serialPort.DtrEnable = true;

        serialPort.RtsEnable = true;
        while (! serialPort.IsOpen)
        {
            print("Try to open the serial port\n");
            serialPort.Open();
        }

        print("Serial port opened\n");
        //coordXYZ = this.UpdateCoords();
    }

    // Update is called once per frame
    public void Update()
    {
        //this.PrintArduinoOutput();
        this.updateDirectionCoef();
    }

    public void PrintArduinoOutput()
    {
        print("\n\n In PrintArduinoOutput() \n\n");
        //string b = serialPort.ReadExisting();
        string a = serialPort.ReadLine();
        //print("Output: " + b);
        print("Output: " + a);
        //print("Line: " + a + "\tExisting: " + b);
        //Console.WriteLine(a);
        //Console.Write("Other one: " + b);
        //Thread.Sleep(1200);
        string test = new String(a.Where(Char.IsDigit).ToArray());        
        string[] test2 = Regex.Split(a.Trim(), @"\D+");
        test2 = test2.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        print("test: " + test);
        print("------------------------");
        for (int i = 0; i < test2.Length; ++i)
            print(i + "==> " + test2[i]);
        print("------------------------");
    }

    public void PrintOutput()
    {
        print("output: " + serialPort.ReadLine());
    }


    public int[] UpdateCoords()
    {
        // Get Arduino output           
        string output = serialPort.ReadLine();
        // Get the numbers from the output using a regex
        string[] numData = Regex.Split(output, @"\D+");
        // Remove potential empty spaces that could appear
        numData = numData.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        return Array.ConvertAll(numData, int.Parse);
    }

    public void updateDirectionCoef()
    {

        int i;
        int threshold = 100;//2;
        bool updateOldCoords = false;
        int[] coords = this.UpdateCoords();

        for (i = 0; i < newCoordXYZ.Length && i < coords.Length; ++i)
            newCoordXYZ[i] = coords[i];
        
        for(i = 0; i < (newCoordXYZ.Length - 1) && i < coordXYZ.Length; ++i)
        {
            //print("BEFORE\tOldLen: " + coordXYZ.Length + "\tNewLen: " + newCoordXYZ.Length);
            int delta = coordXYZ[i] - newCoordXYZ[i];
            if (delta > threshold)
                coefXYZ[i] = 1;
            else if (delta < -threshold)
                coefXYZ[i] = -1;
            else if (delta >= 0 && delta <= threshold)
            {
                coefXYZ[i] = 0;
            }

           /* if(coefXYZ[i] != 0)
                updateOldCoords = true;*/

        }
        coefXYZ[i] = (newCoordXYZ[i] == 0) ? 1 : 0;

        print("AFTER\tOldLen: " + coordXYZ.Length + "\tNewLen: " + newCoordXYZ.Length);
        for (i = 0; i < newCoordXYZ.Length /*&& i < coordXYZ.Length*/; ++i)
        {
            print(i + ":\t" + coordXYZ[i] + " VS " + newCoordXYZ[i] + "\t==> " + coefXYZ[i]);
            
        }
        print("---------------------------------------------------------------------------------------------------------------------------------------------------------");

       /* for (i = 0; updateOldCoords && i < coordXYZ.Length; ++i)
        {
            //print("UPDATE OLD COORDS");
            coordXYZ[i] = newCoordXYZ[i];
        }*/

    }


    /* public void UpdateOldCoords()
     {
         print("UPDATE OLD COORDS");

     }*/
}
