using PrimeNumbers;

int a = 0, b = 0;
bool isOutput = false, isDatabase = false, isCorrect = false;

DB db = new DB();
Calculation c = new Calculation();



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



c.Set(a, b, isOutput, isDatabase);
c.Start();

if (isOutput)
    IO.Output(c.primes);
c.Verify(c.primes);

DB.ClearDatabase();