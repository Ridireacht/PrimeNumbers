using PrimeNumbers;
using System.Diagnostics;

int a = 0, 
    b = 0;

bool isOutput = false, 
    isDatabase = false, 
    isCorrect = false,
    isToBeCleared = false;


DB db = new DB();
Calculation c = new Calculation();
Stopwatch timer = new();



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



if (isDatabase)
{
    DB.CreateDatabase();
    DB.GetFromDatabase(ref c.primes, a, b);
}

c.Set(a, b);


timer = Stopwatch.StartNew();
c.GetPrimes(ref c.primes);
timer.Stop();


if (isOutput)
    IO.Output(c.primes);
c.Verify(c.primes);

if (isCorrect && isDatabase && c.primes.Any())
{
    timer = Stopwatch.StartNew();

    DB.FillDatabase(c.primes);

    timer.Stop();
    Console.WriteLine($"\n\nDatabase operations took {timer.ElapsedMilliseconds}ms\n");

    DB.ClearDatabase();
}

IO.SetByInput(ref isToBeCleared, "we fully clear DB");
if (isToBeCleared)
    DB.ClearDatabase();

#if !DEBUG
    Console.ReadKey();
#endif