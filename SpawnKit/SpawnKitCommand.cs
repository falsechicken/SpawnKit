/*****
 * SpawnKitCommand - Provides the command functionality of SpawnKit.
 * 
 * Copyright (C) 2015 False_Chicken
 * Contact: jmdevsupport@gmail.com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, Get it here: https://www.gnu.org/licenses/gpl-2.0.html
 *****/
 
using System;
using Rocket.Logging;
using Rocket.RocketAPI;
using messageLevels = FC.LogMan.MessageLevels;

namespace FC.SpawnKit
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
        	
        	if (cmd[0].ToLower().Equals("set") && isAdmin) { //Set various settings.
        		
        		if (cmd.Length < 3 || cmd.Length > 3) { //Make sure we have the right number or arguments.
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect number of arguments for 'set'.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("enabled")) { //Enable or disable plugin entirely.
        			
        			if (cmd[2].ToLower().Equals("true")) {
        			    	SpawnKit.SetEnabled(true);
        			    	SpawnKit.logMan.LogMessage(messageLevels.INFO, "Kits enabled" + " by " + charName);
        			    	return;
        			    }
        			if (cmd[2].ToLower().Equals("false")) {
        			    SpawnKit.SetEnabled(false);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Kits disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect option. Only true or false is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].ToLower().Equals("cooldown")) { //Turn off the cooldown.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(true);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Cooldown enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(false);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Cooldown disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].ToLower().Equals("cooldowntime")){ //set the spawn kit cooldown.
        			
        			try {
        				SpawnKit.SetCooldown(int.Parse(cmd[2]));
        				SpawnKit.logMan.LogMessage(messageLevels.INFO, "Kit cooldown set to " + cmd[2] + " by " + charName);
        				return;
        			}
        			catch (Exception e )
        			{
        				SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Not a valid input for cooldown. (Seconds)");
        				Logger.LogException(e);
        				return;
        			}
        		}
        		
        		if (cmd[1].ToLower().Equals("cooldownmessages")) { //Turn off the cooldown chat messages.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(true);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Cooldown chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(false);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Cooldown chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("professionmode")) { //Enable or disable profession mode.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetProfessionModeEnable(true);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Profession mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetProfessionModeEnable(false);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Profession mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("professionmessages")) { //Turn off the prefession chat messages.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(true);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Profession chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(false);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Profession chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("subscriptionmode")) { //Enable or disable subscription (class) mode.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetSubscriptionMode(true);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Subscription mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetSubscriptionMode(false);
        			    SpawnKit.logMan.LogMessage(messageLevels.INFO, "Subscription mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Invalid set option.");
        		return;
        	}
        	
        	if (cmd[0].ToLower().Equals("status") && isAdmin) { //View plugin status.
        		PrintStatus();
        		return;
        	}
        	
        	if (cmd[0].ToLower().Equals("reload") && isAdmin) {
        		SpawnKit.ReloadConfiguration();
        		return;
        	}
        	
        	if (cmd[0].ToLower().Equals("save") && isAdmin) {
        		SpawnKit.SaveConfiguration();
        		return;
        	}
        	
        	if (cmd[0].ToLower().Equals("givekit") && isAdmin) { //Clear the named players inventory and give them a kit.
        		
        		if (cmd.Length == 3) { //Make sure we have to proper number of parameters.
        			SpawnKit.AdminGiveKit(cmd[1], cmd[2]);
        			return;
        		}
        			
        		SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Incorrect number of arguments for 'givekit'.");
        		return;

        	}
        	
        	if (cmd[0].ToLower().Equals("list")) { //List Classes if class subscription mode is on.
        	    	
        		if (SpawnKit.GetSubscriptionModeEnabled()) { //If subscription mode is enabled print list.
        		
        			string classListString = "";
        			
        			foreach (Kit k in SpawnKit.GetKitsList()){
        				
        				classListString = classListString + k.Name + ", ";
        			}
        			
        			SpawnKit.logMan.SayChatToPlayer(caller, "Class List: " + classListString);
        			SpawnKit.logMan.SayChatToPlayer(caller, "/sk class ClassName to pick class.");
        			return;
        		}
        		
        		SpawnKit.logMan.SayChatToPlayer(caller, "Subscription mode disabled.");
        		return;
        		
        	}
        	
        	if (cmd[0].ToLower().Equals("class")) { //Select class
        		
        		if (SpawnKit.GetSubscriptionModeEnabled()) { //If subscription mode is enabled.
        			try {
        				foreach (Kit k in SpawnKit.GetKitsList()) { //Loop through kits to see if they picked a valid class.
        					if (cmd[1].ToLower().Equals(k.Name.ToLower())) { //They did.
        						SpawnKit.AddPlayerToSubscriptionList(charName, k);
        						SpawnKit.logMan.SayChatToPlayer(caller, "Class selected.");
        						return;
	        				}
        				}
    	    			SpawnKit.logMan.SayChatToPlayer(caller, "No such class.");
	        			return;
        			}
        			catch (Exception e) {
        				SpawnKit.logMan.SayChatToPlayer(caller, "No such class.");
        				return;
        			}
        		}
        		
        		SpawnKit.logMan.SayChatToPlayer(caller, "Subscription mode disabled.");
        		return;
        		
        	}
        	
        	if (cmd[0].ToLower().Equals("listsubs") && isAdmin) { //Print the player kit subscriptions to the console.
        		
        		if (SpawnKit.GetSubscriptionModeEnabled()) {
        		
        			var tempSubList = SpawnKit.GetSubscriptionList();
        			SpawnKit.logMan.LogMessage(messageLevels.INFO, "-- Kit Subscriptions --");
        		
        			foreach (var pName in SpawnKit.GetSubscriptionList().Keys){
        				SpawnKit.logMan.LogMessage(messageLevels.INFO, "Player: "+ pName + " | Kit: " + tempSubList[pName].Name);
        			}
        			return;
        		}
        		
        		SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Subscription mode disabled.");
        		
        		return;
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
        		
        		if (cmd[1].ToLower().Equals("listsubs")) {
        			ShowHelp("listsubs");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("class")) {
        			ShowHelp("class");
        			return;
        		}
        		
        		SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Invalid help topic.");
        		return;
        	}
        	
        	SpawnKit.logMan.LogMessage(messageLevels.WARNING, "Invalid SpawnKit command. Use 'sk help' for more info.");
        	return;
        	
        	
        }
        
        /*
         * Print plugin status to the console.
         */
        private void PrintStatus() {
       		SpawnKit.logMan.LogMessage(messageLevels.INFO, "-- SpawnKit Status --");
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Plugin Enabled: " + SpawnKit.GetEnabled());
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Subscription Mode Enabled: " + SpawnKit.GetSubscriptionModeEnabled());
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Profession Mode Enabled: " + SpawnKit.GetProfessionModeEnabled());
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Cooldown Enabled: " + SpawnKit.GetCooldownEnabled());
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Cooldown Seconds: " + SpawnKit.GetCooldown());
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Cooldown Chat Messages Enabled: " + SpawnKit.GetCoolDownMessagesEnabled());
        	SpawnKit.logMan.LogMessage(messageLevels.INFO, "- Profession Chat Messages Enabled: " + SpawnKit.GetProfessionChatMessagesEnabled());
        }
        
        /*
         * Shows help topics to the console.
         */
        private void ShowHelp(string _topic) {
        	
        	if (_topic.ToLower().Equals("help")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "-- SpawnKit Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* set - Change plugin settings. Admin only.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* status - Display current plugin status. Admin only.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* reload - Reload SpawnKit settings file. Admin only.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* save - Save current settings into settings file. Admin only.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* givekit - Clear players inventory and give kit. Admin only.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* listsubs - List players subscribed to kits. Admin only.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* list - Shows a list of kits in game chat. sk permission..");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* class - Select a class. Will get upon death. sk permission.");
				SpawnKit.logMan.LogMessage(messageLevels.INFO, "-- Use 'sk help <command>' for more info about each. --");        		
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("set")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "*-- SpawnKit Set Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* Usage: sk set <setting> <value> -");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* enabled - true/false. Is plugin enabled.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* cooldown - on/off. Is kit cooldown enabled.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* cooldowntime - Seconds. Kit cooldown time length.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* cooldownmessages - on/off. Chat cooldown messages.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* professionmode - on/off. Random kit spawn profession mode.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* professionmessages - on/off. Chat profession messages.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO, "* subscriptionmode - on/off. Kit subscription mode.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("status")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit Status Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk status' ");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Shows current plugin settings.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("reload")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit Reload Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk reload' ");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Reloads settings file. Changes made using");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   'set' command will be lost.");
        		return;
        	}        	
        	
        	if (_topic.ToLower().Equals("save")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit Save Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk save' ");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Writes settings file using current settings.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("givekit")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit Givekit Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk givekit <player> <kit>' ");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Clears a players inventory and gives them a kit.");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   The <player> field can take no spaces. Write the first few");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   letters of the players name. (Like admin in vanilla)");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("list")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit List Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk list' ");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Prints available kits to players. If");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   subscriptionMode is off an error is shown.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("class")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit Class Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk class <classname>' ");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Subscribes a player to a class/kit. The");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   player will receive the kit when they die or after");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   the cooldown expires and they die while cooldown is");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO,"   on.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("listsubs")) {
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," -- SpawnKit Listsubs Command Help --");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Usage: 'sk listsubs'");
        		SpawnKit.logMan.LogMessage(messageLevels.INFO," * Fuction: Lists players subscribed to kits.");
        		return;
        	}
        	
        }
    }
}