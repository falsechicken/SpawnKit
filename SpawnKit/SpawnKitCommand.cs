using System;
using System.Security;
using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using UnityEngine;
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
            get { return "A sample command";}
        }

        public void Execute(RocketPlayer caller, string command)
        {
        	string[] cmd = command.Split(null);
        	
        	bool isServer;
        	
        	bool isAdmin;
        	
        	string charName;
        	
        	try { charName = caller.CharacterName; isServer = false; isAdmin = caller.IsAdmin; } //Mainly to fix exceptions when user is typing commands from the server console.
        	catch (NullReferenceException n) { charName = "Server"; isServer = true; isAdmin = true; }
        	
        	if (cmd[0].Equals("set") && isAdmin) {
        		
        		if (cmd[1].Equals("cooldowntime")){ //set the spawn kit cooldown.
        			
        			try {
        				SpawnKit.SetCooldown(int.Parse(cmd[2]));
        				Logger.Log("Kit cooldown set to " + cmd[2] + " by " + charName);
        				return;
        			}
        			catch (Exception e )
        			{
        				Logger.Log("Not a valid input for cooldown. (Seconds)");
        				Logger.LogException(e);
        				return;
        			}
        		}

        		if (cmd[1].Equals("enabled")) { //Enable or disable plugin entirely.
        			
        			if (cmd[2].Equals("true")) {
        			    	SpawnKit.SetEnabled(true);
        			    	Logger.Log("Kits enabled" + " by " + charName);
        			    	return;
        			    }
        			if (cmd[2].Equals("false")) {
        			    SpawnKit.SetEnabled(false);
        			    Logger.Log("Kits disabled" + " by " + charName);
        			    return;
        			 }
        			
        			Logger.Log("Incorrect option. Only true or false is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].Equals("cooldown")) { //Turn off the cooldown.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(true);
        			    Logger.Log("Cooldown enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetGlobalCoolDownEnabled(false);
        			    Logger.Log("Cooldown disabled" + " by " + charName);
        			    return;
        			 }
        			
        			Logger.Log("Incorrect option. Only on or off is accepted.");
        			return;
        			
        		}
        		
        		if (cmd[1].Equals("cooldownmessages")) { //Turn off the cooldown chat messages.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(true);
        			    Logger.Log("Cooldown chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetCoolDownChatMessagesEnabled(false);
        			    Logger.Log("Cooldown chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			Logger.Log("Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].Equals("professionmessages")) { //Turn off the prefession chat messages.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(true);
        			    Logger.Log("Profession chat messages enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetProfessionChatMessagesEnabled(false);
        			    Logger.Log("Profession chat messages disabled" + " by " + charName);
        			    return;
        			 }
        			
        			Logger.Log("Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		if (cmd[1].Equals("professionmode")) { //Enable or disable profession mode.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetProfessionModeEnable(true);
        			    Logger.Log("Profession mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetProfessionModeEnable(false);
        			    Logger.Log("Profession mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			Logger.Log("Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        		    if (cmd[1].Equals("subscriptionmode")) { //Enable or disable subscription (class) mode.
        			
        			if (cmd[2].Equals("on")) {
        			    SpawnKit.SetSubscriptionMode(true);
        			    Logger.Log("Subscription mode enabled" + " by " + charName);
        			    return;
        			    }
        			if (cmd[2].Equals("off")) {
        			    SpawnKit.SetSubscriptionMode(false);
        			    Logger.Log("Subscription mode disabled" + " by " + charName);
        			    return;
        			 }
        			
        			Logger.Log("Incorrect option. Only on or off is accepted.");
        			return;
        		}
        		
        	}
        	
        	if (cmd[0].Equals("status") && isAdmin) { //View plugin status.
        		Logger.Log("-- SpawnKit Status --");
        		Logger.Log("- Plugin Enabled: " + SpawnKit.GetEnabled());
        		Logger.Log("- Subscription Mode Enabled: " + SpawnKit.GetSubscriptionModeEnabled());
        		Logger.Log("- Profession Mode Enabled: " + SpawnKit.GetProfessionModeEnabled());
        		Logger.Log("- Cooldown Enabled: " + SpawnKit.GetCooldownEnabled());
        		Logger.Log("- Cooldown Seconds: " + SpawnKit.GetCooldown());
        		Logger.Log("- Cooldown Chat Messages Enabled: " + SpawnKit.GetCoolDownMessagesEnabled());
        		Logger.Log("- Profession Chat Messaged Enabled: " + SpawnKit.GetProfessionChatMessagesEnabled());
        		return;
        	}
        	
        	if (cmd[0].Equals("list") && SpawnKit.GetSubscriptionModeEnabled()) { //List Classes if class subscription mode is on.
        	    	
        		string classListString = "";
        		
        		foreach (Kit k in SpawnKit.GetKitsList()){
        			
        			classListString = classListString + k.Name + ", ";
        		}
        		
        		RocketChatManager.Say("Class List: " + classListString);
        		RocketChatManager.Say("/sk class classname to pick class.");
        		
        	}
        	
        	if (cmd[0].Equals("class") && SpawnKit.GetSubscriptionModeEnabled()) { //Select class script
        		
        		try {
        			 
        			foreach (Kit k in SpawnKit.GetKitsList()) { //Loop through kits to see if they picked a valid class.
        				if (cmd[1].Equals(k.Name)) { //They did.
        					SpawnKit.AddToSubscriptionList(charName, k);
        					RocketChatManager.Say("Class selected " + charName);
        					return;
        				}

        			}
        			
        			RocketChatManager.Say("No such class " + charName);
        			return;
        			
        		}
        		catch (Exception e) {
        			RocketChatManager.Say("No such class " + charName);
        			return;
        		}
        		
        	}
        	
        	if (cmd[0].Equals("reload") && isAdmin == true) {
        		SpawnKit.ReloadConfiguration();
        	}
        	
        	
        }
    }
}