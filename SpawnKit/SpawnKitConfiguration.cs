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

namespace FC.SpawnKit
{
	public class SpawnKitConfiguration : IRocketConfiguration
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
		
		public IRocketConfiguration DefaultConfiguration
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
							SpawnPercentChance = 100,
							Items = new List<KitItem>
							{
								new KitItem(3, 1),
								new KitItem(2, 1),
								new KitItem(1, 1),
							} 
						},
					},
				};
			}
		}
	}
}
