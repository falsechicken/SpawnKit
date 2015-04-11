/*

 */
using System;
using Rocket.RocketAPI;

namespace fc.spawnkit
{
	/// <summary>
	/// Description of SpawnKitConfiguration.
	/// </summary>
	public class SpawnKitConfiguration : RocketConfiguration
	{
		public bool enabled;
		public bool cooldownEnabled;
		public bool cooldownChatMessages;
		public int cooldownInSecs;
		public ushort[] itemIDs;
		public RocketConfiguration DefaultConfiguration
		{
			get
			{
				return new SpawnKitConfiguration
				{
					enabled = true,
					cooldownEnabled = true,
					cooldownInSecs = 600,
					cooldownChatMessages = true,
					itemIDs = new ushort[]
					{
						139,
						184,
						2
					}
				};
			}
		}
	}
}
