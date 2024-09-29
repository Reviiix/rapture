# rapture

Made with Unity 2021.3.44f1 by Leon Michael Higgin on 27/10/2024 && 29/10/2024.

A basic match game where the aim is to match 2 cards by choosing them concurrently from a selection of card.
The game is intended to be played with an aspect ratio of 16:9. It will dynamically scale to devices but this is the target ratio.
I chose to use playing cards as the matching objects as I feel like this has more potential to be scaled.
I use my own sub repo called ‘unity-pure-functions’ which I keep updated with common unity functionality. This speeds up my development process by allowing me easy access to code I have already wrote before. Not all functionality in this sub module will be used but including it in no way affects performance.

ProjectInitialiser:
Controls the order of execution of the other classes to avoid the possibility of race conditions.

Audio:
Creates a pool of audio sources on start and uses them on repeat with a rotating source of clips. This approach is more optimal than using multiple audio sources because it doesn't require as many audio sources or creating new ones on the fly.

Grid:
Creates a grid based of values set in the editor. The grid then assigns cards to the grid items on app start. Grid works in conjunction with GridRow and GridItem.

DeckOfCards:
Stores a reference to a standard deck of playing cards.

Evaluator:
Evaluates whether the last 2 picked cards match each other.

StateMachine:
Cycles through a queue of states in order. The menu state can skip the queue. This class controls the flow of the game: PickOne > PickTwo > Evaluate.

Score:
Tracks the users score in 3 parts, combo, turns and matches and then calculates a score based of that. ScoreDisplay scripts will subscribe to events in here to display accordingly.

Menu:
The menu system is a basic canvas that appears at the end of the game or when the user clicks the menu button. From here the user can either continue or restart the level.

Save System:
The save system has not been fully implemented because of time constraints but I have left in the code so you can see my intentions. It would save with either the Binary Formatter or JSON serialisation.