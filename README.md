# PrimeNumbers
## Summary
A learning project whose sole purpose is to improve my basics of C#. It is done via solving some simple task with technologies more and more advanced (thus naturally being an 'overkill', nonetheless helping my code skill and also being an original project idea). The die has been cast for calculating prime numbers within given range. Not really intended to be an ideal solution, but I'm trying.

</br>

## How to use
The application calculates all prime numbers in a given range. The range limits are any natural numbers greater than two. Calculation can be done using three methods: mono-threaded, multi-threaded, and the automatic (selected by default, it chooses between the first and second mode depending on the range). All prime numbers calculated can be displayed in the "output" window on the left (first checkbox option), and/or saved to a separate text file ("Save results to the file" button). 

The program also offers to use a database for calculations (second checkbox option). When checked, the app will will create a database and save the results of calculations there. These database records will be used in the further calculations, as app won't need to recalculate values already known and saved (thus speeding up the calculations). Unchecking this option will prevent any operations with a database, be it saving results or using its content during calculations. And "Clear database" button might be used to wipe all the data stored in a database.

The app also comes out with the "verification list" - a list of prime numbers already pre-calculated. It contains all the prime numbers in range from 2 to 15485863. So you could prove your calculations to be right. Any prime numbers calculated above the end range will still be calculated, yet not checked.

</br>

## Project structure
* <b>Calculator.cs</b> - all sort of calculations
* <b>DB.cs</b> - interactions with database
* <b>IO.cs</b> - handling input (of range ends) and output (of primes calculated)
* <b>Window.cs</b> - handling all GUI stuff

Based on <b>.NET 6</b>.

GUI made on <b>WinForms</b>.

<b>SQLite</b> used as database
