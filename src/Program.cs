using System.IO;
record struct MenuItem(string text, ItemType type, string value);
enum ItemType { Switch, Numeric, Text, Empty, Exit }

static partial class Extensions
{
    public static string Print(this MenuItem item)
    {
        switch (item.type)
        {
            case ItemType.Switch:
                return item.text + ' ' + (item.value == "1" ? '✔' : '✘');
            case ItemType.Numeric:
                return item.text + ' ' + item.value;
            case ItemType.Text:
                return item.text + ' ' + item.value;
            case ItemType.Exit:
                return item.text;
            case ItemType.Empty:
                return "";
            default:
                return item.text;
        }
    }
}

public static class DefaultOptions
{
    public static string PlayerName = "Player";
    public static int Ships4 = 1;
    public static int Ships3 = 2;
    public static int Ships2 = 3;
    public static int Ships1 = 4;
    public static bool PowerUps = false;
    public static bool Traps = false;
    public static int MineCount = 0;
    public static int Gain1Count = 0;
    public static int ArtilleryCount = 0;
    public static int ArmageddonCount = 0;

    public static void Save()
    {
        using (var fs = new FileStream("options.ini", FileMode.Create))
        {
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine($"PlayerName={DefaultOptions.PlayerName}");
                sw.WriteLine($"Ships4={DefaultOptions.Ships4}");
                sw.WriteLine($"Ships3={DefaultOptions.Ships3}");
                sw.WriteLine($"Ships2={DefaultOptions.Ships2}");
                sw.WriteLine($"Ships1={DefaultOptions.Ships1}");
                sw.WriteLine($"PowerUps={DefaultOptions.PowerUps}");
                sw.WriteLine($"Traps={DefaultOptions.Traps}");
                sw.WriteLine($"MineCount={DefaultOptions.MineCount}");
                sw.WriteLine($"Gain1Count={DefaultOptions.Gain1Count}");
                sw.WriteLine($"ArtilleryCount={DefaultOptions.ArtilleryCount}");
                sw.WriteLine($"ArmageddonCount={DefaultOptions.ArmageddonCount}");
            }
        }
    }

    public static void Load()
    {
        using (var fs = new FileStream("options.ini", FileMode.Open))
        {
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine()!.Split('=');
                    switch (line[0])
                    {
                        case "PlayerName":
                            DefaultOptions.PlayerName = line[1];
                            break;
                        case "Ships4":
                            DefaultOptions.Ships4 = int.Parse(line[1]);
                            break;
                        case "Ships3":
                            DefaultOptions.Ships3 = int.Parse(line[1]);
                            break;
                        case "Ships2":
                            DefaultOptions.Ships2 = int.Parse(line[1]);
                            break;
                        case "Ships1":
                            DefaultOptions.Ships1 = int.Parse(line[1]);
                            break;
                        case "PowerUps":
                            DefaultOptions.PowerUps = (line[1] == "1" ? true : false);
                            break;
                        case "Traps":
                            DefaultOptions.Traps = (line[1] == "1" ? true : false);
                            break;
                        case "MineCount":
                            DefaultOptions.MineCount = int.Parse(line[1]);
                            break;
                        case "Gain1Count":
                            DefaultOptions.Gain1Count = int.Parse(line[1]);
                            break;
                        case "ArtilleryCount":
                            DefaultOptions.ArtilleryCount = int.Parse(line[1]);
                            break;
                        case "ArmageddonCount":
                            DefaultOptions.ArmageddonCount = int.Parse(line[1]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}

class Program
{
    public static void ExitGame()
    {
        DefaultOptions.Save();
        Console.WriteLine("Saved configuration. Exiting...");
        Environment.Exit(0);
    }
    public static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.WriteLine("Battleships v0.01");
        if (File.Exists("options.ini")) DefaultOptions.Load();
        MainMenu();
    }

    public static (MenuItem[] options, int selected) CreateMenu(MenuItem[] options, int initialPos = 0)
    {
        Console.CursorVisible = false;
        if (!(initialPos < options.Length)) throw new ArgumentOutOfRangeException("Initial cursor position out of bounds.");


        int offset = initialPos;
        // Write options
        for (int i = 0; i < options.Length - 1; i++)
        {
            Console.WriteLine(' ' + options[i].Print());
        }
        Console.Write(' ' + options[options.Length - 1].Print());

        // Create cursor
        Console.CursorTop = Console.CursorTop - options.Length + 1 + offset;
        Console.CursorLeft = 0;
        Console.Write('>');

        // Move cursor
        var key = Console.ReadKey(false).Key;

        while (key != ConsoleKey.Enter || options[offset].type != ItemType.Exit)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.CursorVisible = false;
                    if (offset - 1 >= 0)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(' ');
                        if (offset - 2 >= 0 && options[offset - 1].type == ItemType.Empty)
                        {
                            Console.SetCursorPosition(0, Console.CursorTop - 2);
                            offset -= 2;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            offset -= 1;
                        }
                        Console.Write('>');
                    }
                    break;
                case ConsoleKey.DownArrow:
                    Console.CursorVisible = false;
                    if (offset + 1 < options.Length)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(' ');
                        if (offset + 2 < options.Length && options[offset + 1].type == ItemType.Empty)
                        {
                            Console.SetCursorPosition(0, Console.CursorTop + 2);
                            offset += 2;
                        }
                        else
                        {
                            Console.SetCursorPosition(0, Console.CursorTop + 1);
                            offset += 1;
                        }
                        Console.Write('>');
                    }
                    break;
                case ConsoleKey.RightArrow:
                    Console.CursorVisible = false;
                    if (options[offset].type == ItemType.Numeric)
                    {
                        Console.CursorLeft = 1;
                        Console.Write(new String(' ', Console.BufferWidth - 2));
                        Console.CursorLeft = 1;
                        options[offset].value = (int.Parse(options[offset].value) + 1).ToString();
                        Console.Write(options[offset].Print());
                    }
                    else if (options[offset].type == ItemType.Text)
                    {
                        Console.CursorVisible = true;
                        // move cursor here but only within bounds after options[offset].text and up to the length of the current string saved in the value, up to 20 chars.
                        // save the value too by inserting characters at the proper offset
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (options[offset].type == ItemType.Numeric)
                    {
                        Console.CursorVisible = false;
                        Console.CursorLeft = 1;
                        Console.Write(new String(' ', Console.BufferWidth - 2));
                        Console.CursorLeft = 1;
                        options[offset].value = (int.Parse(options[offset].value) - 1).ToString();
                        Console.Write(options[offset].Print());

                    }
                    else if (options[offset].type == ItemType.Text)
                    {
                        Console.CursorVisible = true;
                        // move cursor here but only within bounds after options[offset].text and up to the length of the current string saved in the value, up to 20 chars.
                        // save the value too by inserting characters at the proper offset
                    }
                    break;
                case ConsoleKey.Enter:
                    Console.CursorVisible = false;
                    if (options[offset].type == ItemType.Switch)
                    {
                        options[offset].value = (options[offset].value == "0" ? "1" : "0");
                        Console.CursorLeft = 1;
                        Console.Write(options[offset].Print());
                        Console.CursorLeft = 0;
                    }
                    break;
                default:
                    if (options[offset].type == ItemType.Text)
                    {
                        Console.CursorVisible = true;
                        //handle typing here and limit max characters to console width - some offset for var name, spaces and cursor

                        //this is placeholder code:
                        Console.CursorLeft = 1;
                        Console.Write(options[offset].Print());
                    }
                    else
                    {
                        Console.CursorVisible = false;
                        Console.CursorLeft = 1;
                        Console.Write(options[offset].Print());
                    }
                    break;
            }
            key = Console.ReadKey(false).Key;
        }

        // Clean Menu
        Console.CursorLeft = 0;
        Console.CursorTop -= offset;
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine(new String(' ', Console.BufferWidth));
        }
        Console.CursorTop -= (offset + 1);

        Console.WriteLine($"Selected option {offset + 1}: {options[offset].text}");


        return (options, offset);
    }

    //if Point is null then menu was exited, try again
    public static Point? CreateSelectFieldBoxMenu(ref Battleships game)
    {
        Console.CursorVisible = false;
        // TOOD: if pressed ESC: print "Exiting box select menu", return null.
        string board = game.GetPlayerFieldString();

        // Prepend board with proper indentation and symbols
        string[] boardArr = board.Split('\n');
        for (var i = 0; i < boardArr.Length; i++)
        {
            boardArr[i] = string.Join("", boardArr[i].Prepend(' ').Prepend(' '));
        }
        boardArr = boardArr.Prepend("\n").ToArray();

        // Print the board
        board = string.Join('\n', boardArr);
        Console.Write(board);

        // Print lower cursor
        Console.CursorLeft = 1;
        Console.CursorTop = Console.WindowHeight - boardArr.Length + 3;
        Console.Write('>');

        // Print upper cursor
        Console.CursorLeft = 5;
        Console.CursorTop = Console.WindowHeight - boardArr.Length;
        Console.Write('V');
        int offsetLower = 0;
        int offsetUpper = 0;

        // Render cursors for both dimensions (initial position is always (0,0) == (A, 1))
        void RemoveUpperCursor(ref Battleships g)
        {
            Console.CursorTop = Console.WindowHeight - boardArr.Length;
            Console.CursorLeft = 5 + offsetUpper;
            Console.Write(new String(' ', Convert.ToInt32(g.FieldX)));
        }

        void RemoveLowerCursor(ref Battleships g)
        {
            for(int i = 0; i < g.FieldY; i++)
            {
                Console.CursorTop = Console.WindowHeight - boardArr.Length + 3 + i;
                Console.CursorLeft = 1;
                Console.Write('>');
            }
        }

        // Move cursor
        var key = Console.ReadKey(false).Key;
        Console.CursorVisible = false;

        while (key != ConsoleKey.Escape || key != ConsoleKey.Enter)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (offsetUpper - 1 >= 0)
                    {
                        Console.CursorVisible = false;
                        RemoveUpperCursor(ref game);
                        offsetUpper -= 1;
                        Console.CursorTop = Console.WindowHeight - boardArr.Length;
                        Console.CursorLeft = 5 + offsetUpper;
                        Console.Write('V');
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (offsetUpper + 1 < 4 + game.FieldX)
                    {
                        Console.CursorVisible = false;
                        RemoveUpperCursor(ref game);
                        offsetUpper += 1;
                        Console.CursorTop = Console.WindowHeight - boardArr.Length;
                        Console.CursorLeft = 5 + offsetUpper;
                        Console.Write('V');
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (offsetLower - 1 >= 0)
                    {
                        Console.CursorVisible = false;
                        RemoveLowerCursor(ref game);
                        offsetLower -= 1;
                        Console.CursorTop = Console.WindowHeight - boardArr.Length + 3 + offsetLower;
                        Console.CursorLeft = 1;
                        Console.Write('>');
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (offsetLower + 1 >= game.FieldY - 1)
                    {
                        Console.CursorVisible = false;
                        RemoveLowerCursor(ref game);
                        offsetLower += 1;
                        Console.CursorTop = Console.WindowHeight - boardArr.Length + 3 + offsetLower;
                        Console.CursorLeft = 1;
                        Console.Write('>');
                    }
                    break;
            }
            key = Console.ReadKey(false).Key;
        }
        if (key == ConsoleKey.Enter)
            return new Point(offsetLower, offsetUpper);
        else return null;
    }
    public static void CreateShipPlaceMenu(ref Battleships game, int shipLength)
    {
        if (!(shipLength < game.FieldX && shipLength < game.FieldY))
            throw new ArgumentOutOfRangeException("Ship does not fit on the field in either direction.");

        Console.WriteLine($"Ship placement, length {shipLength}\n");

        Point? startPoint = CreateSelectFieldBoxMenu(ref game);

        if (startPoint == null)
        {
            Console.WriteLine("Exiting ship placement menu.");
            return;
        }

    }
    public static void MainMenu()
    {
        int initPos = 0;
    repeatmainmenu:
        Console.WriteLine("Main menu");
        Console.Write("\n\n\n");
        var options = new MenuItem[] {
            new MenuItem("Start game", ItemType.Exit, "0"),
            new MenuItem("Default options", ItemType.Exit, "0"),
            new MenuItem("Exit game", ItemType.Exit, "0")
        };
        (var newOptions, var selected) = CreateMenu(options, initPos);

        if (selected == 0)
        {
            GameCreationMenu();
        }
        else if (selected == 1)
        {
            DefaultOptionsMenu();
        }
        else if (selected == 2)
        {
            ExitGame();
        }

        initPos = selected;
        goto repeatmainmenu;
    }

    public static void DefaultOptionsMenu()
    {
        Console.CursorVisible = false;
        Console.WriteLine("Default options");
        Console.Write("\n\n\n");
        var options = new MenuItem[] {
            new MenuItem("PlayerName", ItemType.Text, DefaultOptions.PlayerName),
            new MenuItem("Ships4", ItemType.Numeric, DefaultOptions.Ships4.ToString()),
            new MenuItem("Ships3", ItemType.Numeric, DefaultOptions.Ships3.ToString()),
            new MenuItem("Ships2", ItemType.Numeric, DefaultOptions.Ships2.ToString()),
            new MenuItem("Ships1", ItemType.Numeric, DefaultOptions.Ships1.ToString()),
            new MenuItem("PowerUps", ItemType.Switch, DefaultOptions.PowerUps.ToString()),
            new MenuItem("Traps", ItemType.Switch, DefaultOptions.Traps.ToString()),
            new MenuItem("MineCount", ItemType.Numeric, DefaultOptions.MineCount.ToString()),
            new MenuItem("Gain1Count", ItemType.Numeric, DefaultOptions.Gain1Count.ToString()),
            new MenuItem("ArtilleryCount", ItemType.Numeric, DefaultOptions.ArtilleryCount.ToString()),
            new MenuItem("ArmageddonCount", ItemType.Numeric, DefaultOptions.ArmageddonCount.ToString()),
            new MenuItem("", ItemType.Empty, "0"),
            new MenuItem("Save & Exit", ItemType.Exit, "0"),
        };
        (var newOptions, var selected) = CreateMenu(options);

        //save to options.ini
        DefaultOptions.PlayerName = newOptions[0].value;
        DefaultOptions.Ships4 = int.Parse(newOptions[1].value);
        DefaultOptions.Ships3 = int.Parse(newOptions[2].value);
        DefaultOptions.Ships2 = int.Parse(newOptions[3].value);
        DefaultOptions.Ships1 = int.Parse(newOptions[4].value);
        DefaultOptions.PowerUps = (newOptions[5].value == "1" ? true : false);
        DefaultOptions.Traps = (newOptions[6].value == "1" ? true : false);
        DefaultOptions.MineCount = int.Parse(newOptions[7].value);
        DefaultOptions.Gain1Count = int.Parse(newOptions[8].value);
        DefaultOptions.ArtilleryCount = int.Parse(newOptions[9].value);
        DefaultOptions.ArmageddonCount = int.Parse(newOptions[10].value);

        DefaultOptions.Save();
    }

    public static void GameCreationMenu()
    {
        Console.CursorVisible = false;
        var game = new Battleships();
        CreateSelectFieldBoxMenu(ref game);
    }
}