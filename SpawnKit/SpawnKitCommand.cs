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
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect number of arguments for 'set'.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("enabled")) { //Enable or disable plugin entirely.
        			
        			if (cmd[2].ToLower().Equals("true")) {
        			    	SpawnKit.SetEnabled(true);
        			    	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Kits enabled" + " by " + charName);
        			    	return;
        			    }
        			if (cmd[2].ToLower().Equals("false")) {
        			    SpawnKit.SetEnabled(false);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Kits disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect option. Only true or false is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].ToLower().Equals("cooldown")) { //Turn off the cooldown.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(true);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Cooldown enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(false);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Cooldown disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].ToLower().Equals("cooldowntime")){ //set the spawn kit cooldown.
        			
        			try {
        				SpawnKit.SetCooldown(int.Parse(cmd[2]));
        				SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Kit cooldown set to " + cmd[2] + " by " + charName);
        				return;
        			}
        			catch (Exception e )
        			{
        				SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Not a valid input for cooldown. (Seconds)");
        				Logger.LogException(e);
        				return;
        			}
        		}
        		
        		if (cmd[1].ToLower().Equals("cooldownmessages")) { //Turn off the cooldown chat messages.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(true);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Cooldown chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(false);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Cooldown chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("professionmode")) { //Enable or disable profession mode.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetProfessionModeEnable(true);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Profession mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetProfessionModeEnable(false);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Profession mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("professionmessages")) { //Turn off the prefession chat messages.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(true);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Profession chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(false);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Profession chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].ToLower().Equals("subscriptionmode")) { //Enable or disable subscription (class) mode.
        			
        			if (cmd[2].ToLower().Equals("on")) {
        			    SpawnKit.SetSubscriptionMode(true);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Subscription mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].ToLower().Equals("off")) {
        			    SpawnKit.SetSubscriptionMode(false);
        			    SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Subscription mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Invalid set option.");
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
        			
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Incorrect number of arguments for 'givekit'.");
        		return;

        	}
        	
        	if (cmd[0].ToLower().Equals("list")) { //List Classes if class subscription mode is on.
        	    	
        		if (SpawnKit.GetSubscriptionModeEnabled()) { //If subscription mode is enabled print list.
        		
        			string classListString = "";
        			
        			foreach (Kit k in SpawnKit.GetKitsList()){
        				
        				classListString = classListString + k.Name + ", ";
        			}
        			
        			RocketChatManager.Say(caller, "Class List: " + classListString);
        			RocketChatManager.Say(caller, "/sk class ClassName to pick class.");
        			return;
        		}
        		
        		RocketChatManager.Say(caller, "Subscription mode disabled.");
        		return;
        		
        	}
        	
        	if (cmd[0].ToLower().Equals("class")) { //Select class
        		
        		if (SpawnKit.GetSubscriptionModeEnabled()) { //If subscription mode is enabled.
        			try {
        				foreach (Kit k in SpawnKit.GetKitsList()) { //Loop through kits to see if they picked a valid class.
        					if (cmd[1].ToLower().Equals(k.Name.ToLower())) { //They did.
        						SpawnKit.AddPlayerToSubscriptionList(charName, k);
        						RocketChatManager.Say(caller, "Class selected.");
        						return;
	        				}
        				}
    	    			RocketChatManager.Say(caller, "No such class.");
	        			return;
        			}
        			catch (Exception e) {
        				RocketChatManager.Say(caller, "No such class.");
        				return;
        			}
        		}
        		
        		RocketChatManager.Say(caller, "Subscription mode disabled.");
        		return;
        		
        	}
        	
        	if (cmd[0].ToLower().Equals("listsubs") && isAdmin) { //Print the player kit subscriptions to the console.
        		
        		if (SpawnKit.GetSubscriptionModeEnabled()) {
        		
        			var tempSubList = SpawnKit.GetSubscriptionList();
        			SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "-- Kit Subscriptions --");
        		
        			foreach (var pName in SpawnKit.GetSubscriptionList().Keys){
        				SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Player: "+ pName + " | Kit: " + tempSubList[pName].Name);
        			}
        			return;
        		}
        		
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Subscription mode disabled.");
        		
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
        		
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Invalid help topic.");
        		return;
        	}
        	
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Invalid SpawnKit command. Use 'sk help' for more info.");
        	return;
        	
        	
        }
        
        /*
         * Print plugin status to the console.
         */
        private void PrintStatus() {
       		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "-- SpawnKit Status --");
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Plugin Enabled: " + SpawnKit.GetEnabled());
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Subscription Mode Enabled: " + SpawnKit.GetSubscriptionModeEnabled());
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Profession Mode Enabled: " + SpawnKit.GetProfessionModeEnabled());
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Cooldown Enabled: " + SpawnKit.GetCooldownEnabled());
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Cooldown Seconds: " + SpawnKit.GetCooldown());
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Cooldown Chat Messages Enabled: " + SpawnKit.GetCoolDownMessagesEnabled());
        	SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "- Profession Chat Messages Enabled: " + SpawnKit.GetProfessionChatMessagesEnabled());
        }
        
        /*
         * Shows help topics to the console.
         */
        private void ShowHelp(string _topic) {
        	
        	if (_topic.ToLower().Equals("help")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "-- SpawnKit Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* set - Change plugin settings. Admin only.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* status - Display current plugin status. Admin only.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* reload - Reload SpawnKit settings file. Admin only.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* save - Save current settings into settings file. Admin only.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* givekit - Clear players inventory and give kit. Admin only.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* listsubs - List players subscribed to kits. Admin only.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* list - Shows a list of kits in game chat. sk permission..");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* class - Select a class. Will get upon death. sk permission.");
				SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "-- Use 'sk help <command>' for more info about each. --");        		
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("set")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "*-- SpawnKit Set Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* Usage: sk set <setting> <value> -");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* enabled - true/false. Is plugin enabled.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* cooldown - on/off. Is kit cooldown enabled.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* cooldowntime - Seconds. Kit cooldown time length.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* cooldownmessages - on/off. Chat cooldown messages.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* professionmode - on/off. Random kit spawn profession mode.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* professionmessages - on/off. Chat profession messages.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "* subscriptionmode - on/off. Kit subscription mode.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("status")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit Status Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk status' ");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Shows current plugin settings.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("reload")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit Reload Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk reload' ");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Reloads settings file. Changes made using");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   'set' command will be lost.");
        		return;
        	}        	
        	
        	if (_topic.ToLower().Equals("save")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit Save Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk save' ");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Writes settings file using current settings.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("givekit")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit Givekit Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk givekit <player> <kit>' ");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Clears a players inventory and gives them a kit.");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   The <player> field can take no spaces. Write the first few");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   letters of the players name. (Like admin in vanilla)");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("list")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit List Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk list' ");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Prints available kits to players. If");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   subscriptionMode is off an error is shown.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("class")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit Class Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk class <classname>' ");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Subscribes a player to a class/kit. The");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   player will receive the kit when they die or after");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   the cooldown expires and they die while cooldown is");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO,"   on.");
        		return;
        	}
        	
        	if (_topic.ToLower().Equals("listsubs")) {
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," -- SpawnKit Listsubs Command Help --");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Usage: 'sk listsubs'");
        		SpawnKit.logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO," * Fuction: Lists players subscribed to kits.");
        		return;
        	}
        	
        }
    }
}