using Mastermind.Assets;

bool playing = true;

//Main game loop
while (playing)
{
    //Initialize game parameters
    Game session = new(10, 4, 1, 6);

    //Show Rules
    session.Intro();

    //Round loop
    while (session.Continue)
    {
        var guess = session.Begin_Round();
        session.End_Round(guess);
    }

    //Offer retry
    Game.Play_Again();
}

Environment.Exit(0);