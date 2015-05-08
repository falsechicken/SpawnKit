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
using Rocket.Logging;
using Rocket.RocketAPI;

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
		
		private List<RocketPlayer> playerSendToList;
		
		#endregion
		
		internal LogHelper(string _parentName)
		{
			debugMode = false;
			parentName = _parentName;
			playerSendToList = new List<RocketPlayer>();
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
			if (!playerSendToList.Contains(_player)) { playerSendToList.Add(_player); }
		}
		
		internal void RemovePlayerFromLogOutputList(RocketPlayer _player)
		{
			foreach (RocketPlayer rP in playerSendToList)
			{
				if (rP.CSteamID.Equals(_player.CSteamID))
				    {
				    	playerSendToList.Remove(rP);
				    	return;
				    }
			}
		}
		
		internal void ClearPlayerLogOutputList()
		{
			playerSendToList.Clear();
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
			foreach (RocketPlayer _player in playerSendToList)
			{
				RocketChatManager.Say(_player, parentName + ": " + _message);
			}
		}
		
		#endregion
	}
}
