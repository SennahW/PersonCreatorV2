using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Person_Creator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool tempCorrectInput = false;
            int tempPlayerChoice = 0;
            while (tempCorrectInput == false)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("[1] Create person");
                Console.WriteLine("[2] Read person file");
                Console.WriteLine("[3] Exit program");
                int.TryParse(Console.ReadLine(), out tempPlayerChoice);
                switch (tempPlayerChoice)
                {
                    case 1:
                        Console.Clear();
                        CreatePerson();
                        break;

                    case 2:
                        Console.Clear();
                        ReadPerson();
                        break;

                    case 3:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Try entering 1-3");
                        break;
                }
            }
        }

        static void CreatePerson()
        {
            string[] countries = new string[1];
            string tempFirstname;
            string tempLastname;
            int tempBirthdate = 0;
            int tempGender;
            string tempCountry = "";
            bool tempCorrectInput = false;
            
            Console.WriteLine("Hi there! In this program you can create a person");
            Console.WriteLine("Start by entering the firstname of your new person");
            tempFirstname = Console.ReadLine();
            Console.WriteLine("whats's the lastname?");
            tempLastname = Console.ReadLine();
            Console.Clear();
            while (tempCorrectInput == false)
            {
                string tempUserInput = "";
                Console.WriteLine("When was " + tempFirstname + " " + tempLastname + " born? (YYMMDD)");
                tempUserInput = Console.ReadLine();
                if (tempUserInput.Length > 5 && tempUserInput.Length < 7)
                {
                    int.TryParse(tempUserInput, out tempBirthdate);
                    tempCorrectInput = true;
                }
            }
            Console.WriteLine("What gender is " + tempFirstname + " " + tempLastname + "?");
            Console.WriteLine("[1] Man");
            Console.WriteLine("[2] Women");
            Console.WriteLine("[3] Non-binary");
            int.TryParse(Console.ReadLine(), out tempGender);
            tempCountry = GetCountry(tempCountry);
            string[] filePaths = Directory.GetFiles(Path.GetFullPath("Persons/"), "*.txt");
            string tempFilePath = null;
            for (int i = 0; i < filePaths.Length; i++)
            {
                if (filePaths[i] == Path.GetFullPath("Persons/" + tempLastname + "_" + tempFirstname + ".txt"))
                {
                    int tempUserInput;
                    Console.WriteLine("WARNING!!! A file already exist");
                    Console.WriteLine("[{0}] Throwaway this person",1);
                    Console.WriteLine("[{0}] Overwrite the old file",2);
                    int.TryParse(Console.ReadLine(), out tempUserInput);
                    switch (tempUserInput)
                    {
                        case 1:
                            return;
                        case 2:
                            tempFilePath = Path.GetFullPath("Persons/" + tempLastname + "_" + tempFirstname + ".txt");
                            break;
                    }
                }
                else
                {
                    tempFilePath = Path.GetFullPath("Persons/" + tempLastname + "_" + tempFirstname + ".txt");
                }
            }
            using (FileStream tempFileStream = File.Create(tempFilePath))
            {
                AddText(tempFileStream, tempFirstname + ";" + tempLastname + ";" + tempBirthdate + ";" + tempGender + ";" + tempCountry);
            }
            Console.WriteLine("Your person was saved to " + tempFilePath);
            Console.WriteLine("Press enter to continue");
            Console.ReadKey();
            Console.Clear();
        }

        static void ReadPerson()
        {
            int tempPlayerChoice = 1;
            Console.WriteLine("Select a person file");
            string[] filePaths = Directory.GetFiles(Path.GetFullPath("Persons/"), "*.txt");
            for (int i = 0; i < filePaths.Length; i++)
            {
                Console.WriteLine("[" + (i+1) + "] " + filePaths[i]);
            }
            bool tempCorrectInput = false;
            while (tempCorrectInput == false)
            {
                int.TryParse(Console.ReadLine(), out tempPlayerChoice);
                if (tempPlayerChoice > 0 && tempPlayerChoice <= filePaths.Length)
                {
                    tempCorrectInput = true;
                }
            }
            using (FileStream tempFileStream = File.OpenRead(filePaths[tempPlayerChoice - 1]))
            {
                byte[] tempArray = new byte[tempFileStream.Length];
                UTF8Encoding tempText = new UTF8Encoding(true);
                while (tempFileStream.Read(tempArray, 0, tempArray.Length) > 0)
                {
                    string[] personArray = tempText.GetString(tempArray).Split(';');
                    Console.Clear();
                    Console.WriteLine("Firstname: " + personArray[0]);
                    Console.WriteLine("Lastname: " + personArray[1]);
                    if (personArray[2].Length == 6)
                    {
                        Console.WriteLine("Birthdate: " + personArray[2]);
                    }
                    else if (personArray[2].Length == 5)
                    {
                        Console.WriteLine("Birthdate: 0" + personArray[2]);
                    }
                    switch (Int32.Parse(personArray[3]))
                    {
                        case 1:
                            Console.WriteLine("Gender: Male");
                            break;

                        case 2:
                            Console.WriteLine("Gender: Women");
                            break;

                        case 3:
                            Console.WriteLine("Gender: Non-binary");
                            break;
                    }
                    Console.WriteLine("Country: " + personArray[4]);
                }
            }
            Console.WriteLine("Press a button to continue");
            Console.ReadKey();
            Console.Clear();
        }
        
        static void AddText(FileStream aFileStream, string aStringToAdd)
        {
            byte[] tempTextToWrite = new UTF8Encoding(true).GetBytes(aStringToAdd);
            aFileStream.Write(tempTextToWrite, 0, tempTextToWrite.Length);
        }

        static string GetCountry(string aString)
        { 
            using (FileStream tempFileStream = File.OpenRead(Path.GetFullPath("Countries.txt")))
            {
                byte[] tempArray = new byte[tempFileStream.Length];
                UTF8Encoding tempText = new UTF8Encoding(true);

                List<string> tempCorrectCountries = new List<string>();
                string[] tempCountries = new string[0];
                while (tempFileStream.Read(tempArray, 0, tempArray.Length) > 0)
                {
                    tempCountries = tempText.GetString(tempArray).Split(',');
                }
                for (int i = 0; i < tempCountries.Length; i++)
                {
                    Console.WriteLine(tempCountries[i]);
                }
                Console.WriteLine("What is the first letter of your country");
                string tempUserInputLetter = Console.ReadLine().ToUpper();
                if (tempUserInputLetter.Length > 0)
                {   
                    tempUserInputLetter.Remove(2);
                }
                

                for (int i = 0; i < tempCountries.Length; i++)
                {
                    if (tempCountries[i].Substring(1, 1) == tempUserInputLetter)
                    {
                        tempCorrectCountries.Add(tempCountries[i]);
                    }
                }
                Console.WriteLine("What's your country?");
                for (int i = 0; i < tempCorrectCountries.Count; i++)
                {
                    Console.WriteLine("[{0}]" + tempCorrectCountries[i], i);
                }
                int tempCountryNumber;
                int.TryParse(Console.ReadLine(), out tempCountryNumber );
                Console.Clear();
                return tempCorrectCountries[tempCountryNumber];
            }
        }
    }
}
