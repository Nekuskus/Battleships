using System.Text;
public record struct Point(int X, int Y);
// Add turn counter, isPlayerTurn flipping, removePrintChecking argument, powerUps (artillery! hits 5 random fields; moveship! probably won't implement; gain a (1) ship!), traps (lose a turn!)

static class Extensions
{


    // Print extension methods

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

    // Public enums

    public enum BoxStatus
    {
        Unknown, // ' '
        ShipBox, // 'S' 
        ShotMiss, // '-'
        ShotHit // 'X'
    }

    public enum ShotReturn
    {
        ShotMissed,
        ShotHit
    }


    // Private fields

    private BoxStatus[,] _fieldPlayer;
    private BoxStatus[,] _fieldOpponent;


    // Public properties

    public BoxStatus[,] FieldPlayer
    {
        get => _fieldPlayer;
    }
    public BoxStatus[,] FieldOpponent
    {
        get => _fieldOpponent;
    }


    // Game initialization methods

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
    }
    public void PlaceShipOpponent(Point start, Point end)
    {
        if ((start.X == end.X && start.Y == end.Y) || (start.X != end.X && start.Y != end.Y))
            throw new ArgumentException("Ship start and end must align in one dimension.");

        if (start.X == end.X)
        {
            if (start.Y > end.Y) (start.Y, end.Y) = (end.Y, start.Y);
            for (int y = start.Y; y <= end.Y; y++)
            {
                _fieldOpponent[start.X, y] = BoxStatus.ShipBox;
            }
        }
        else
        { // if(start.Y == end.Y) {
            if (start.X > end.X) (start.X, end.X) = (end.X, start.X);
            for (int x = start.X; x <= end.X; x++)
            {
                _fieldOpponent[x, start.Y] = BoxStatus.ShipBox;
            }
        }
    }


    // Player methods

    public ShotReturn TryShoot(Point p)
    {
        if (_fieldOpponent[p.X, p.Y] == BoxStatus.ShipBox) {
            _fieldOpponent[p.X, p.Y] = BoxStatus.ShotHit;
	    return ShotReturn.ShotHit;
        } else {
            _fieldOpponent[p.X, p.Y] = BoxStatus.ShotMiss;
	    return ShotReturn.ShotMissed;
	}
    }

    public ShotReturn EnemyShot(Point p)
    {
	if (_fieldPlayer[p.X, p.Y] == BoxStatus.
ShipBox) {
            _fieldPlayer[p.X, p.Y] = BoxStatus.S
hotHit;
            return ShotReturn.ShotHit;
        } else {
            _fieldPlayer[p.X, p.Y] = BoxStatus.S
hotMiss;
            return ShotReturn.ShotMissed;
        }
    }


    // Constructor

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
