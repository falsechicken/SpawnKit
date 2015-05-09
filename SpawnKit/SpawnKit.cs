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
using FC.SpawnKit;

namespace FC.SpawnKit
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
		
		internal static LogHelper logHelper = new LogHelper("SpawnKit"); //Declare logHelper for use.
    	
    	private System.Random rand = new System.Random(); //Random used for random kit selection.
    	
    	private List<string> weightedProfessionList = new List<string>(); //List of kits
    	
    	private int cooldownSecondsRemaining;
    	
    	//Table with the players name as the key and the time they got a kit as the value. Used for kit cooldown.
    	private static Dictionary<string, DateTime> cooldownTable = new Dictionary<string, DateTime>();
    	
    	//Table with players name as the key and the Kit they select as the value. Used to track who subscribed to what kit.
    	private static Dictionary<string, Kit> kitSubscriptionTable = new Dictionary<string, Kit>();
    	
        protected override void Load()
        {
        	RocketPlayerEvents.OnPlayerRevive += OnPlayerSpawn;
        	
            logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Kits Loaded:");
            
            foreach (Kit k in this.Configuration.Kits)
            {
            	logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, k.Name);
            }
            BuildProfessionWeighedList();
            GetSettings();
        }
    	
    	private void FixedUpdate()
        {	
        	
        	DoQueuedUpdateCommands();
        	
        	ApplySettings();
        	
        }
    	
    	#region PRIVATE METHODS
        
        private void OnPlayerSpawn(RocketPlayer _player, Vector3 position, byte angle)
        {
        	if (this.Configuration.globalEnabled == false) //if the mod is disabled return;
        		return;
        		
        	if (!this.Configuration.globalCooldownEnabled) { //If we have no cooldown.
        			
        			if (this.Configuration.subscriptionMode) {
        				if (kitSubscriptionTable.ContainsKey(_player.CharacterName)) {
        					GivePlayerKit(_player.Player, kitSubscriptionTable[_player.CharacterName].Name, false);
        					return;
        				}
        				else {
        					GivePlayerKit(_player.Player, this.Configuration.defaultKit, false);
        					return;
        				}
        			}
        			
        			if (!this.Configuration.randomProfessionMode) {
        				GivePlayerKit(_player.Player, this.Configuration.defaultKit, false);
        				return;
        			}
        			GivePlayerKit(_player.Player, GetChancedProfession(), false);
        			return;
        	}
        			
        	//If global cooldown is enabled.
        	DateTime dtKitUsedLast;
        			
        	if (cooldownTable.TryGetValue(_player.SteamName, out dtKitUsedLast)){ //Cooldown is up and player is already in cooldown list.
        			
        			if ((DateTime.Now - dtKitUsedLast).TotalSeconds > this.Configuration.cooldownInSecs) {
        					
        				if (this.Configuration.subscriptionMode) {
        					if (kitSubscriptionTable.ContainsKey(_player.CharacterName)) { //If the player has subscribed to a kit.
        						GivePlayerKit(_player.Player, kitSubscriptionTable[_player.CharacterName].Name, false);
        						cooldownTable.Remove(_player.SteamName);
        						cooldownTable.Add(_player.SteamName, DateTime.Now);
        						return;
        					}
        					else { //If the player has not subscribed give them the config default.
	        					GivePlayerKit(_player.Player, this.Configuration.defaultKit, false);
	        					cooldownTable.Remove(_player.SteamName);
        						cooldownTable.Add(_player.SteamName, DateTime.Now);
        						return;
        					}
        				}
        				
        				if (this.Configuration.randomProfessionMode) {
        					GivePlayerKit(_player.Player, GetChancedProfession(), false);
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        				else {
        					GivePlayerKit(_player.Player, this.Configuration.defaultKit, false);
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}

        			}
        			else //If cooldown is not up.
        			{
        				cooldownSecondsRemaining = this.Configuration.cooldownInSecs - (int)(DateTime.Now - dtKitUsedLast).TotalSeconds;
        					
        				if (this.Configuration.cooldownChatMessages) //If we are to send messages to players.
        					RocketChatManager.Say(_player, cooldownSecondsRemaining + " seconds remaining until kit available.");
        			}
        		}
        		
        	else //If the player is not in the cooldown list.
        		{
        			if (this.Configuration.subscriptionMode) {
        				if (kitSubscriptionTable.ContainsKey(_player.CharacterName)) { //If the player has subscribed to a kit.
        					GivePlayerKit(_player.Player, kitSubscriptionTable[_player.CharacterName].Name, false);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        				else { //If the player has not subscribed give them the config default.
	        				GivePlayerKit(_player.Player, this.Configuration.defaultKit, false);
							cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        			}
        			
        			if (this.Configuration.randomProfessionMode) { //If we are in random profession mode.
        				GivePlayerKit(_player.Player, GetChancedProfession(), false);
        				cooldownTable.Add(_player.SteamName, DateTime.Now);
        				return;
        			}
        			else {
        				GivePlayerKit(_player.Player, this.Configuration.defaultKit, false);
        				cooldownTable.Add(_player.SteamName, DateTime.Now);
        				return;
        			}
        			
        		}

        }
        
        private void GivePlayerKit(Player _player, string _kit, bool _adminGive)
        {	
        	foreach (Kit kit in this.Configuration.Kits) //Loop through kits and see if kit with name exists.
        	{
        		if (kit.Name.ToLower().Equals(_kit.ToLower())) //Found a matching kit.
        		{
        			if (this.Configuration.randomProfessionMode && this.Configuration.professionChatMessages && _adminGive == false)
        					RocketChatManager.Say(RocketPlayer.FromPlayer(_player), "You spawned as a " + _kit + "." +
        					                      " " + kit.SpawnPercentChance + "% Chance.");
        			foreach (KitItem kitItem in kit.Items) //Loop through all items
        			{
        				if (!ItemTool.tryForceGiveItem(_player, kitItem.ItemId, kitItem.Amount))
        				{
        					logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Failed to give player item!");        			
        				}
        			}
        			return;
        		}
        		else 
        		{
        			
        		}
        	}
        	
        	logHelper.LogMessage(3, "Kit does not exist!");
        	
        }
        
        private string GetChancedProfession() {
        	
        	return weightedProfessionList[rand.Next(weightedProfessionList.Count)];
        	
        }
        
        private void BuildProfessionWeighedList() {
        
        	weightedProfessionList.Clear(); //clear list first.
        	
        	foreach (Kit k in this.Configuration.Kits) {
        		for (int i = 0; i <= k.SpawnPercentChance; i++) {
        			if (k.SpawnPercentChance == 0) { //To avoid putting zero chance  (Disabled) kits in the lot. 
        				logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Excluded kit " + k.Name + " from Profession list. Zero spawn chance.");
        			}
        			else { 
        				weightedProfessionList.Add(k.Name); 
        			}
        		}
        	}
        	
        	logHelper.LogMessage(LogHelper.MESSAGELEVEL_DEBUG, "Profession List Built.");
        	
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
        
        private void ClearInventory(RocketPlayer _player) {
        	_player.Inventory.Clear();
			logHelper.LogMessage(LogHelper.MESSAGELEVEL_DEBUG, _player.CharacterName + "'s inventory has been cleared!");
        }
        
        private bool DoesKitExist(string _kitName) {
        	foreach (Kit k in this.Configuration.Kits) {
        		if (k.Name.ToLower().Equals(_kitName.ToLower())) {
        		    	return true;
        		}
        	}
        	return false;
        }
        
        private void DoQueuedUpdateCommands() {
        	
        	if (reloadCalled) { //Reload settings if called.
        		this.ReloadConfiguration();
        		GetSettings();
        		BuildProfessionWeighedList();
        		logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Configuration Reloaded! Any active changes not saved.");
        		reloadCalled = false;
        	}
        	
        	if (adminGiveRequested) { //Admin gives kit TODO: CLEANUP
        		
        		SteamPlayer steamPlayer;
        			
        		if (PlayerTool.tryGetSteamPlayer(givePlayerName, out steamPlayer) && DoesKitExist(giveKitName)) { //If steam playername is found.
        			ClearInventory(RocketPlayer.FromName(givePlayerName));
        			GivePlayerKit(RocketPlayer.FromName(givePlayerName).Player, giveKitName, true);
        			logHelper.LogMessage(LogHelper.MESSAGELEVEL_INFO, "Admin gave " + givePlayerName + " the " + giveKitName + " kit.");
        	    }
        		else {
        			logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "givekit: no such player or kit.");
        		}
        		
        		adminGiveRequested = false;
        	}
        	
        	if (saveCalled) {
        		ApplySettings();
        		this.Configuration.Save();
        		logHelper.LogMessage(LogHelper.MESSAGELEVEL_WARNING, "Configuration written to disk.");
        		saveCalled = false;
        	}
        }
        
        private bool IsSpawnKitEnabled() {
        	return this.Configuration.globalEnabled;
        }
        
        #endregion
        
        #region PUBLIC METHODS
        
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
        
        public static void ReloadSpawnKitConfiguration() {
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
        
        /**
         * Adds a player to LogHelpers list of players to get log output sent to them.
         */
        public static void AddPlayerToLogList(RocketPlayer _player)
        {
        	logHelper.AddPlayerToLogOutputList(_player);
        }
        
        /**
         * Removes a player from the list of players to get log output sent to them.
         */
        public static void RemovePlayerFromLogList(RocketPlayer _player)
        {
        	logHelper.RemovePlayerFromLogOutputList(_player);
        }
        
        /**
         * Clears the list of players that get log output messages sent to them.
         */
        public static void CleanPlayerLogList()
        {
        	logHelper.ClearPlayerLogOutputList();
        }
        
        #endregion
        
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