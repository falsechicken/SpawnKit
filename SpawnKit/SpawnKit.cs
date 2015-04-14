/**
 * AnnounceBot plugin for the Rocket Unturned Server Wrapper.
 * 
 * Licensed under the GPLv2.
 * 
 * False_Chicken
 * */

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using Rocket;
using Rocket.RocketAPI;
using Rocket.Logging;
using Rocket.RocketAPI.Events;
using SDG;
using Steamworks;
using UnityEngine;

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
		
		static bool reloadCalled;
		static SpawnKitConfiguration reLoadedConfig;
		
		
		bool sub = false;
    	
    	System.Random rand = new System.Random();
    	
    	List<string> weightedProfessionList = new List<string>();
    	
    	
    	int cooldownSecondsRemaining;
    	
    	//Table with the players name as the key and the time they got a kit as the DateTime
    	static Dictionary<string, DateTime> cooldownTable = new Dictionary<string, DateTime>();
    	
    	static Dictionary<string, Kit> spawnSubscriptionTable = new Dictionary<string, Kit>();
    	
        private void FixedUpdate()
        {	
        	if (sub == false && this.Loaded)
        	{
            	RocketPlayerEvents.OnPlayerRevive += skOnPlayerSpawn;
            	Logger.Log("Kits Loaded:");
            	foreach (Kit k in this.Configuration.Kits)
            	{
            		Logger.Log(k.Name);
            	}
            	BuildProfessionWeighedList();
            	GetSettings();
            	sub = true;
        	}
        	
        	if (reloadCalled) {
        		this.Configuration = reLoadedConfig;
        		GetSettings();
        		BuildProfessionWeighedList();
        		Logger.Log("Configuration Reloaded! Any active changes not saved.");
        		reloadCalled = false;
        	}
        	
        	ApplySettings();
        	
        }
        
        private void skOnPlayerSpawn(RocketPlayer _player, Vector3 position, byte angle)
        {
        	if (this.Configuration.globalEnabled)
        	{        		
        		if (!this.Configuration.globalCooldownEnabled) { //If we have no cooldown.
        			
        			if (this.Configuration.subscriptionMode) {
        				if (spawnSubscriptionTable.ContainsKey(_player.CharacterName)) {
        					GivePlayerKit(_player.Player, spawnSubscriptionTable[_player.CharacterName].Name);
        					return;
        				}
        			}
        			
        			if (!this.Configuration.randomProfessionMode) {
        				GivePlayerKit(_player.Player, "Default");
        				return;
        			}
        			GivePlayerKit(_player.Player, GetChancedProfession());
        			return;
        		}
        			
        		//If global cooldown is enabled.
        		DateTime dtKitUsedLast;
        			
        		if (cooldownTable.TryGetValue(_player.SteamName, out dtKitUsedLast)){
        			
        			if ((DateTime.Now - dtKitUsedLast).TotalSeconds > this.Configuration.cooldownInSecs) {
        					
        				if (this.Configuration.subscriptionMode) {
        					GivePlayerKit(_player.Player, spawnSubscriptionTable[_player.CharacterName].Name);
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        				
        				if (this.Configuration.randomProfessionMode) {
        					GivePlayerKit(_player.Player, GetChancedProfession());
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}
        				else {
        					GivePlayerKit(_player.Player, "Default");
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        					return;
        				}

        			}
        			else //If cooldown is not up.
        			{
        				cooldownSecondsRemaining = this.Configuration.cooldownInSecs - (int)(DateTime.Now - dtKitUsedLast).TotalSeconds;
        					
        				if (this.Configuration.cooldownChatMessages) //If we are to send messages to players.
        					RocketChatManager.Say(_player.SteamName + " " + cooldownSecondsRemaining + " seconds remaining until kit available.");
        			}
        		}
        		
        		else
        		{
        			cooldownTable.Add(_player.SteamName, DateTime.Now);
        			
        			if (this.Configuration.randomProfessionMode) { //If we are in random profession mode.
        				GivePlayerKit(_player.Player, GetChancedProfession());
        				return;
        			}
        			else {
        				GivePlayerKit(_player.Player, "Default");
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
        			Logger.Log("Found Kit!");
        			
        			if (this.Configuration.randomProfessionMode && this.Configuration.professionChatMessages)
        				RocketChatManager.Say(_player.name + " has spawned as a " + _kit + "." +
        				                      " " + kit.SpawnPercentChance + "% Chance.");
        			
        			foreach (KitItem kitItem in kit.Items) //Loop through all items
        			{
        				if (!ItemTool.tryForceGiveItem(_player, kitItem.ItemId, kitItem.Amount))
        				{
        					Logger.Log("Failed to give player item!");        			
        				}
        			}
        			return;

        		}
        		else 
        		{
        			
        		}

        	}
        	
        }
        
        private string GetChancedProfession() {
        	
        	return weightedProfessionList[rand.Next(weightedProfessionList.Count)];
        	
        }
        
        private void BuildProfessionWeighedList() {
        
        	weightedProfessionList.Clear(); //clear list first.
        	
        	foreach (Kit k in this.Configuration.Kits) {
        		for (int i = 0; i <= k.SpawnPercentChance; i++) {
        			weightedProfessionList.Add(k.Name);
        		}
        	}
        	
        	Logger.Log("Profession List Built.");
        	
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
        
        public static void AddToSubscriptionList(string _playerName, Kit _selectedKit) {
        	try {
        		spawnSubscriptionTable.Add(_playerName, _selectedKit);
        	}
        	catch (ArgumentException e) {
        		spawnSubscriptionTable.Remove(_playerName);
        		spawnSubscriptionTable.Add(_playerName, _selectedKit);
        	}
        		
        }
        
        public static void RemoveFromSubscriptionList(string _playerName, Kit _selectedKit) {
        	spawnSubscriptionTable.Remove(_playerName);
        }
        
        public static void ReloadConfiguration() {
        	reLoadedConfig = RocketConfigurationHelper.LoadConfiguration<SpawnKitConfiguration>();
        	reloadCalled = true;
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
        
        #endregion
    }
}
