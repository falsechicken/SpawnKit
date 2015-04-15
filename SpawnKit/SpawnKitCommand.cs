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
        		
        		else {
        			SpawnKit.logMan.LogMessage(3, "Incorrect set option.");
        			return;
        		}
        		
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
        	
        	if (cmd[0].Equals("help") {
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
        
        private void ShowHelp(string _topic) {
        	
        }
    }
}