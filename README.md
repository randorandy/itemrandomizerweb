# Super Metroid Item Randomizer for ROTATION

ROMs created with this tool must be rotated with strotlog's web tool here:
* https://strotlog.github.io/randoxrotation/

This is the casual logic version

* How I use:
* console: dotnet run
* browser: localhost:8888
* for now, use the "full" option when generating the rom for this logic (and it is full rando, not m/m)

Notes about this logic:
* The first four available locations that require nothing else are Morph, Ceiling E, Beta Missle, and 230 missile. Later morph can be made available with early screw.
* Basic hellruns can be required, but a screen's worth of lava requires varia and E.
* Acid always allows both suits to be available. This *should* never put a suit in acid, but I have seen unintended exceptions.
* Suitless Maridia consists of getting to Mama Turtle and Crab Supers with Hi Jump and no Gravity. For the rest of Maridia, Gravity will be available.
* Lower Norfair will always require Varia, Gravity, Space Jump, and Screw attack. Therefore, it is intended that those items will not appear in LN.
* Infinite bomb jumping is presumed, and can be an intended escape from rooms such as Phantoon. Underwater bomb jumping is not logically required.

Removed total's text from the readme
