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
