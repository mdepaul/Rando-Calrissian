# Rando-Calrissian: C-Sharp command line utility.
## Use Rando to:
 * Get random quotes from Lando Calrissian
 * Generate keys, nonces, etc (hex; base64; safe base64)
 * Generate complex, configurable passwords
 * Pick random lines from a text file
 * Simulate rolling dice
 * Developers: See RandomizedList.cs for doing just what it sounds like.
  
## Useage
* Calrissian
  * Get random Star Wars quotes from BDW
    * ex: rando Calrissian
    * Break off the attack! The shield is still up!
    * Don't worry, my friend's down there. He'll have that shield down in time. Or this'll be the shortest offensive of all time.

* B = Number of bytes of output desired.
  * ex: 'rando b=12 e=h' will generate 12 random bytes, encoded in hexadecimal

* E = Encoding
  * Key Encodings
    * H=Hexadecimal
    * b64=Base64
    * sb64=Safe Base64. Replaces '/' with '.'; '+' with '_'; '==' with '--'
      * ex: 'rando b=12 e=h' will generate 12 random bytes, encoded in hexadecimal
      * ex: 'rando b=32 e=b64' will generate 32 random bytes, encoded in Base64
      * ex: 'rando b=64 e=sb64' will generate 64 random bytes, encoded in Safe Base64
* Password Encoding
  * ulds=Use Uppercase, Lowercase, Numbers and Special characters
    * e=ud means use only Uppercase and Digits
    * u/l/d/s=N to set a minimum number for a character set.
      * ex: ' rando e=uld b=12 u=5' means use Upper, Lower and Digits. Get 12 bytes of random values. Have a least 5 Upper case characters.
      * ex: 'rando b=32 e=ulds' means get 32 bytes, mixed with upper, lower, digits and specials
      * ex: 'rando b=32 e=ulds u=1 l=1 d=3 s=2' means get 32 bytes, mixed with upper, lower, digits and specials with a minimum of 1 upper, 1 lower, 3 digits and 2 specials.
* Dice mode
  * e=dice
  * Must include a listing of dice to 'roll.'
  * Valid dice a d4, d6, d10, d12& d20
  * ex: 'rando e=dice d6=1 d10=3 d4=5' means roll 1 d6, 3 d10's and 5 d4's. The sum of all dice rolls is returned.	

* F = File mode
  * Read the lines in the given file and return the value at a random line.
  * Good for a list of values where you want to select one at random, as in a drawing.
  * Use with top=N to select N lines from the list
  * Top=N means return N number of random lines from this file text
    * ex: 'rando f=c:\list.txt top=3' to get three random lines from C:\list.txt

* C = Clipboard
  * Copy the output to the clipboard for easy pasting.

* V = Verbose
  * Also output full switch settings
