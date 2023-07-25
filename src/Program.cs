class Program
{
    public static void Main(string[] args)
    {
        var game = new Battleships();
        game.PlaceShipPlayer(new Point(0, 3), new Point(9, 3));

        return;
    }
}