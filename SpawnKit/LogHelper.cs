 /*****
 * -- LogHelper: Used as a tiny helper to process messages in one place. Designed to be added to your existing source.  --
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
using Rocket.Unturned.Logging;
using SDG;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Steamworks;

namespace FC.SpawnKit
{

	internal class LogHelper
	{
		
		#region VARS
		
		internal const byte
			MESSAGELEVEL_DEBUG = 0,
			MESSAGELEVEL_INFO = 1,
			MESSAGELEVEL_WARNING = 2,
			MESSAGELEVEL_ERROR = 3
			;
		
		private  bool debugMode;
		
		private string parentName;
		
		private List<ulong> playerIDSendToList;
		
		#endregion
		
		internal LogHelper(string _parentName)
		{
			debugMode = false;
			parentName = _parentName;
			playerIDSendToList = new List<ulong>();
		}
		
		#region INTERNAL METHODS
		
		internal void LogMessage(byte _messageLevel, string _message)
		{
			switch (_messageLevel)
			{
				case MESSAGELEVEL_DEBUG:
					PrintDebugMessage(_message); break;
				case MESSAGELEVEL_INFO:
					PrintMessage(_message); break;
				case MESSAGELEVEL_WARNING:
					PrintWarningMessage(_message); break;
				case MESSAGELEVEL_ERROR:
					PrintErrorMessage(_message); break;
			}
		}
		
		internal void SetDebugMode(bool _debug)
		{
			debugMode = _debug;
		}
		
		internal void AddPlayerToLogOutputList(RocketPlayer _player)
		{
			if (!playerIDSendToList.Contains(_player.CSteamID.m_SteamID)) { playerIDSendToList.Add(_player.CSteamID.m_SteamID); }
		}
		
		internal void RemovePlayerFromLogOutputList(RocketPlayer _player)
		{
			
			foreach (ulong sID in playerIDSendToList)
			{
				if (sID.Equals(_player.CSteamID.m_SteamID))
				{
				    	playerIDSendToList.Remove(sID);
				    	return;
				}
			}
		}
		
		internal void ClearPlayerLogOutputList()
		{
			playerIDSendToList.Clear();
		}
		
		#endregion
		
		#region PRIVATE METHODS
		
		private void PrintDebugMessage(string _message)
		{
			if (debugMode) 
			{ 
				Logger.Log(_message); 
				PrintMessageToPlayerList(_message);
			}
		}
		
		private void PrintMessage(string _message)
		{
			Logger.Log(_message);
			PrintMessageToPlayerList(_message);
		}
		
		private void PrintWarningMessage(string _message)
		{
			Logger.LogWarning(parentName + " >> " + _message);
			PrintMessageToPlayerList(_message);
		}
		
		private void PrintErrorMessage(string _message)
		{
			Logger.LogError(parentName + " >> " + _message);
			PrintMessageToPlayerList(_message);
		}
		
		private void PrintMessageToPlayerList(string _message)
		{
			foreach (ulong sID in playerIDSendToList)
			{
				RocketChat.Say(RocketPlayer.FromCSteamID((CSteamID) sID), parentName + ": " + _message);
			}
		}
		
		#endregion
	}
}
