using System.Text;
using System.Linq;
public record struct Point(int X, int Y);
// removePrintChecking argument, powerUps (artillery! hits 5 random fields; moveship! probably won't implement; gain a (1) ship!), traps (lose a turn!)

static partial class Extensions
{


    // Print extension methods

    public static string Print(this Battleships.BoxStatus box)
    {
        if (box == Battleships.BoxStatus.ShipBox) return "S";
        else if (box == Battleships.BoxStatus.ShotMiss) return "x";
        else if (box == Battleships.BoxStatus.ShotHit) return "X";
        else if (box == Battleships.BoxStatus.MineTrap) return "M";
        else if (box == Battleships.BoxStatus.Artillery) return "a";
        else if (box == Battleships.BoxStatus.Armageddon) return "A";
        else /*if (box == Battleships.BoxStatus.Unknown)*/ return " ";
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

        sb.Append("]");
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
        var sb = new StringBuilder("[");

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

    public static IEnumerable<T> ToEnumerable<T>(this Array target)
    {
        foreach (var item in target)
            yield return (T)item;
    }
}
class Battleships
{

    #region Public enums
    public enum BoxStatus
    {
        Unknown, // ' '
        ShipBox, // 'S' 
        ShotMiss, // 'x'
        ShotHit, // 'X'
        MineTrap, // 'M'
        Artillery, // 'a'
        Armageddon // 'A'
    }

    public enum ShotReturn
    {
        ShotMissed,
        ShotHit,
        PlayerWon,
        OpponentWon
    }

    #endregion

    #region Private fields

    private BoxStatus[,] _fieldPlayer;
    private BoxStatus[,] _fieldOpponent;
    private readonly bool _powerUps;
    private readonly bool _traps;
    private bool _isPlayerTurn;
    private uint _turnCounter;
    private readonly bool _isPlayerFirst;
    private readonly uint _fieldX;
    private readonly uint _fieldY;

    private bool _isGameOver;

    #endregion

    #region Public properties

    public BoxStatus[,] FieldPlayer
    {
        get => _fieldPlayer;
    }
    public BoxStatus[,] FieldOpponent
    {
        get => _fieldOpponent;
    }
    public bool IsPlayerTurn
    {
        private set => _isPlayerTurn = value;
        get => _isPlayerTurn;
    }
    public bool IsPlayerFirst
    {
        get => _isPlayerTurn;
    }
    public bool HasPowerUps
    {
        get => _powerUps;
    }
    public bool HasTraps
    {
        get => _traps;
    }
    public uint TurnCounter
    {
        private set => _turnCounter = value;
        get => _turnCounter;
    }
    public bool IsGameOver
    {
        private set => _isGameOver = value;
        get => _isGameOver;
    }
    public uint FieldX
    {
        get => _fieldX;
    }
    public uint FieldY
    {
        get => _fieldY;
    }
    public string PlayerName
    {
        get;
        set;
    }
    public string OpponentName
    {
        get;
        set;
    }

    #endregion

    #region Game initialization methods

    public void PlaceShipPlayer(Point start, Point end)
    {
        Validate(start);
        Validate(end);
        if (start.X != end.X && start.Y != end.Y)
            throw new ArgumentException("Ship start and end must align in at least one dimension.");

        if (IsGameOver) throw new InvalidOperationException("Ships cannot be placed if the game is over");
        
        if (start.X == end.X)
        {
            if (start.Y > end.Y) (start.Y, end.Y) = (end.Y, start.Y);
            for (int y = start.Y; y <= end.Y; y++)
            {
                _fieldPlayer[start.X, y] = BoxStatus.ShipBox;
            }
        }
        else // if(start.Y == end.Y)
        {    // no need to write a separate handler for (1) ships, since this loop works fine for them
            if (start.X > end.X) (start.X, end.X) = (end.X, start.X);
            for (int x = start.X; x <= end.X; x++)
            {
                _fieldPlayer[x, start.Y] = BoxStatus.ShipBox;
            }
        }
    }
    public void PlaceShipOpponent(Point start, Point end)
    {
        Validate(start);
        Validate(end);
        if (start.X != end.X && start.Y != end.Y)
            throw new ArgumentException("Ship start and end must align in at least one dimension.");

        if (IsGameOver) throw new InvalidOperationException("Ships cannot be placed if the game is over");

        if (start.X == end.X)
        {
            if (start.Y > end.Y) (start.Y, end.Y) = (end.Y, start.Y);
            for (int y = start.Y; y <= end.Y; y++)
            {
                _fieldOpponent[start.X, y] = BoxStatus.ShipBox;
            }
        }
        else // if(start.Y == end.Y)
        {    // no need to write a separate handler for (1) ships, since this loop works fine for them
            if (start.X > end.X) (start.X, end.X) = (end.X, start.X);
            for (int x = start.X; x <= end.X; x++)
            {
                _fieldOpponent[x, start.Y] = BoxStatus.ShipBox;
            }
        }
    }

    #endregion

    #region Player methods

    public ShotReturn TryShoot(Point p)
    {
        Validate(p);
        if (!IsPlayerTurn) throw new InvalidOperationException("A player can't move during the other player's turn.");
        if (IsGameOver) throw new InvalidOperationException("Shots cannot be made after the game is over.");

        if (_fieldOpponent[p.X, p.Y] == BoxStatus.ShipBox)
        {
            _fieldOpponent[p.X, p.Y] = BoxStatus.ShotHit;
            if (_endTurn()) return ShotReturn.ShotHit;
            else return ShotReturn.PlayerWon;
        }
        else
        {
            _fieldOpponent[p.X, p.Y] = BoxStatus.ShotMiss;
            if (_endTurn()) return ShotReturn.ShotMissed;
            else throw new InvalidOperationException("This code should not be reached. This means you won the game by not shooting.");
        }
    }

