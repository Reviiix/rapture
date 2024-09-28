# rapture
Made with Unity 2021.3.44f1 by Leon Michael Higgin on 27/10/2024 

A basic match game where the aim is to match 2 cards by choosing them concurrently from a selection of card.

I chose to use playing cards as the matching objects as I feel like this has more potential to be scaled.

I use my own sub repo called ‘unity-pure-functions’ which I keep updated with common unity functionality. This speeds up my development process by allowing me easy access to code I have already wrote before. Not all functionality in this sub module will be used but including it in no way effects performance

Project Initialiser:
Controls the order of execution of the other classes to avoid the possibility of race conditions.

AudioManager:
Creates a pool of audio sources on start and uses them on repeat with a rotating source of clips. This approach is more optimal than using multiple audio sources because it doesn't require as many audio sources or creating new ones on the fly.

GridManager:
Creates a grid based of values set in the editor. A tool that allows for easy level creation.

StateMachine:

Cards:

Score Tracker: