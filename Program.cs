using Course_Work.Infrastructure;
using Course_Work.backend;
namespace Course_Work{
    class Program
    {
        static char[] _splitters = {' ', '(', ':','"','”','“',')'};
        static string _folderPath = AppDomain.CurrentDomain.BaseDirectory;
        
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Write your command here:");
                string command = Console.ReadLine();
                string[] splitted = CustomCommands.SplitOnArr(command, _splitters);
                switch (splitted[0])
                {
                    case "CreateTable":
                        backend.Commands.CreateTable(command);
                        break;
                    case "DropTable":
                        backend.Commands.DropTable(command);
                        break;
                    case "ListTables":
                        backend.Commands.ListTables();
                        break;
                    case "Insert":
                        backend.Commands.WriteHashToFile(backend.Commands.CalculateFileHash(splitted[2]+".marinata"), splitted[2]+".marinata");
                        backend.Commands.CalculateHashesForFolder(_folderPath);
                        backend.Commands.Insert(command);
                        break;
                    case "TableInfo":
                        backend.Commands.TableInfo(command);
                        break;
                    case "Exit":
                        return;
                    default:
                        Console.WriteLine("Wrong input!");
                        break;
                }
            }
        }
    }
}
