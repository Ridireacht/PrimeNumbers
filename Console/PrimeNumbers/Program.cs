using PrimeNumbers;
using System.Diagnostics;


// global vars
int a = 0, 
    b = 0;

bool isOutput = false, 
     isDatabase = false, 
     isCorrect = false,
     isToBeCleared = false;

// global objs
DB db = new();
Calculator c = new();

Stopwatch timer = new();
List<int> primes = new();



{
    Input();
    Set();
    Calculate();
    Output();
    Check();
    ManageDB();

    Console.WriteLine("\nPress any button to close the program...");

    // save window from pre-closing
    #if !DEBUG
        Console.ReadKey();
    #endif
}



void Input()
{
    Console.WriteLine("Both of your range ends have to be >= 2.\n");

    // getting range ends
    IO.SetByInput(ref a, "Input first value: ");
    IO.SetByInput(ref b, "Input second value: ");

    // swap their if it's incorrect (using tuples)
    if (a > b)
        (a, b) = (b, a);

    // getting conditions
    IO.SetByInput(ref isOutput, "there be an output of calculated primes");
    IO.SetByInput(ref isDatabase, "the program use DB");
}

void Set()
{
    if (isDatabase)
    {
        DB.CreateDatabase();
        DB.GetFromDatabase(ref primes, a, b);
    }

    c.SetEnds(a, b);
}

void Calculate()
{
    timer = Stopwatch.StartNew();
    c.GetPrimes(ref primes);
    timer.Stop();
}

void Output()
{
    if (isOutput)
        IO.Output(primes);
    Console.WriteLine($"\n\nCalculations took {timer.ElapsedMilliseconds}ms");
}

void Check()
{
    timer = Stopwatch.StartNew();
    isCorrect = c.IsCorrect(primes);
    timer.Stop();
    Console.WriteLine($"\nVerification took {timer.ElapsedMilliseconds}ms");
}

void ManageDB()
{
    if (isCorrect && isDatabase && primes.Any())
    {
        timer = Stopwatch.StartNew();

        DB.FillDatabase(primes);

        timer.Stop();
        Console.WriteLine($"\n\nDatabase operations took {timer.ElapsedMilliseconds}ms\n");

        IO.SetByInput(ref isToBeCleared, "we fully clear DB");
        if (isToBeCleared)
            DB.ClearDatabase();
    }
}