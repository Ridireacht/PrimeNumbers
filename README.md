# PrimeNumbers
A learning project whose sole purpose is to improve my basics of C#. It is done via solving some simple task with technologies more and more advanced (thus naturally being an 'overkill', nonetheless helping my code skill and also being an original project idea). The die has been cast for calculating prime numbers within given range. Not really intended to be an ideal solution, but I'm trying.

Project contains two folders: one for the console version of the program, and the other for the GUI one. Both are fully functioning, and instead of compiling the code you can just download the latest release with binaries for both included.

____

<br/>
<details>
<summary>PLAN OF STAGES</summary>

<br/>

- [X] Write a method to find all prime numbers within the range of 2 given numbers ***(done 11.04.2022)***
  - [X] Implement input of range ends
  - [X] Add checks to those inputs
  - [X] Make calculation method itself (trial division)
  - [X] Add output of values calculated
- [X] Prove calculations to be valid, optimize the algorithm itself, fix security issues and simply enchance your program
  - [X] Prepare a function that compares calculated primes with the actual list of primes from an external file, while also counting number of primes checked and saves the last correct one ***(done 12.04.2022)***
  - [X] Optimize algorithm with more accurate range, even/uneven number handler and other stuff ***(done 13.04.2022 - 16.04.2022)***
  - [X] Embed a timer for algorithm and its results' verification function, so you could see their performance ***(done 13.04.2022)***
  - [X] Handle or fix existing exceptions, bugs and other problems, so your program could run stable ***(done 13.04.2022 - 14.04.2022)***
- [X] Optimize function with multi-threading ***(done 17.04.2022)***
  - [X] Implement multi-threading calculations themselves
  - [X] Make sure the right order of primes will be sustained during this
  - [X] Allow program to choose fastest method (of those two) for current range
- [X] Add some database to your program
  - [X] Make it save the calculation results in a database ***(done 18.04.2022 - 19.04.2022)***
  - [X] Use timer to mark the time of DB working ***(done 20.04.2022)***
  - [X] Add an option to either use DB during your calculations, or turn it off ***(done 20.04.2022)***
  - [X] Add an option to clear the DB, so it can be filled once again ***(done 20.04.2022)***
  - [X] If calculations were ever done once, use database's primes instead of recalculating numbers already known ***(done 20.04.2022 - 21.04.2022)***
- [X] Implement unit-tests for all the things added
  - [X] Tests for isPrime() ***(done 22.04.2022)***
  - [X] Tests for both SetByInput() ***(done 22.04.2022 - 23.04.2022)***
  - [X] Tests for DB calls ***(done 23.04.2022)***


<<<***some hiatus taken***>>>

- [X] Finish creating your tests ***(done 03.05.2022)***
    - [X] Mono-threading w/o DB
    - [X] Multi-threading w/o DB
    - [X] Mono-threading w/ DB
    - [X] Multi-threading w/ DB
- [X] Improve the console version and start developing the GUI one
  - [X] Split class methods where necessary (overwhelmed structure) ***(done 03.05.2022-04.05.2022)***
  - [X] Clean and optimize the code ***(done 03.05.2022)***
  - [X] Start work on the GUI version ***(done 05.05.2022-08.05.2022)***
- [X] Polish the console version, once again and finally
  - [X] Migrate project from .NET Framework 4.8 to .NET 6.0 ***(done 09.05.2022)***
  - [X] Split Function class into few separate ones ***(done 11.05.2022-13.05.2022)***
  - [X] Overhaul the main file structure to suit the changes and simplify the readability ***(done 13.05.2022)***
  - [X] Make according changes in the tests ***(done 14.05.2022-15.05.2022)***

</details>
