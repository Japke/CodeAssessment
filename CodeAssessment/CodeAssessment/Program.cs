using System;
using System.Collections.Generic;
using System.IO;
using CodeAssessment;

List<RadarData> radarDataList = new List<RadarData>();

ReadCsv();

BinaryToDecimal();

DetectHostileEntity();

FireMissile();

DisplayResults();

// Reads the csv file
// Saves each line in a RadarData object
// Saves the objects in a List of RadarData objects
void ReadCsv()
{
    var currentDirectory = Directory.GetCurrentDirectory();
    var basePath = currentDirectory.Split(new string[] { "\\bin" }, StringSplitOptions.None)[0];

    using(var reader = new StreamReader(basePath + @"\radar_data.csv"))
    {
        while (!reader.EndOfStream)
        {
            RadarData radarData = new RadarData();
            var line = reader.ReadLine();
            if (line == null) continue;
            var values = line.Split(';');

            for (int i = 0; i < values.Length; i++)
            {
                radarData.binaryList.Add(values[i]);
            }
            radarDataList.Add(radarData);
        }
    }
}

// Converts the value entries in the RadarDataList from binary to decimal
void BinaryToDecimal()
{
    for (int i = 0; i < radarDataList.Count; i++)
    {
        for (int j = 0; j < radarDataList[i].binaryList.Count; j++)
        {
            int result = 0;
            for (int k = radarDataList[i].binaryList[j].Length; k > 0 ; k--)
                if (radarDataList[i].binaryList[j].Substring(k-1,1) == "1")
                    result += (int)Math.Pow(2,(radarDataList[i].binaryList[j].Length-k));
            radarDataList[i].decimalList.Add(result);
        }
    }
}

// Checks if there are more odd or even value entries in each RadarData object
// If there are more odd value entries, a hostile entity is detected and isHostile is set to true
void DetectHostileEntity()
{
    for (int i = 0; i < radarDataList.Count; i++)
    {
        int even = 0;
        int odd = 0;
        for (int j = 0; j < radarDataList[i].decimalList.Count; j++)
        {
            if (radarDataList[i].decimalList[j] % 2 == 0)
            {
                even++;
            }
            else
            {
                odd++;
            }
        }

        radarDataList[i].isHostile = odd > even;
    }
}

// If a hostile entity is detected for a RadarData object, a missile is fired
// Whether or not the missile neutralized the target is determined by a pseudo random number
void FireMissile()
{
    for (int i = 0; i < radarDataList.Count; i++)
    {
        if (radarDataList[i].isHostile)
        {
            Random rnd = new Random();
            int rndValue = rnd.Next(11);
            if (rndValue > 8)
            {
                radarDataList[i].missileLaunch = "A hostile entity was identified. A missile was launched, but did not neutralize the target.";
            }
            else
            {
                radarDataList[i].missileLaunch = "A hostile entity was identified. A missile was launched and the target was neutralized.";
            }
        }
        else
        {
            radarDataList[i].missileLaunch = "There was no hostile entity identified and no missile was launched.";
        }
    }
}

// Display the results, one result per second
// Technically each line takes just over a second to display due to the time it takes to display
void DisplayResults()
{
    for (int i = 0; i < radarDataList.Count; i++)
    {
        Console.WriteLine(radarDataList[i].missileLaunch);
        Console.WriteLine("");
        
        System.Threading.Thread.Sleep(1000);    
    }
}