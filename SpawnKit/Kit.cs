using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace fc.spawnkit
{
	public class Kit
	{
		public string Name;
		public double SpawnPercentChance;
		[XmlArrayItem(ElementName = "Item")]
		public List<KitItem> Items;
	}
}