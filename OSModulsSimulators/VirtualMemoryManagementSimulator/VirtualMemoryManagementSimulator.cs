namespace VirtualMemoryManagementSimulator
{
    using System;
    using System.Text;
    class VirtualMemoryManagementSimulator
    {
        static void Main()
        {
            int memorySize = 5;
            MemoryManager memoryManger = new MemoryManager(memorySize);
            StringBuilder menu = new StringBuilder();
            menu.AppendLine("\t\t Memory Manger - Least Recently Used strategy.");
            menu.AppendLine("Enter command. 1 - To load a procces into memory");
            menu.AppendLine("2 - To access frame.");
            menu.AppendLine("3 - To show memory content.");
            menu.AppendLine("4 - For exit.");

            bool exitCommand = false;
            while (!exitCommand)
            {
                //Print menu
                Console.WriteLine(menu.ToString());
                Console.Write("Enter command: ");
                int inputCommand = 0;
                while (!int.TryParse(Console.ReadLine(), out inputCommand) || inputCommand <= 0 || inputCommand > 4)
                {
                    Console.Write("Invalid command! Enter again, but valid command: ");
                }
                switch (inputCommand)
                {
                    case 1: memoryManger.LoadProcess("Process 1"); Console.WriteLine("Process loaded."); break; 
                    case 2: 
                        {
                            Console.Write("Enter index of the frame: ");
                            int frameIndex = 0;
                            
                            while (!int.TryParse(Console.ReadLine(), out frameIndex) || frameIndex < 0 || frameIndex >= memorySize)
                            {
                                Console.Write("Invalid index! Enter again, but valid index: ");
                            }
                        } break;
                    case 3: Console.WriteLine(memoryManger.ToString()); break;
                    case 4: exitCommand = true; break;
                }
            }
            //memoryManger.LoadProcess("Process 1");
            //System.Threading.Thread.Sleep(2000);
            //memoryManger.LoadProcess("Process 2");
            //System.Threading.Thread.Sleep(2000);
            //memoryManger.LoadProcess("Process 3");
            //System.Threading.Thread.Sleep(2000);
            //memoryManger.LoadProcess("Process 4");
            //System.Threading.Thread.Sleep(2000);
            //memoryManger.LoadProcess("Process 5");

            //memoryManger.AccessFrame(0);
            //memoryManger.AccessFrame(1);
            //Console.WriteLine(memoryManger.ToString());

            //memoryManger.LoadProcess("Process 6");

            //Console.WriteLine(memoryManger.ToString());

        }
            
    }
}
