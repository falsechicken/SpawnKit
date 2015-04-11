/**
 * AnnounceBot plugin for the Rocket Unturned Server Wrapper.
 * 
 * Licensed under the GPLv2.
 * 
 * False_Chicken
 * */

using System;
using System.Collections.Generic;
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
    	bool sub = false;
    	
    	int cooldownSecondsRemaining;
    	
    	//Table with the players name as the key and the time they got a kit as the DateTime
    	Dictionary<string, DateTime> cooldownTable = new Dictionary<string, DateTime>();
    	
        private void FixedUpdate()
        {	
        	if (sub == false && this.Loaded)
        	{
            	RocketPlayerEvents.OnPlayerRevive += skOnPlayerSpawn;
            	sub = true;
        	}
        }
        
        private void skOnPlayerSpawn(RocketPlayer _player, Vector3 position, byte angle)
        {
        	if (this.Configuration.enabled)
        	{        		
        		if (this.Configuration.cooldownEnabled)
        		{	
        			DateTime dtKitUsedLast;
        			
        			if (cooldownTable.TryGetValue(_player.SteamName, out dtKitUsedLast)){
        				if ((DateTime.Now - dtKitUsedLast).TotalSeconds > this.Configuration.cooldownInSecs) {
        					GivePlayerKit(_player.Player, "spawn");
        					cooldownTable.Remove(_player.SteamName);
        					cooldownTable.Add(_player.SteamName, DateTime.Now);
        				}
        				else
        				{
        					cooldownSecondsRemaining = this.Configuration.cooldownInSecs - (int)(DateTime.Now - dtKitUsedLast).TotalSeconds;
        					
        					if (this.Configuration.cooldownChatMessages)
        						RocketChatManager.Say(_player.SteamName + " " + cooldownSecondsRemaining + " seconds remaining until kit available.");
        				}
        			}
        			else
        			{
        				cooldownTable.Add(_player.SteamName, DateTime.Now);
        				GivePlayerKit(_player.Player, "spawn");
        			}
        		}
        		else //If we have no cooldown.
        		{
        			GivePlayerKit(_player.Player, "spawn");
        		}
        	}
        }
        
        private void GivePlayerKit(Player _player, string _kit)
        {	
      		for (int i = 0; i < (int)this.Configuration.itemIDs.Length; i++)
        	{
        		if (!ItemTool.tryForceGiveItem(_player, this.Configuration.itemIDs[i], 1))
        		{
        			Logger.Log("Failed to give player item!");        			
        		}
        	}
        }
    }
}