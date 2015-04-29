/*****
 * SpawnKit - Rocket Unturned Server Wrapper plugin providing spawn kits, randomized kits, and more.
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
using System.Collections.Generic;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using UnityEngine;
using fc.logman;

namespace fc.spawnkit
{
	public class SpawnKit : RocketPlugin<SpawnKitConfiguration>
    {
		#region HOT SWAP SETTINGS VARS
		
		static bool hotGlobalEnabled;
		static int hotCooldownInSecs;
		static bool hotGlobalCooldownEnabled;
		static bool hotCooldownChatMessages;
		static bool hotProfessionChatMessages;
		static bool hotRandomProfessionMode;
		static bool hotSubscriptionMode;
		static List<Kit> hotKitsList;
		
		#endregion
		
		#region UPDATE COMMAND/HACKY VARS
		
		private static bool reloadCalled;
		private static SpawnKitConfiguration reLoadedConfig;
		
		private static bool saveCalled;
		
		private static string givePlayerName; //Used for givekit command. Workaround for not having an instance of SpawnKit to use.
    	private static string giveKitName;
    	private static bool adminGiveRequested;
		
		#endregion
		
		public static LogMan logMan = new LogMan(); //Declare logman for use.
    	
    	System.Random rand = new System.Random(); //Random used for random kit selection.
    	
    	List<string> weightedProfessionList = new List<string>(); //List of kits
    	
    	int cooldownSecondsRemaining;
    	
    	//Table with the players name as the key and the time they got a kit as the value. Used for kit cooldown.
    	static Dictionary<string, DateTime> cooldownTable = new Dictionary<string, DateTime>();
    	
    	//Table with players name as the key and the Kit they select as the value. Used to track who subscribed to what kit.
    	static Dictionary<string, Kit> kitSubscriptionTable = new Dictionary<string, Kit>();
    	
        protected override void Load()
        {
			RocketPlayerEvents.OnPlayerRevive += OnPlayerSpawn;
            logMan.LogMessage(2, "Kits Loaded:");
            foreach (Kit k in this.Configuration.Kits)
            {
            	logMan.LogMessage(2, k.Name);
            }
            BuildProfessionWeighedList();
            GetSettings();
        }
    	
    	private void FixedUpdate()
        {	
        	
        	DoQueuedUpdateCommands();
        	
        	ApplySettings();
        	
        }
        
        private void OnPlayerSpawn(RocketPlayer _player, Vector3 position, byte angle)
        {
        	if (this.Configuration.globalEnabled)
        	{        		
        		if (!this.Configuration.globalCooldownEnabled) { //If we have no cooldown.
        			
        			if (this.Configuration.subscriptionMode) {
        				if (kitSubscriptionTable.ContainsKey(_player.CharacterName)) {
        					GivePlayerKit(_player.Player, kitSubscriptionTable[_player.CharacterName].Name);
        					return;
        				}
        				else {
        					GivePlayerKit(_player.Player, this.Configuration.defaultKit);
        					return;
        				}
        			}
        			
        			if (!this.Configuration.randomProfessionMode) {
        				GivePlayerKit(_player.Player, this.Configuration.defaultKit);
        				return;
        			}
        			GivePlayerKit(_player.Player, GetChancedProfession());
        			return;
        		}
        			
        		//If global cooldown is enabled.
        		DateTime dtKitUsedLast;
        			
        		if (cooldownTable.TryGetValue(_player.SteamName, out dtKitUsedLast)){ //Cooldown is up and player is already in cooldown list.
        			
        			if ((DateTime.Now - dtKitUsedLast).TotalSeconds > this.Configuration.cooldownInSecs) {
        					
        				if (this.Configuration.subscriptionMode) {
        					if (kitSubscriptionTable.ContainsKey(_player.CharacterName)) { //If the player has subscribed to a kit.
        						GivePlayerKit(_player.Player, kitSubscriptionTable[_player.CharacterName].Name);
        						cooldownTable.Remove(_player.SteamName);
        						cooldownTable.Add(_player.SteamName, DateTime.Now);
        						return;
        					}
        					else { //If the player has not subscribed give them the config default.
	        					GivePlayerKit(_player.Player, this.Configuration.defaultKit);
	        					cooldownTable.Remove(_player.SteamName);
        						cooldownTable.Add(_player.SteamName, DateTime.Now);
        						return;
        					}
        				}
        				
        				if (this.Configuration.randomProfessionMode) {
        					GivePlayerKit(_player.Player, GetChancedProfession());
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        				else {
        					GivePlayerKit(_player.Player, this.Configuration.defaultKit);
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}

        			}
        			else //If cooldown is not up.
        			{
        				cooldownSecondsRemaining = this.Configuration.cooldownInSecs - (int)(DateTime.Now - dtKitUsedLast).TotalSeconds;
        					
        				if (this.Configuration.cooldownChatMessages) //If we are to send messages to players.
        					logMan.SayChatToPlayer(_player, cooldownSecondsRemaining + " seconds remaining until kit available.");
        			}
        		}
        		
        		else //If the player is not in the cooldown list.
        		{
        			if (this.Configuration.subscriptionMode) {
        				if (kitSubscriptionTable.ContainsKey(_player.CharacterName)) { //If the player has subscribed to a kit.
        					GivePlayerKit(_player.Player, kitSubscriptionTable[_player.CharacterName].Name);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        				else { //If the player has not subscribed give them the config default.
	        				GivePlayerKit(_player.Player, this.Configuration.defaultKit);
							cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        			}
        			
        			if (this.Configuration.randomProfessionMode) { //If we are in random profession mode.
        				GivePlayerKit(_player.Player, GetChancedProfession());
        				cooldownTable.Add(_player.SteamName, DateTime.Now);
        				return;
        			}
        			else {
        				GivePlayerKit(_player.Player, this.Configuration.defaultKit);
        				cooldownTable.Add(_player.SteamName, DateTime.Now);
        				return;
        			}
        			
        		}
        	}

        }
        
        private void GivePlayerKit(Player _player, string _kit)
        {	
        	foreach (Kit kit in this.Configuration.Kits) //Loop through kits and see if kit with name exists.
        	{
        		if (kit.Name.Equals(_kit)) //Found a matching kit.
        		{
        			if (this.Configuration.randomProfessionMode && this.Configuration.professionChatMessages)
        				logMan.SayChatToPlayer(RocketPlayer.FromPlayer(_player), "You spawned as a " + _kit + "." +
        				                      " " + kit.SpawnPercentChance + "% Chance.");
        			
        			foreach (KitItem kitItem in kit.Items) //Loop through all items
        			{
        				if (!ItemTool.tryForceGiveItem(_player, kitItem.ItemId, kitItem.Amount))
        				{
        					logMan.LogMessage(3, "Failed to give player item!");        			
        				}
        			}
        			return;
        		}
        		else 
        		{
        			
        		}
        	}
        	
        	logMan.LogMessage(3, "Kit does not exist!");
        	
        }
        
        private string GetChancedProfession() {
        	
        	return weightedProfessionList[rand.Next(weightedProfessionList.Count)];
        	
        }
        
        private void BuildProfessionWeighedList() {
        
        	weightedProfessionList.Clear(); //clear list first.
        	
        	foreach (Kit k in this.Configuration.Kits) {
        		for (int i = 0; i <= k.SpawnPercentChance; i++) {
        			if (k.SpawnPercentChance == 0) { //To avoid putting zero chance  (Disabled) kits in the lot. 
        				logMan.LogMessage(1, "Excluded kit " + k.Name + " from Profession list. Zero spawn chance.");
        			}
        			else { 
        				weightedProfessionList.Add(k.Name); 
        			}
        		}
        	}
        	
        	logMan.LogMessage(1, "Profession List Built.");
        	
        }
        
        private void ApplySettings() {
        	this.Configuration.cooldownInSecs = hotCooldownInSecs;
        	this.Configuration.globalEnabled = hotGlobalEnabled;
        	this.Configuration.globalCooldownEnabled = hotGlobalCooldownEnabled;
        	this.Configuration.cooldownChatMessages = hotCooldownChatMessages;
        	this.Configuration.professionChatMessages = hotProfessionChatMessages;
        	this.Configuration.randomProfessionMode = hotRandomProfessionMode;  
        	this.Configuration.subscriptionMode = hotSubscriptionMode;
        	this.Configuration.Kits = hotKitsList;
        }
        
        private void GetSettings() {
        	hotCooldownInSecs = this.Configuration.cooldownInSecs;
        	hotGlobalEnabled = this.Configuration.globalEnabled;
        	hotGlobalCooldownEnabled = this.Configuration.globalCooldownEnabled;
        	hotCooldownChatMessages = this.Configuration.cooldownChatMessages;
        	hotProfessionChatMessages = this.Configuration.professionChatMessages;
        	hotRandomProfessionMode = this.Configuration.randomProfessionMode;
        	hotSubscriptionMode = this.Configuration.subscriptionMode;
        	hotKitsList = this.Configuration.Kits;
        }
        
        private void ClearInventory(RocketPlayer _player) { //TODO Feels hacky. Figure out a better way.
			_player.Player.Equipment.dequip();
			int count = 7;
			byte b = 7;
			while (count >= 0 && count <= 7)
			{
				byte itemCount = _player.Inventory.getItemCount(b);
				if (itemCount > 0)
				{
					bool finished = false;
					byte b2 = (byte)(itemCount - 1);
					while (b2 >= 0 && b2 <= itemCount - 1 && !finished)
					{
						if (b2 == 0) finished = true; //TODO Hacky way to prevent index out of bounds exception. Prevent b2 from being used at 0 twice.
						_player.Inventory.removeItem(b, b2);
						if (b2 > 0) {
							b2 -= 1;
						}
					}
				}
				if (b > 0) {
					b -= 1;
				}
				count--;
			}
			
			logMan.LogMessage(1, _player.CharacterName + "'s inventory has been cleared!");
        }
        
        private void ClearClothing(RocketPlayer _player) { //TODO Feels hacky. Figure out a better way.
        	if (PlayerSavedata.fileExists(_player.Player.SteamChannel.SteamPlayer.SteamPlayerID, "/Player/Clothing.dat"))
			{
				PlayerSavedata.deleteFile(_player.Player.SteamChannel.SteamPlayer.SteamPlayerID, "/Player/Clothing.dat");
			}
			
        	_player.Player.SteamChannel.send("tellClothing", ESteamCall.ALL, ESteamPacket.UPDATE_TCP_BUFFER, new object[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
        	});
        	
        	_player.Player.Clothing.save();
        	_player.Player.Clothing.load();
        	logMan.LogMessage(1, _player.CharacterName + "'s clothing cleared.");
		}
        
        private bool DoesKitExist(string _kitName) {
        	foreach (Kit k in this.Configuration.Kits) {
        		if (k.Name.Equals(_kitName)) {
        		    	return true;
        		}
        	}
        	return false;
        }
        
        private void DoQueuedUpdateCommands() {
        	
        	if (reloadCalled) { //Reload settings if called.
        		this.Configuration = reLoadedConfig;
        		GetSettings();
        		BuildProfessionWeighedList();
        		logMan.LogMessage(2, "Configuration Reloaded! Any active changes not saved.");
        		reloadCalled = false;
        	}
        	
        	if (adminGiveRequested) { //Admin gives kit TODO: CLEANUP
        		
        		SteamPlayer steamPlayer;
        			
        		if (PlayerTool.tryGetSteamPlayer(givePlayerName, out steamPlayer) && DoesKitExist(giveKitName)) { //If steam playername is found.
        			ClearInventory(RocketPlayer.FromName(givePlayerName));
        			ClearClothing(RocketPlayer.FromName(givePlayerName));
        			GivePlayerKit(RocketPlayer.FromName(givePlayerName).Player, giveKitName);
        			logMan.LogMessage(2, "Admin gave " + givePlayerName + " the " + giveKitName + " kit.");
        	    }
        		else {
        			logMan.LogMessage(3, "givekit: no such player or kit.");
        		}
        		
        		adminGiveRequested = false;
        	}
        	
        	if (saveCalled) { //TODO Doesnt work. Writes defaults.
        		ApplySettings();
        		SpawnKitConfiguration test = this.Configuration;
        		logMan.LogMessage(2, "Configuration written to disk.");
        		saveCalled = false;
        	}
        }
        
        public static void AddPlayerToSubscriptionList(string _playerName, Kit _selectedKit) {
        	try {
        		kitSubscriptionTable.Add(_playerName, _selectedKit);
        	}
        	catch (ArgumentException e) {
        		kitSubscriptionTable.Remove(_playerName);
        		kitSubscriptionTable.Add(_playerName, _selectedKit);
        	}
        		
        }
        
        public static void RemovePlayerFromSubscriptionList(string _playerName, Kit _selectedKit) {
        	kitSubscriptionTable.Remove(_playerName);
        }
        
        public static void ReloadConfiguration() {
        	reLoadedConfig = RocketConfigurationHelper.LoadConfiguration<SpawnKitConfiguration>();
        	reloadCalled = true;
        }
        
        public static void SaveConfiguration() {
        	saveCalled = true;
        }
        
        public static void AdminGiveKit(string _playerCharName, string _kit) {
        	adminGiveRequested = true;
        	givePlayerName = _playerCharName;
        	giveKitName = _kit;
        }
        
        #region SETTERS
        public static void SetCooldown(int _var) {
        	hotCooldownInSecs = _var;
        }
        
        public static void SetEnabled(bool _setting) {
        	hotGlobalEnabled = _setting;
        }
        
        public static void SetGlobalCoolDownEnabled(bool _setting) {
        	hotGlobalCooldownEnabled = _setting;
        }
        
        public static void SetCoolDownChatMessagesEnabled(bool _setting) {
        	hotCooldownChatMessages = _setting;
        }
        
        public static void SetProfessionChatMessagesEnabled(bool _setting) {
        	hotProfessionChatMessages = _setting;
        }
        
        public static void SetProfessionModeEnable(bool _setting) {
        	hotRandomProfessionMode = _setting;
        }
        
        public static void SetSubscriptionMode(bool _setting) {
        	hotSubscriptionMode = _setting;
        }
        
        public static void SetKitsList(List<Kit> _kList) {
        	hotKitsList = _kList;
        }
        
        #endregion
        
        #region GETTERS
        public static int GetCooldown() {
        	return hotCooldownInSecs;
        }
        
        public static bool GetEnabled() {
        	return hotGlobalEnabled;
        }
        
        public static bool GetCooldownEnabled() {
        	return hotGlobalCooldownEnabled;
        }
        
        public static bool GetCoolDownMessagesEnabled() {
        	return hotCooldownChatMessages;
        }
        
        public static bool GetProfessionChatMessagesEnabled() {
        	return hotProfessionChatMessages;
        }
        
        public static bool GetProfessionModeEnabled() {
        	return hotRandomProfessionMode;
        }
        
        public static bool GetSubscriptionModeEnabled() {
        	return hotSubscriptionMode;
        }
        
        public static List<Kit> GetKitsList() {
        	return hotKitsList;
        }
        
        public static Dictionary<string, Kit> GetSubscriptionList() {
        	return kitSubscriptionTable;
        }
        
        #endregion
    }
}
