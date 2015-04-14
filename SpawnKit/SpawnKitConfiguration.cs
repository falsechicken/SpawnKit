/*

 */
using System;
using System.Xml;
using Rocket.RocketAPI;
using System.Collections.Generic;
using System.Xml.Serialization;
using SDG;
using fc.itemdb;

namespace fc.spawnkit
{
	/// <summary>
	/// Description of SpawnKitConfiguration.
	/// </summary>
	public class SpawnKitConfiguration : RocketConfiguration
	{
		[XmlArrayItem(ElementName = "Kit")]
		public List<Kit> Kits;
		public bool globalEnabled;
		public bool globalCooldownEnabled;
		public bool cooldownChatMessages;
		public bool professionChatMessages;
		public bool randomProfessionMode;
		public bool subscriptionMode;
		public string defaultKit;
		public int cooldownInSecs;
		
		public RocketConfiguration DefaultConfiguration
		{
			get
			{
				return new SpawnKitConfiguration() 
				{
					globalEnabled = true,
					globalCooldownEnabled = true,
					cooldownInSecs = 300,
					cooldownChatMessages = true,
					defaultKit = "Default",
					randomProfessionMode = false,
					professionChatMessages = false,
					subscriptionMode = false,
					
					Kits = new List<Kit>
					{
						new Kit
						{
							
							Name = "Default",
							SpawnPercentChance = 0,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Swiss_Knife, 1),
								new KitItem(ItemDB_All.Tape, 1)
							}
						},
						
						new Kit
						{
							Name = "Rifleman",
							SpawnPercentChance = 25,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Military_Top, 1),
								new KitItem(ItemDB_All.Military_Bottom, 1),
								new KitItem(ItemDB_All.Military_Vest, 1),
								new KitItem(ItemDB_All.Eaglefire, 1),
								new KitItem(ItemDB_All.Military_Magazine, 3),
								new KitItem(ItemDB_All.Colt, 1),
								new KitItem(ItemDB_All.Colt_Magazine, 2),
								new KitItem(ItemDB_All.Military_Knife, 1),
								new KitItem(ItemDB_All.Black_Bandana, 1),
								
							}
						},
						
						new Kit
						{
							Name = "Assault",
							SpawnPercentChance = 25,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Military_Top, 1),
								new KitItem(ItemDB_All.Military_Bottom, 1),
								new KitItem(ItemDB_All.Military_Vest, 1),
								new KitItem(ItemDB_All.Bluntforce, 1),
								new KitItem(ItemDB_All.Tweleve_Gauge_Shells, 5),
								new KitItem(ItemDB_All.Cobra, 1),
								new KitItem(ItemDB_All.Cobra_Magazine, 2),
								new KitItem(ItemDB_All.Military_Knife, 1),
								new KitItem(ItemDB_All.Blue_Bandana, 1),
							}
						},
						
												new Kit
						{
							Name = "Gunner",
							SpawnPercentChance = 25,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Military_Top, 1),
								new KitItem(ItemDB_All.Military_Bottom, 1),
								new KitItem(ItemDB_All.Military_Vest, 1),
								new KitItem(ItemDB_All.Nykorev, 1),
								new KitItem(ItemDB_All.Nykorev_Box, 3),
								new KitItem(ItemDB_All.Cobra, 1),
								new KitItem(ItemDB_All.Cobra_Magazine, 2),
								new KitItem(ItemDB_All.Military_Knife, 1),
								new KitItem(ItemDB_All.Green_Bandana, 1),
							}
						},
						

					},
					
				};
			}
			}
		}
}
