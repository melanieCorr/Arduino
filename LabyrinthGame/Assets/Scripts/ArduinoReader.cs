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
    public int[] coordXYZ = { 0, 0, 0};
    public int[] coefXYZ  = {0, 0, 0}; // represents the direction to take
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
        coordXYZ = this.UpdateCoords();
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
        int[] newCoordXYZ = this.UpdateCoords();
        int i;
        int threshold = 100;//2;
        for(i = 0; i < (newCoordXYZ.Length - 1); ++i)
        {
            int result = coordXYZ[i] - newCoordXYZ[i];
            if (result > threshold)
                coefXYZ[i] = 1;
            else if (result < -threshold)
                coefXYZ[i] = -1;
            else if (result >= 0 && result <= threshold) 
                coefXYZ[i] = 0;
        }
        coefXYZ[i] = (newCoordXYZ[i] == 0) ? 0 : 1;

        print("OldLen: " + coordXYZ.Length + "\tNewLen: " + newCoordXYZ.Length);
        for (i = 0; i < newCoordXYZ.Length && i < coordXYZ.Length; ++i)
        {
            print(i + ":\t" + coordXYZ[i] + " VS " + newCoordXYZ[i] + "\t==> " + coefXYZ[i]);
            //print(i + ":\t" +  newCoordXYZ[i]);
        }
        print("------------------------");

        /*for (i = 0; i < coordXYZ.Length; ++i)
        {
            coordXYZ[i] = newCoordXYZ[i];
        }
*/
    }
}
