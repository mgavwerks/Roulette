# Roulette
After a trip to Vegas, i decided it would be fun to build a "game" in Crestron SIMPL and SIMPL+ language. I had already build a random number generator module in S+, so the challenging part was selecting "bets" and determining a win.

There are a few S+ modules built for this project: 1 - Random Number Generator: Simple picks a number between 0 and 37 (using 37 as "00" since i cannot chose "00" at random). 2 - Roulette Bets ( needs to be renamed): This is where the winning number is generated. There are several Digital Outs that help identify the number as Even, Odd, Black, Red, 1/2/3 Twelve, etc. 3 - Player Bets: This is where the roulette table stores the bets made by the player, manages the players' bank, and calculates winnings 4 - Button Toggle: Allows holding the placed bet buttons high until the wheel is spun, and then resets them.

Gameplay:

In the Player Bets S+ Module, there is a Structure declared that holds the selected bet type (straight, split, line, street, corner, etc.), the number(s) in that bet, and the value of that bet. There are 50 bets available via a BetIndex array. Selecting a button stores a bet in the structure (Bet Type, Number, Value), and advances through the bet array. Bets that store multiple numbers, store multiple bets.

On UI, there is the player's bank, which defaults to $1000 (you're welcome), a text input to set your bet value, and the total of the bets placed. Set the bet value before placing bets or else you will set bets with not bet amount (i will be updating that!). After you've place your bets, "spin" the wheel!

"Spinning The Wheel" uses a stepper to enable the vis of the animation, and change the message displayed to let you know a number is being chosen.

Once the stepper completes its steps, a number will be displayed in the Winning Number square.

Once that number is selected, it is compared to every entry in the Bet structure for matches. When it finds a match, its check the bet amount, multiplies it by the appropriate amount, and adds it to the payout. Once every bet is compared, the game displays a you payout, adds it to your bank, and clears your bets.

From there, its rinse and repeat.

Error Messages

There are currently two reasons an /programmed/ error message may be displayed on the UI: 1 - You are trying to place a bet higher than what you have available in your bank. 2 - You do not have any bets left.

If you get stuck because of any of these, or just in general, a Reset button is available to "reset" the game.

NOTE This is my first "game" built in Crestron (or in any format), that i am sharing publicly. I look forward to feedback and suggestion for improvement!
