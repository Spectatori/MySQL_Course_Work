using System.Text;
using Course_Work.Infrastructure;
namespace Course_Work.backend{
    public class Commands{
        static char[] _splitters = {' ', '(', ':','"','”','“',')'}; //splitters needed in the method CreateTable
        public static void CreateTable(string readText)
        {
            int counter = 1;
            string[] command = Infrastructure.CustomCommands.SplitOnCharAndNum(readText,' ', 2);//readText.Split(' ', 2);
            List<string> dftList = new List<string>();
            List<string> dataList = new List<string>();
            
            switch (command[0])
            {
                case "CreateTable":
                    string[] split = Infrastructure.CustomCommands.SplitOnArrAndNum(command[1],_splitters, 2);
                    dataList.Add(split[0]);
                    split = Infrastructure.CustomCommands.SplitOnChar(split[1],',');
                    foreach (string sentence in split)
                    {
                        string[] toArray = Infrastructure.CustomCommands.SplitOnArr(sentence,_splitters);
                        foreach (var word in toArray)
                        {
                            if(word != string.Empty)
                                dataList.Add(word);
                        }
                    }
                    break;
            }
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    switch (dataList[i])
                    {
                        case "int":
                            dataList[i] = "System.Int32";
                            dataList[i - 1] = dataList[i - 1] + ":";
                            break;
                    
                        case "string":
                            dataList[i] = "System.String";
                            dataList[i - 1] = dataList[i - 1] + ":";
                            break;
                    
                        case "bool":
                            dataList[i] = "System.Boolean";
                            dataList[i - 1] = dataList[i - 1] + ":";
                            break;
                        case "char":
                            dataList[i] = "System.Char";
                            dataList[i - 1] = dataList[i - 1] + ":";
                            break;
                        case "date":
                            dataList[i] = "System.DateTime";
                            dataList[i - 1] = dataList[i - 1] + ":";
                            break;
                        case "default":
                            dftList.Add(dataList[i-2]+" ");
                            dftList.Add(dataList[i-1] + " ");
                            dftList.Add(dataList[i+1]);
                            dataList.RemoveAt(i+1);
                            dataList.RemoveAt(i);
                            dataList.RemoveAt(i-1);
                            dataList[i - 2] = dataList[i - 2] + " ";
                            break;
                    }
                }
            }
            string fileName = Environment.CurrentDirectory+ "/" + dataList[0]+".marinata";
            if (File.Exists(fileName))
            {
                Console.WriteLine("Table already exists");
            }
            else
                using (FileStream fs = File.Create(fileName))
                {
                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                    for (int j = 1; j < dataList.Count; j +=2)
                    {
                        for (int i = 0; i < dftList.Count; i+=3)
                        {
                            if (dftList[i] == dataList[j])
                            {
                                dataList[j] = dftList[i] + dftList[i + 1]+ dftList[i + 2];
                            }
                        }

                        if (j == dataList.Count()-1)
                        {
                            Byte[] data = new UTF8Encoding(true).GetBytes(dataList[j]);
                            fs.Write(data, 0, data.Length);
                            break;
                        }
                        else
                        {
                            Byte[] dataTable = new UTF8Encoding(true).GetBytes(dataList[j] + dataList[j+1]+ '\t');
                            fs.Write(dataTable, 0, dataTable.Length);
                        }
                    }
                    fs.Write(newline, 0, newline.Length);
                    Console.WriteLine("Table created successfuly!");
                }
        }
        
        public static void DropTable(string text)
        {
            string[] splitted = Infrastructure.CustomCommands.SplitOnChar(text,' ');
            string fileName = Environment.CurrentDirectory+ "/" + splitted[1]+".marinata";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                Console.WriteLine("Table deleted successfuly!");
            }
            else
                Console.WriteLine("No such table exist");
        }

        public static void ListTables()
        {
            string folderPath = Environment.CurrentDirectory;
            string[] fileNames = Directory.GetFiles(folderPath);
            foreach (string fileName in fileNames)
            {
                if(fileName.EndsWith(".marinata"))
                    Console.WriteLine(fileName);
            }
        }
        
        public static void TableInfo(string tableName)
        {
            {
                string[] table = CustomCommands.SplitOnArr(tableName, _splitters);
                string path = Path.Combine(Environment.CurrentDirectory, table[1] + ".marinata");
                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    string text = File.ReadAllText(path);
                    string[] splitted = CustomCommands.SplitOnArr(tableName, _splitters);
                    
                    Console.WriteLine(text);
                    FileInfo fileInfo = new FileInfo(path);
                    Console.WriteLine("Table: " + splitted[1]);
                    Console.WriteLine("Number of records: " + (lines.Length - 1));
                    Console.WriteLine("Size of file: " + fileInfo.Length + " bytes");
                    Console.WriteLine("Creation time: " + fileInfo.CreationTime);
                }
                else
                {
                    Console.WriteLine("Table not found");
                }
            }
        }
        
        public static void Insert(string command)
        {
            // Split the command by space
            string[] words = command.Split(' ');
            
                // Get the table name and file name
                string tableName = words[2];
                string fileName = tableName + ".marinata";
                
                // Loop through the words in the command
                int dataCount = 0;
                List<string> dataList = new List<string>();
                for (int i = 0; i < words.Length; i++)              //Checks the splitted text for Null or empty, and if it contains commas
                {
                    string[] value = words[i].Split(_splitters);
                    for (int j = 0; j < value.Length; j++)          
                    {
                        if (!string.IsNullOrEmpty(value[j]))
                        {
                            if (value[j].Contains(','))
                            {
                                string converted = "";
                                for (int k = 0; k < value[j].Length; k++)
                                {
                                    if (value[j][k] != ',')
                                    {
                                        converted += value[j][k];
                                    }
                                }
                                dataList.Add(converted);
                            }
                            else
                            {
                                dataList.Add(value[j]);
                            }
                        }
                    }
                }
                int counter = 0;            //checks how many values are set in the command
                for (int i = 3; i < dataList.Count; i++)
                {
                    if (dataList[i] != "VALUES")
                    {
                        counter++;
                    }
                    else
                        break;
                }

                int c = 0;
                for (int i = 0; i < dataList.Count-c; i++) //removes empty strings
                {
                    if (dataList[i] == String.Empty)
                    {
                        dataList.RemoveAt(i);
                        c++;
                    }
                }
                string filePath = dataList[2] + ".marinata";
                for (int i = 3; i < dataList.Count-counter-1; i++) //converts the info to be more readable
                {
                    dataList[i] = dataList[i] +":"+ dataList[i + counter + 1];
                }
                for (int i = 0; i <= counter; i++)  //removes the unnecessary info
                {
                    dataList.RemoveAt(dataList.Count-1);
                }

                using (StreamReader sr = new StreamReader(fileName))
                {
                    var check = false;
                    string fr = sr.ReadLine();
                    string[] split = CustomCommands.SplitOnChar(fr,'\t');
                    string[] table;
                    char[] splitters = { ':', ' ' };
                    for (int i = 0; i < split.Length; i++)//checks if there are defaults in the file
                    {
                        if (split[i].Contains(' '))
                            check = true;
                    }
                    if (check==false&&split.Length>=counter)
                    {
                        Console.WriteLine("Not enough information");
                    }
                    else
                    {
                        for (int i = 0; i < split.Length; i++)
                        {
                            table = CustomCommands.SplitOnArr(split[i], splitters);
                            for (int j = 3+i; j < 3+i+1; j++)
                            {
                                if (j >= dataList.Count)
                                {
                                    if (split[i].Contains(' '))
                                        File.AppendAllText(filePath, table[table.Length-1] + '\t');
                                    break;
                                }
                                string[] data = CustomCommands.SplitOnArr(dataList[j], splitters);
                                if (table[0] == data[0])
                                {
                                    File.AppendAllText(filePath, data[data.Length-1] + '\t');
                                    break;
                                }
                                else
                                {
                                    if (split[i].Contains(' '))
                                        File.AppendAllText(filePath, table[table.Length-1] + '\t');
                                }
                            }
                        }
                        File.AppendAllText(filePath, Environment.NewLine);
                        Console.WriteLine("Table inserted succesfuly");
                    }
            }
        }
        
        static public int CalculateFileHash(string filePath)
        {
            string fileData = File.ReadAllText(filePath);

            int hash = 0;

            // Hash the contents of the file
            for (int i = 0; i < fileData.Length; i++)
            {
                // The modulus operator is used to keep the value of the hash within the range of an int
                hash += (hash * 31 + (fileData[i] * (i + 1))) % int.MaxValue;
            }

            return hash;
        }
        
        static public void WriteHashToFile(int hash, string filePath)
        {
            string outputFilePath = "hash.txt";

            // Create a string with the current file and hash
            string outputText = "File: " + filePath + "\nHash: " + hash;

            if (File.Exists(outputFilePath))
            {
                // If the output file already exists, read all the lines of the file into a string array
                string[] lines = File.ReadAllLines(outputFilePath);
                bool found = false;

                // Iterate through the array of lines and check if the line starts with the "File:" and input file path
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == "File: " + filePath)
                    {
                        // If it finds it, replace the next line with the new hash value
                        lines[i + 1] = "Hash: " + hash;
                        found = true;
                        break;
                    }
                }
                // If it does not find the file in the output file, append the new hash value and file name to the existing file
                if (!found)
                {
                    File.AppendAllText(outputFilePath, "\n" + outputText);
                }
                // If it found the file, write all the lines back to the file
                else
                {
                    File.WriteAllLines(outputFilePath, lines);
                }
            }
            // If the output file does not exist, create a new file and write the hash value and file name
            else
            {
                File.WriteAllText(outputFilePath, outputText);
            }
        }
        
        public static void CalculateHashesForFolder(string folderPath)
        {
            if (File.Exists("hash.txt") && File.ReadAllText("hash.txt") != "")
            {
                
                // Get a list of all files in the specified folder
                string[] files = Directory.GetFiles(folderPath);
                bool hashnotMatch = false;
                string[] lines = File.ReadAllLines("hash.txt");
                // Iterate through each file in the folder
                foreach (string file in files)
                {
                    // Calculate the hash of the current file
                    int currentHash = CalculateFileHash(file);
                    string formattedFile = string.Empty;
                    // Iterate through the array of lines
                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Check if the line is equal to the "File:" and input file path
                        if (lines[i] == "File: " + formattedFile)
                        {
                            // Compare the next line with the current hash
                            if (lines[i + 1] != "Hash: " + currentHash)
                            {
                                hashnotMatch = true;
                                break;
                            }
                        }
                    }

                }
                // If the hash does not match, shows an error
                if (hashnotMatch)
                {
                    // In a fully working case we would not allow the user to continue working
                    Console.WriteLine("Interruption Detected");
                }
            }
            else if(!File.Exists("hash.txt"))
                File.Create("hash.txt");

        }
    }
}