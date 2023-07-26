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
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Battleships v0.01");
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
                        Console.Write(new String(' ', Console.BufferWidth-1));
                        Console.CursorLeft = 1;
                        options[offset].value += 1;
                        Console.Write(options[offset].Print());
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (options[offset].type == ItemType.Numeric)
                    {
                        Console.CursorLeft = 1;
                        Console.Write(new String(' ', Console.BufferWidth-1));
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

        Console.WriteLine($"Selected option {offset+1}: {options[offset].text}");


        Console.CursorVisible = true;

        return (options, offset);
    }

    public static void MainMenu()
    {
        Console.WriteLine("Main menu");
        Console.Write("\n\n\n\n\n");
        var options = new MenuItem[] {
            new MenuItem("Start game", ItemType.Exit, 0),
            new MenuItem("Default options", ItemType.Exit, 0),
            new MenuItem("Exit game", ItemType.Exit, 0)
        };
        var selected = CreateMenu(options);



    }

    public static void DefaultOptionsMenu()
    {
        //save to options.ini
    }

    public static void GameCreationMenu()
    {

    }
}