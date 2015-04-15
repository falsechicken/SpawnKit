using System;
using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using fc.spawnkit;

namespace fc.spawnkit
{
    public class SpawnKitCommand : IRocketCommand
    {
        public bool RunFromConsole
        {
            get { return true; }
        }

        public string Name
        {
            get { return "sk"; }
        }

        public string Help
        {
            get { return "Root SpawnKit Command. Run 'sk help' for more info.";}
        }

        public void Execute(RocketPlayer caller, string command)
        {
        	string[] cmd = command.Split(null);
        	
        	bool isServer;
        	
        	bool isAdmin;
        	
        	string charName;
        	
        	try { charName = caller.CharacterName; isServer = false; isAdmin = caller.IsAdmin; } //Mainly to fix exceptions when user is typing commands from the server console.
        	catch (NullReferenceException n) { charName = "Server"; isServer = true; isAdmin = true; }
        	
        	if (cmd[0].Equals("set") && isAdmin) { //Set various settings.
        		
        		if (cmd.Length < 3) { //Make sure we have the right number or arguments.
        			SpawnKit.logMan.LogMessage(3, "Incorrect number of argumets for 'set'.");
        			return;
        		}
        		
        		if (cmd[1].Equals("enabled")) { //Enable or disable plugin entirely.
        			
        			if (cmd[2].Equals("true")) {
        			    	SpawnKit.SetEnabled(true);
        			    	SpawnKit.logMan.LogMessage(2, "Kits enabled" + " by " + charName);
        			    	return;
        			    }
        			if (cmd[2].Equals("false")) {
        			    SpawnKit.SetEnabled(false);
        			    SpawnKit.logMan.LogMessage(2, "Kits disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(2, "Incorrect option. Only true or false is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].Equals("cooldown")) { //Turn off the cooldown.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(true);
        			    SpawnKit.logMan.LogMessage(2, "Cooldown enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(false);
        			    SpawnKit.logMan.LogMessage(2, "Cooldown disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(2, "Incorrect option. Only on or off is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].Equals("cooldowntime")){ //set the spawn kit cooldown.
        			
        			try {
        				SpawnKit.SetCooldown(int.Parse(cmd[2]));
        				SpawnKit.logMan.LogMessage(2, "Kit cooldown set to " + cmd[2] + " by " + charName);
        				return;
        			}
        			catch (Exception e )
        			{
        				SpawnKit.logMan.LogMessage(2, "Not a valid input for cooldown. (Seconds)");
        				Logger.LogException(e);
        				return;
        			}
        		}
        		
        		if (cmd[1].Equals("cooldownmessages")) { //Turn off the cooldown chat messages.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(true);
        			    SpawnKit.logMan.LogMessage(2, "Cooldown chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(false);
        			    SpawnKit.logMan.LogMessage(2, "Cooldown chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(2, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].Equals("professionmode")) { //Enable or disable profession mode.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetProfessionModeEnable(true);
        			    SpawnKit.logMan.LogMessage(2, "Profession mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetProfessionModeEnable(false);
        			    SpawnKit.logMan.LogMessage(2, "Profession mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(2, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].Equals("professionmessages")) { //Turn off the prefession chat messages.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(true);
        			    SpawnKit.logMan.LogMessage(2, "Profession chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(false);
        			    SpawnKit.logMan.LogMessage(2, "Profession chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(2, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].Equals("subscriptionmode")) { //Enable or disable subscription (class) mode.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetSubscriptionMode(true);
        			    SpawnKit.logMan.LogMessage(2, "Subscription mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetSubscriptionMode(false);
        			    SpawnKit.logMan.LogMessage(2, "Subscription mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(2, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		SpawnKit.logMan.LogMessage(3, "Invalid set option.");
        		return;
        	}
        	
        	if (cmd[0].Equals("status") && isAdmin) { //View plugin status.
        		PrintStatus();
        		return;
        	}
        	
        	if (cmd[0].Equals("reload") && isAdmin) {
        		SpawnKit.ReloadConfiguration();
        		return;
        	}
        	
        	if (cmd[0].Equals("save") && isAdmin) { //TODO Doesnt work.
        		SpawnKit.SaveConfiguration();
        		return;
        	}
        	
        	if (cmd[0].Equals("givekit") && isAdmin) { //Clear the named players inventory and give them a kit.
        		SpawnKit.AdminGiveKit(cmd[1], cmd[2]);
        		return;
        	}
        	
        	if (cmd[0].Equals("list") && SpawnKit.GetSubscriptionModeEnabled()) { //List Classes if class subscription mode is on.
        	    	
        		string classListString = "";
        		
        		foreach (Kit k in SpawnKit.GetKitsList()){
        			
        			classListString = classListString + k.Name + ", ";
        		}
        		
        		SpawnKit.logMan.SayChat("Class List: " + classListString, EChatMode.SAY);
        		SpawnKit.logMan.SayChat("/sk class ClassName to pick class.", EChatMode.SAY);
        		return;
        		
        	}
        	
        	if (cmd[0].Equals("class") && SpawnKit.GetSubscriptionModeEnabled()) { //Select class script
        		
        		try {
        			 
        			foreach (Kit k in SpawnKit.GetKitsList()) { //Loop through kits to see if they picked a valid class.
        				if (cmd[1].Equals(k.Name)) { //They did.
        					SpawnKit.AddPlayerToSubscriptionList(charName, k);
        					SpawnKit.logMan.SayChat("Class selected " + charName, EChatMode.SAY);
        					return;
        				}

        			}
        			
        			SpawnKit.logMan.SayChat("No such class " + charName, EChatMode.SAY);
        			return;
        			
        		}
        		catch (Exception e) {
        			SpawnKit.logMan.SayChat("No such class " + charName, EChatMode.SAY);
        			return;
        		}
        		
        	}
        	
        	if (cmd[0].ToLower().Equals("help")) {
        	    	
        		if (cmd.Length == 1) { //If we are only showing the default help page.
        			ShowHelp("help");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("set")) {
        			ShowHelp("set");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("status")) {
        			ShowHelp("status");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("reload")) {
        			ShowHelp("reload");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("save")) {
        			ShowHelp("save");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("givekit")) {
        			ShowHelp("givekit");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("list")) {
        			ShowHelp("list");
        			return;
        		}
        		
        		SpawnKit.logMan.LogMessage(3, "Invalid help topic.");
        		return;
        	}
        	
        }
        
        /*
         * Print plugin status to the console.
         */
        private void PrintStatus() {
       		SpawnKit.logMan.LogMessage(2, "-- SpawnKit Status --");
        	SpawnKit.logMan.LogMessage(2, "- Plugin Enabled: " + SpawnKit.GetEnabled());
        	SpawnKit.logMan.LogMessage(2, "- Subscription Mode Enabled: " + SpawnKit.GetSubscriptionModeEnabled());
        	SpawnKit.logMan.LogMessage(2, "- Profession Mode Enabled: " + SpawnKit.GetProfessionModeEnabled());
        	SpawnKit.logMan.LogMessage(2, "- Cooldown Enabled: " + SpawnKit.GetCooldownEnabled());
        	SpawnKit.logMan.LogMessage(2, "- Cooldown Seconds: " + SpawnKit.GetCooldown());
        	SpawnKit.logMan.LogMessage(2, "- Cooldown Chat Messages Enabled: " + SpawnKit.GetCoolDownMessagesEnabled());
        	SpawnKit.logMan.LogMessage(2, "- Profession Chat Messages Enabled: " + SpawnKit.GetProfessionChatMessagesEnabled());
        }
        
        /*
         * Shows help topics to the console.
         */
        private void ShowHelp(string _topic) {
        	
        	if (_topic.ToLower().Equals("help")) {
        		SpawnKit.logMan.LogMessage(2, "-- SpawnKit Commands --");
        		SpawnKit.logMan.LogMessage(2, "* set - Change plugin settings. Admin only.");
        		SpawnKit.logMan.LogMessage(2, "* status - Display current plugin status. Admin only.");
        		SpawnKit.logMan.LogMessage(2, "* reload - Reload SpawnKit settings file. Admin only.");
        		SpawnKit.logMan.LogMessage(2, "* save - Save current settings into settings file. Admin only.");
        		SpawnKit.logMan.LogMessage(2, "* givekit - Clear players inventory and give kit. Admin only.");
        		SpawnKit.logMan.LogMessage(2, "* list - Shows a list of kits in game chat. sk permission..");
        		SpawnKit.logMan.LogMessage(2, "* class - Select a class. Will get upon death. sk permission.");
				SpawnKit.logMan.LogMessage(2, "-- Use 'sk help command' for more info about each. --");        		
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("set")) {
        		SpawnKit.logMan.LogMessage(2, "*-- SpawnKit Set Command Help --");
        		SpawnKit.logMan.LogMessage(2, "* - sk set <setting> <value> -");
        		SpawnKit.logMan.LogMessage(2, "* enabled - true/false. Is plugin enabled.");
        		SpawnKit.logMan.LogMessage(2, "* cooldown - on/off. Is kit cooldown enabled.");
        		SpawnKit.logMan.LogMessage(2, "* cooldowntime - seconds. Kit cooldown time length.");
        		SpawnKit.logMan.LogMessage(2, "* cooldownmessages - on/off. Chat cooldown messages.");
        		SpawnKit.logMan.LogMessage(2, "* professionmode - on/off. Random kit spawn profession mode.");
        		SpawnKit.logMan.LogMessage(2, "* professionmessages - on/off. Chat profession messages.");
        		SpawnKit.logMan.LogMessage(2, "* subscriptionmode - on/off. Kit subscription mode.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("status")) {
        		SpawnKit.logMan.LogMessage(2," -- SpawnKit Status Command Help --");
        		SpawnKit.logMan.LogMessage(2," * Usage: 'sk status' ");
        		SpawnKit.logMan.LogMessage(2," * Fuction: Shows current plugin settings.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("reload")) {
        		SpawnKit.logMan.LogMessage(2," -- SpawnKit Reload Command Help --");
        		SpawnKit.logMan.LogMessage(2," * Usage: 'sk reload' ");
        		SpawnKit.logMan.LogMessage(2," * Fuction: Reloads settings file. Changes made using");
        		SpawnKit.logMan.LogMessage(2,"   'set' command will be lost.");
        		return;
        	}        	
        	
        	if (_topic.ToLower().Equals("save")) {
        		SpawnKit.logMan.LogMessage(2," -- SpawnKit Save Command Help --");
        		SpawnKit.logMan.LogMessage(2," * Usage: 'sk save' ");
        		SpawnKit.logMan.LogMessage(2," * Fuction: Writes settings file using current settings.");
        		SpawnKit.logMan.LogMessage(2,"   (NON FUNCTIONAL!)");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("givekit")) {
        		SpawnKit.logMan.LogMessage(2," -- SpawnKit Givekit Command Help --");
        		SpawnKit.logMan.LogMessage(2," * Usage: 'sk givekit <player> <kit>' ");
        		SpawnKit.logMan.LogMessage(2," * Fuction: Clears a players inventory and gives them a kit.");
        		SpawnKit.logMan.LogMessage(2,"   The <player> field can take no spaces. Write the first few");
        		SpawnKit.logMan.LogMessage(2,"   letters of the players name. (Like admin in vanilla)");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("list")) {
        		SpawnKit.logMan.LogMessage(2," -- SpawnKit Givekit Command Help --");
        		SpawnKit.logMan.LogMessage(2," * Usage: 'sk givekit <player> <kit>' ");
        		SpawnKit.logMan.LogMessage(2," * Fuction: Clears a players inventory and gives them a kit.");
        		SpawnKit.logMan.LogMessage(2,"   The <player> field can take no spaces. Write the first few");
        		SpawnKit.logMan.LogMessage(2,"   letters of the players name. (Like admin in vanilla)");
        		return;
        	}
        	
        }
    }
}