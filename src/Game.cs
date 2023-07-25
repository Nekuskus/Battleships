using System.Text;
public record struct Point(int X, int Y);

static class Extensions
{
    public static string Print(this Battleships.BoxStatus box)
    {
        if (box == Battleships.BoxStatus.Unknown) return " ";
        else if (box == Battleships.BoxStatus.ShipBox) return "S";
        else if (box == Battleships.BoxStatus.ShotMiss) return "-";
        else /*if(box == Battleships.BoxStatus.ShotHit)*/ return "X";
    }
    public static string Print<T>(this T[] array)
    {
        var sb = new StringBuilder("[ ");

        for (int i = 0; i < array.Length - 1; i++)
        {
            sb.Append(array[i].ToString() + ", ");
        }
        sb.Append(array[array.Length - 1].ToString());

        sb.Append(" ]");
        return sb.ToString();
    }
    public static string Print<T>(this T[,] array)
    {
        var sb = new StringBuilder("[ ");

        for (int i = 0; i < array.GetLength(0) - 1; i++)
        {
            sb.Append("[ ");
            for (int j = 0; j < array.GetLength(1) - 1; j++)
            {
                sb.Append(array[i, j].ToString() + ", ");
            }
            sb.Append(array[i, array.GetLength(1) - 1].ToString());
            sb.Append(" ], ");
        }

        sb.Append("[ ");
        for (int j = 0; j < array.GetLength(1) - 1; j++)
        {
            sb.Append(array[array.GetLength(0) - 1, j].ToString() + ", ");
        }
        sb.Append(array[array.GetLength(0) - 1, array.GetLength(1) - 1].ToString());
        sb.Append(" ] ");

        sb.Append(" ]");
        return sb.ToString();
    }
    public static string Print(this Battleships.BoxStatus[] array)
    {
        var sb = new StringBuilder("[ ");

        for (int i = 0; i < array.Length - 1; i++)
        {
            sb.Append(array[i].Print() + ", ");
        }
        sb.Append(array[array.Length - 1].Print());

        sb.Append(" ]");
        return sb.ToString();
    }
    public static string Print(this Battleships.BoxStatus[,] array)
    {
        var sb = new StringBuilder("[ ");

        for (int i = 0; i < array.GetLength(0) - 1; i++)
        {
            sb.Append("[ ");
            for (int j = 0; j < array.GetLength(1) - 1; j++)
            {
                sb.Append(array[i, j].Print() + ", ");
            }
            sb.Append(array[i, array.GetLength(1) - 1].Print());
            sb.Append(" ], ");
        }

        sb.Append("[ ");
        for (int j = 0; j < array.GetLength(1) - 1; j++)
        {
            sb.Append(array[array.GetLength(0) - 1, j].Print() + ", ");
        }
        sb.Append(array[array.GetLength(0) - 1, array.GetLength(1) - 1].Print());
        sb.Append(" ] ");

        sb.Append(" ]");
        return sb.ToString();
    }
}
class Battleships
{
    public enum BoxStatus
    {
        Unknown, // ' '
        ShipBox, // 'S' 
        ShotMiss, // '-'
        ShotHit // 'X'
    }


    private BoxStatus[,] _fieldPlayer;
    private BoxStatus[,] _fieldOpponent;
    public BoxStatus[,] FieldPlayer
    {
        get => _fieldPlayer;
    }
    public BoxStatus[,] FieldOpponent
    {
        get => _fieldOpponent;
    }

    public void PlaceShipPlayer(Point start, Point end)
    {
        if ((start.X == end.X && start.Y == end.Y) || (start.X != end.X && start.Y != end.Y))
            throw new ArgumentException("Ship start and end must align in one dimension.");

        if (start.X == end.X)
        {
            if (start.Y > end.Y) (start.Y, end.Y) = (end.Y, start.Y);
            for (int y = start.Y; y <= end.Y; y++)
            {
                _fieldPlayer[start.X, y] = BoxStatus.ShipBox;
            }
        }
        else
        { // if(start.Y == end.Y) {
            if (start.X > end.X) (start.X, end.X) = (end.X, start.X);
            for (int x = start.X; x <= end.X; x++)
            {
                _fieldPlayer[x, start.Y] = BoxStatus.ShipBox;
            }
        }
        
        Console.WriteLine(FieldPlayer.Print());
        Console.WriteLine(FieldOpponent.Print());
    }

    public Battleships(int x = 10, int y = 10)
    {
        _fieldPlayer = new BoxStatus[x, y];
        _fieldOpponent = new BoxStatus[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                _fieldPlayer[i, j] = BoxStatus.Unknown;
                _fieldOpponent[i, j] = BoxStatus.Unknown;
            }
        }
    }
}