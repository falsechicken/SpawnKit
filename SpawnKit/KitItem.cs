/*****
 * KitItem - An item that goes into a 'kit' for SpawnKit. Represents and ItemID and amount.
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
using System.Xml.Serialization;
namespace fc.spawnkit
{
	public class KitItem
	{
		[XmlAttribute("id")]
		public ushort ItemId;
		[XmlAttribute("amount")]
		public byte Amount;
		public KitItem()
		{
		}
		public KitItem(ushort itemId, byte amount)
		{
			this.ItemId = itemId;
			this.Amount = amount;
		}
	}
}
