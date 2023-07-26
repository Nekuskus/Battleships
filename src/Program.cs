class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Battleships v0.01");
        MainMenu();
    }

    public static int CreateMenu(string[] options)
    {
        Console.CursorVisible = false;
        int offset = 0;
        // Write options
        for (int i = 0; i < options.Length-1; i++)
        {
            Console.WriteLine(' ' + options[i]);
        }
        Console.Write(' ' + options[options.Length-1]);

        // Create cursor
        Console.CursorTop = Console.CursorTop - options.Length + 1;
        Console.CursorLeft = 0;
        Console.Write('>');

        // Move cursor
        var key = Console.ReadKey(false).Key;

        while (key != ConsoleKey.Enter)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (offset - 1 >= 0)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(' ');
                        Console.SetCursorPosition(0, Console.CursorTop-1);
                        Console.Write('>');
                        offset -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (offset + 1 < options.Length)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(' ');
                        Console.SetCursorPosition(0, Console.CursorTop+1);
                        Console.Write('>');
                        offset += 1;
                    }
                    break;
                default:
                    break;
            }
            key = Console.ReadKey(false).Key;
        }

        // Clean Menu
        Console.CursorTop -= offset;
        for(int i = 0; i < options.Length; i++) {
            Console.WriteLine(new String(' ', 20));
        }
        Console.CursorTop -= (offset + 1);

        Console.WriteLine($"Selected option {offset}: {options[offset]}");

        
        Console.CursorVisible = true;

        return offset;
    }

    public static void MainMenu()
    {
        Console.WriteLine("Main menu");
        Console.Write("\n\n\n\n\n");
        var options = new string[] {"Start game", "Default options", "Exit game"};
        var selected = CreateMenu(options);
        
        

    }

    public static void GameCreationMenu()
    {

    }
}