using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.BusinessLayer;

namespace TpSlManager
{
    public class SlTpItems
    {
        public string Id { get; set; }
        public Order EntryOrder { get; set; }
        public List<Order> SlItems { get; set; }
        public List<Order> TpItems { get; set; }
        public string Comment { get; set; }

        public SlTpItems(Order order, string guid, string comment = "")
        {
            this.Id = guid;
            this.EntryOrder = order;
            this.SlItems = new List<Order>();
            this.TpItems = new List<Order>();
            this.Comment = comment;
        }
        
        public void AddSl(Order order) => SlItems.Add(order);
        public void AddTp(Order order) => TpItems.Add(order);

    }
}
