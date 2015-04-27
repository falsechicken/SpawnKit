-- SpawnKit Plugin For Rocket --

Provides spawn kits, classes, and a few extra bells and whistles. - False_Chicken

Contact: jmdevsupport@gmail.com

Licensed under the GPLv2: https://www.gnu.org/licenses/gpl-2.0.txt

- Features -

* Spawn kits: (Obviously) Able to set a kit players get every time they spawn (With or without cooldown). 
	Or....

* Profession Mode: Randomized kits based on a spawn chance configurable in the settings xml file. Created to
	give servers a bit more realistic feel. Giving players the impression that their characters did something
	before the apocalypse other than standing naked on a beach...
	
* Subscription Mode: Think classes. Players can use the commands 'sk list' and 'sk class className' to pick
	a kit (Or class. If you think of it that way) that they will get every time they spawn. Useful in 
	deathmatch like scenarios.
	
* Live Configuration Modifications: All settings (Other than kit definitions) can be modified in game using
	the 'sk set <option> <argument>' command. Turn modes on and off, enable or disable cooldown, etc.. However
	these settings will not save to the config file. (Admin only)
	
Most of the documentation is in game using the 'sk help' command from the server console and in the example
configurations provided in the 'Example Config' folder. I like and am much better at writing code than
documentation.

WARNING: You will need to add the sk command to the Rocket Permissions.config.xml in order to let players use the sk command. If
not the command will do nothing for players.
