# LabyrinthGame 

LabyrinthGame is a game made with unity and arduino.

## Goal of the game

You must escape from the labyrinth ! To achieve this, find the three keys hidden within it. 
Then try to open each of the three doors to find the one that will lead you to the exit.

But beware ! You must escape within the time limit otherwise you will lose your chance to get out of this dark maze. 

Good luck ! 

## Equipment used

- Unity 
- Arduino Card
- A joystick to move the player, catch the hidden keys and open the doors of the labyrinth.

## Existant bugs 

###### Unity 

The collision between a wall and the player causes at certain times bugs on the player, which makes him move in a weird way. 
However, it is easy to regain control of the player when this happens.

###### Arduino

Data recovered from arduino isn't recovered instantly by unity. 
So, if we want to move the character for exemple, his movement will only be visible after ten seconds or more.