    public ShotReturn EnemyShot(Point p)
    {
        Validate(p);
        if (IsPlayerTurn) throw new InvalidOperationException("A player can't move during the other player's turn.");
        if (IsGameOver) throw new InvalidOperationException("Shots cannot be made after the game is over.");

        if (_fieldPlayer[p.X, p.Y] == BoxStatus.ShipBox)
        {
            _fieldPlayer[p.X, p.Y] = BoxStatus.ShotHit;
            if (_endTurn()) return ShotReturn.ShotHit;
            else return ShotReturn.OpponentWon;
        }
        else
        {
            _fieldPlayer[p.X, p.Y] = BoxStatus.ShotMiss;
            if (_endTurn()) return ShotReturn.ShotMissed;
            else throw new InvalidOperationException("This code should not be reached. This means your opponent won the game by not shooting.");
        }
    }

    private bool _endTurn()
    {
        // Only advance turn counter if the second player of the turn has just finished
        if (IsPlayerFirst && !IsPlayerTurn) _turnCounter += 1;
        else if (!IsPlayerFirst && IsPlayerTurn) _turnCounter += 1;

        if (IsPlayerTurn)
        {
            if (!Enumerable.Any(FieldOpponent.ToEnumerable<BoxStatus>(), (val) => val == BoxStatus.ShipBox))
            {
                // Player Win
                _endGame();
                return false;
            }
        }
        else
        {
            if (!Enumerable.Any(FieldPlayer.ToEnumerable<BoxStatus>(), (val) => val == BoxStatus.ShipBox))
            {
                // Opponent Win
                _endGame();
                return false;
            }
        }

        // Flip turn
        IsPlayerTurn = !IsPlayerTurn;
        return true;
    }

    private void _endGame()
    {
        IsGameOver = false;
    }

    
    #region PowerUp and Trap methods
    
    public string GetPlayerFieldString()
    {
        var sb = new StringBuilder();
        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        sb.AppendLine("   " + string.Join("", alphabet.Take(Convert.ToInt32(FieldX))));
        sb.AppendLine("  " + new String('-', (int)FieldX+2));
        for (int y = 0; y < FieldY; y++)
        {
            // Prepend line number and a space if needed. Breaks for lines above 99.
            sb.Append($"{(y+1).ToString()}{((y+1).ToString().Length == 1 ? " " : "")}|");
            for (int x = 0; x < FieldX; x++)
            {
                sb.Append(FieldPlayer[x, y].Print());
            }
            sb.AppendLine("|");
        }
        sb.Append("  " + new String('-', (int)FieldX+2));

        return sb.ToString();
    }

    public string GetOpponentFieldString()
    {

        var sb = new StringBuilder();
        
        sb.AppendLine(new String('-', (int)FieldX+2));
        for (int y = 0; y < FieldY; y++)
        {
            sb.Append('|');
            for (int x = 0; x < FieldX; x++)
            {
                sb.Append(FieldOpponent[x, y].Print());
            }
            sb.AppendLine("|");
        }
        sb.AppendLine(new String('-', (int)FieldX+2));

        return sb.ToString();
    }

    public void PrintPlayerField()
    {
        Console.WriteLine(GetPlayerFieldString());
    }

    public void PrintOpponentField()
    {
        Console.WriteLine(GetOpponentFieldString());
    }

    public void PlayerPlaceMineTrap(Point p)
    {
        if(!HasTraps) throw new InvalidOperationException("Players can't use traps in this session");
        Validate(p);
    }

    public void OpponentPlaceMineTrap(Point p)
    {
        if(!HasTraps) throw new InvalidOperationException("Players can't use traps in this session");
        Validate(p);
    }

    public void PlayerPlaceGainOne(Point p)
    {
        if(!HasPowerUps) throw new InvalidOperationException("Players can't use powerups in this session");
        Validate(p);
    }

    public void OpponentPlaceGainOne(Point p)
    {
        if(!HasPowerUps) throw new InvalidOperationException("Players can't use powerups in this session");
        Validate(p);
    }

    public void PlayerPlaceArtillery(Point p)
    {
        if(!HasPowerUps) throw new InvalidOperationException("Players can't use powerups in this session");
        Validate(p);
    }

    public void OpponentPlaceArtillery(Point p)
    {
        if(!HasPowerUps) throw new InvalidOperationException("Players can't use powerups in this session");
        Validate(p);
    }

    public void PlayerPlaceArmageddon(Point p)
    {
        if(!HasPowerUps) throw new InvalidOperationException("Players can't use powerups in this session");
        Validate(p);
    }

    public void OpponentPlaceArmageddon(Point p)
    {
        if(!HasPowerUps) throw new InvalidOperationException("Players can't use powerups in this session");
        Validate(p);
    }
    
    #endregion

    public void Validate(Point p)
    {
        if (p.X >= FieldX || p.X < 0) throw new ArgumentOutOfRangeException($"The point's X field {p.X} is out of bounds.");
        if (p.Y >= FieldY || p.Y < 0) throw new ArgumentOutOfRangeException($"The point's Y field {p.Y} is out of bounds.");
    }

    #endregion

    #region Constructor

    public Battleships(uint x = 10, uint y = 10, bool isPlayerFirst = true, bool powerUps = false, bool traps = false)
    {
        _fieldX = x;
        _fieldY = y;
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
        _isPlayerFirst = isPlayerFirst;
        _powerUps = powerUps;
        _traps = traps;

        _isGameOver = false;
    }

    #endregion
}
