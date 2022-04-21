# PrimeNumbers
A project whose sole purpose is to improve my basics of C# knowledge, which is done via solving some really simple task with technologies more and more advanced (thus naturally considered 'overkill', nonetheless helping my code skill). The die was cast for calculating prime numbers (within range of 2 nums). And down below is the list of stages I've planned:

- [X] Write a method to find all prime numbers within the range of 2 given numbers ***(done 11.04.2022)***
  - [X] Implement input of range ends
  - [X] Add checks to those inputs
  - [X] Make calculation method itself (simple trial division)
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
- [ ] Make the DB calls asynchronous with async/await
- [ ] Implement Repository / Unit of Work design pattern on a program
