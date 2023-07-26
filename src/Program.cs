class Program
{
    public static void Main(string[] args)
    {
        var game = new Battleships();
        game.PlaceShipPlayer(new Point(0, 3), new Point(9, 3));
        Console.WriteLine(game.FieldOpponent.Print());
        Console.WriteLine(game.FieldPlayer.Print());
        Console.WriteLine(game.FieldPlayer.Print<Battleships.BoxStatus>());
        Console.WriteLine(game.FieldOpponent.Print<Battleships.BoxStatus>());
        return;
    }
}