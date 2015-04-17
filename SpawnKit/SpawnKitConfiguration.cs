/*****
 * SpawnKitConfiguration - Configuration object for SpawnKit
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
using Rocket.RocketAPI;
using System.Collections.Generic;
using System.Xml.Serialization;
using fc.itemdb;

namespace fc.spawnkit
{
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
					randomProfessionMode = true,
					professionChatMessages = true,
					subscriptionMode = false,
					
					Kits = new List<Kit>
					{
						new Kit
						{
							Name = "Civilian",
							SpawnPercentChance = 23,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Green_Shirt, 1),
								new KitItem(ItemDB_All.Trouser_Pants, 1),
								new KitItem(ItemDB_All.Swiss_Knife, 1),
								new KitItem(ItemDB_All.Apple_Juice, 1),
								new KitItem(ItemDB_All.Chips, 1),
							} 
						},
						
						new Kit
						{
							Name = "Civilian2",
							SpawnPercentChance = 23,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Grocer_Top, 1),
								new KitItem(ItemDB_All.Work_Jeans, 1),
								new KitItem(ItemDB_All.Kitchen_Knife, 1),
								new KitItem(ItemDB_All.Bottled_Water, 1),
								new KitItem(ItemDB_All.Canned_Chicken_Soup, 1),
							}
						},
						
						new Kit
						{
							Name = "Civilian3",
							SpawnPercentChance = 23,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Orange_Hoodie, 1),
								new KitItem(ItemDB_All.Work_Jeans, 1),
								new KitItem(ItemDB_All.Kitchen_Knife, 1),
								new KitItem(ItemDB_All.Bottled_Water, 1),
								new KitItem(ItemDB_All.Candy_Bar, 1),
							}
						},
						
						new Kit
						{
							Name = "Medic",
							SpawnPercentChance = 12,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Medic_Top, 1),
								new KitItem(ItemDB_All.Medic_Bottom, 1),
								new KitItem(ItemDB_All.Dressing, 2),
								new KitItem(ItemDB_All.Bottled_Water, 1),
								new KitItem(ItemDB_All.Suturekit, 1),
							}
						},
						
						new Kit
						{
							Name = "EscapedPrisoner",
							SpawnPercentChance = 10,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Prisoner_Top, 1),
								new KitItem(ItemDB_All.Prisoner_Bottom, 1),
								new KitItem(ItemDB_All.Butterfly_Knife, 1),
							}
						},
						
						new Kit
						{
							Name = "PoliceOfficer",
							SpawnPercentChance = 6,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Police_Top, 1),
								new KitItem(ItemDB_All.Police_Bottom, 1),
								new KitItem(ItemDB_All.Police_Cap, 1),
								new KitItem(ItemDB_All.Colt, 1),
								new KitItem(ItemDB_All.Colt_Magazine, 1),
							}
						},
						
						new Kit
						{
							Name = "Soldier",
							SpawnPercentChance = 3,
							Items = new List<KitItem>
							{
								new KitItem(ItemDB_All.Military_Top, 1),
								new KitItem(ItemDB_All.Military_Bottom, 1),
								new KitItem(ItemDB_All.Beret, 1),
								new KitItem(ItemDB_All.Maplestrike, 1),
								new KitItem(ItemDB_All.Military_Magazine, 1),
								new KitItem(ItemDB_All.MRE, 1),
							}
						},
					},
				};
			}
		}
	}
}
