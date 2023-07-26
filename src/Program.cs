using System.IO;
record struct MenuItem(string text, ItemType type, int value);
enum ItemType { Switch, Numeric, Exit }

static partial class Extensions
{
    public static string Print(this MenuItem item)
    {
        switch (item.type)
        {
            case ItemType.Switch:
                return item.text + ' ' + (item.value == 1 ? '✔' : '✘');
            case ItemType.Numeric:
                return item.text + ' ' + item.value;
            case ItemType.Exit:
                return item.text;
            default:
                return item.text;
        }
    }
}

public static class DefaultOptions
{
    public static string? PlayerName = null;
    public static int? Ships4 = null;
    public static int? Ships3 = null;
    public static int? Ships2 = null;
    public static int? Ships1 = null;
    public static bool? PowerUps = null;
    public static bool? Traps = null;
    public static int? MineCount = null;
    public static int? Gain1Count = null;
    public static int? ArtilleryCount = null;
    public static int? ArmageddonCount = null;

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
                while(!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split('=');
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
                            DefaultOptions.PowerUps = bool.Parse(line[1]);
                            break;
                        case "Traps":
                            DefaultOptions.Traps = bool.Parse(line[1]);
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
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Battleships v0.01");
        if(File.Exists("options.txt")) DefaultOptions.Load();
        MainMenu();
    }

    public static (MenuItem[] options, int selected) CreateMenu(MenuItem[] options)
    {
        Console.CursorVisible = false;
        int offset = 0;
        // Write options
        for (int i = 0; i < options.Length - 1; i++)
        {
            Console.WriteLine(' ' + options[i].Print());
        }
        Console.Write(' ' + options[options.Length - 1].Print());

        // Create cursor
        Console.CursorTop = Console.CursorTop - options.Length + 1;
        Console.CursorLeft = 0;
        Console.Write('>');

        // Move cursor
        var key = Console.ReadKey(false).Key;

        while (key != ConsoleKey.Enter || options[offset].type != ItemType.Exit)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (offset - 1 >= 0)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(' ');
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        Console.Write('>');
                        offset -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (offset + 1 < options.Length)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(' ');
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                        Console.Write('>');
                        offset += 1;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (options[offset].type == ItemType.Numeric)
                    {
                        Console.CursorLeft = 1;
                        Console.Write(new String(' ', Console.BufferWidth - 1));
                        Console.CursorLeft = 1;
                        options[offset].value += 1;
                        Console.Write(options[offset].Print());
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (options[offset].type == ItemType.Numeric)
                    {
                        Console.CursorLeft = 1;
                        Console.Write(new String(' ', Console.BufferWidth - 1));
                        Console.CursorLeft = 1;
                        options[offset].value -= 1;
                        Console.Write(options[offset].Print());

                    }
                    break;
                case ConsoleKey.Enter:
                    if (options[offset].type == ItemType.Switch)
                    {
                        options[offset].value = (options[offset].value == 0 ? 1 : 0);
                        Console.CursorLeft = 1;
                        Console.Write(options[offset].Print());
                        Console.CursorLeft = 0;
                    }
                    break;
                default:
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


        Console.CursorVisible = true;

        return (options, offset);
    }

    public static void MainMenu()
    {
    repeatmainmenu:
        Console.WriteLine("Main menu");
        Console.Write("\n\n\n");
        var options = new MenuItem[] {
            new MenuItem("Start game", ItemType.Exit, 0),
            new MenuItem("Default options", ItemType.Exit, 0),
            new MenuItem("Exit game", ItemType.Exit, 0)
        };
        (var newOptions, var selected) = CreateMenu(options);

        if(selected == 0)
        {
            GameCreationMenu();
        }
        else if (selected == 1)
        {
            DefaultOptionsMenu();
        } else if (selected == 2)
        {
            ExitGame();
        }
        goto repeatmainmenu;
    }

    public static void DefaultOptionsMenu()
    {
        //save to options.ini
    }

    public static void GameCreationMenu()
    {

    }
}