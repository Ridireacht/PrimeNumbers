# PrimeNumbers
A project whose sole purpose is to improve my basics of C# knowledge, which is done via solving some really simple task with technologies more and more advanced (thus naturally considered 'overkill', nonetheless helping my code skill). The die was cast for calculating prime numbers (within range of 2 nums). And down below is the list of stages I've planned:

- [X] Write a method to find all prime numbers within the range of 2 given numbers ***(done 11.04.2022)***
  - [X] Implement input of range ends
  - [X] Add checks to those inputs
  - [X] Make calculation method itself (simple trial division)
  - [X] Add output of values calculated
- [ ] Prove calculations to be valid, optimize the algorithm itself, fix security issues and simply enchance your program
  - [X] Prepare a function that compares calculated primes with the actual list of primes from an external file, while also counting number of primes checked and saves the last correct one ***(done 12.04.2022 - 13.04.2022)***
  - [X] Optimize algorithm with more accurate range, even/uneven number handler and other stuff ***(done 13.04.2022)***
  - [X] Embed a timer for algorithm and its results' verification function, so you could see their performance ***(done 13.04.2022)***
  - [ ] Add try {} catch blocks to handle possible exceptions and just generally fix existing bugs
- [ ] Optimize function with multi-threading
- [ ] Make it save the result in a database, and if calculations were ever done once, make it use data from database instead of recalculating numbers already known
- [ ] Make the DB calls asynchronous with async/await
- [ ] Implement Repository / Unit of Work design pattern on a program
