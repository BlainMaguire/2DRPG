2D RPG

Written in C# using XNA. It has the basic functionality needed for an old school RPG game.

Works across multiple platforms including Windows, Xbox and Zune.

Features:

* Scrolling camera using tiles. Support for multiple layers and differnt tile sizes etc.
* Walking around maps: animation, collision detection, doors from one map to another
* Loading maps from a file (with relevant information like what texture to use, what tiles you can walk on)
* Very simple turn based battle system - you attack and do damage, enemy attacks and does damage.


Minimum Requirements:

This game runs fine on an old Zune (with less than 14mb of ram).

Possible Issues:

The assets (.jpg, .mp3, etc) need to go through the content pipeline to be converted to a format XNA can read. Failing to do that will result in a runtime error. You need Windows and Visual Studio to do this. If you are having problems doing this and just want to run the game I can send you ones I created.

Game was originally written for XNA 2.0, subsequently upgraded to 3.1. I haven't been keeping up with the latest version but I imagine it should be fine. With a few lines of code changed I was able to get it to run on Linux using MonoGame. However, you will still need Windows for the content pipeline though which is why I've just put up the windows version here with an Visual Studio solution file to make that easier). I haven't merged the MonoGame fork yet as currently it's doing file IO in a very PC like way just to get it to run (which means runtime errors on more restricted platforms like Zune or Xbox).

Although the game runs fine on ZuneHD, my code doesn't listen for touch events (as it was written before the ZuneHD came out). The ZuneHD doesn't have a directional pad or action buttons so that's why you can't actually do anything.
