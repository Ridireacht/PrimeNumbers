using PrimeNumbers;

IO io = new IO();
Calculation c = new Calculation();


io.Input();
c.Set(io.a, io.b, io.isOutput, io.isDatabase);
c.Start();
IO.Output(c.primes);