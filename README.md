# PSNChecker
A PSN Username Checker written in C#


## Usage:
Download the files and compile them yourself, or download the binaries from [my site](https://tusticles.com/PSNChecker/)

The program itself requires one parameter and has one optional argument. For example:
`````
PSNChecker.exe path_to_wordlist.txt (optional)path_to_output.txt
`````
`path_to_wordlist.txt` is required, and this will be a txt file of all the PSN names you want to check. Each PSN should be on a seperate line.

`path_to_output.txt` is optional, and only needs to be used if you want to output each **valid** PSN to a new file after the program finishes.


I don't know if this API has a rate limiter on it, so if you try it out and you notice it blocks your requests after so many checks, please create a [new issue](https://github.com/Tustin/PSNChecker/issues) and I will work on a fix.
