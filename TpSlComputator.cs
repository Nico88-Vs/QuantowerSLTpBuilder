using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.BusinessLayer;

namespace TpSlManager
{
    public class TpSlComputator<T>
    {
        private SlTpCondictionHolder<T> ListOfDelegates;
        private int tp_items;
        private int sl_items;

        public TpSlComputator(SlTpCondictionHolder<T> listOfDelegates)
        {
            this.ListOfDelegates = listOfDelegates;
            this.tp_items = listOfDelegates.TpDelegateObj.Length;
            this.sl_items = listOfDelegates.SlDelegateObj.Length;
        }

        public void PlaceOrder(Trade trade, SlTpItems items)
        {
            //TODO: market Ordere only
            var _order = Core.Instance.Orders.FirstOrDefault(x => x.Id == trade.OrderId);
            var _slOrdereType = trade.Symbol.GetAlowedOrderTypes(OrderTypeUsage.CloseOrder);
            var side = trade.Side == Side.Buy ? Side.Sell : Side.Buy;

            for (var i = 0; i < sl_items; i++)
            {
                PlaceOrderRequestParameters _sl = new PlaceOrderRequestParameters()
                {
                    Symbol = trade.Symbol,
                    Price = this.ListOfDelegates.SlDelegate[i](this.ListOfDelegates.SlDelegateObj[i]),
                    Comment = _order != null ? _order.Comment : "Order is null",
                    OrderTypeId = _slOrdereType.FirstOrDefault(x => x.Behavior == OrderTypeBehavior.Market).Id,
                    AdditionalParameters = new List<SettingItem>
                    {
                        new SettingItemBoolean(OrderType.REDUCE_ONLY, true)
                    },
                    TimeInForce = TimeInForce.GTC,
                    Quantity = this.RoundQuantity(trade.Quantity / this.sl_items, trade.Symbol),
                    Side = side,
                };

                var resoult = Core.Instance.PlaceOrder(_sl);

                if (resoult.Status != TradingOperationResultStatus.Success)
                {
                    Core.Instance.Loggers.Log("Error placing sl ", LoggingLevel.Trading);
                    items.AddSl(Core.Instance.Orders.FirstOrDefault(x => x.Id == resoult.OrderId));
                }
            }

            for (var i = 0; i < tp_items; i++)
            { 

                PlaceOrderRequestParameters _tp = new PlaceOrderRequestParameters()
                {
                    Symbol = trade.Symbol,
                    Price = this.ListOfDelegates.TpDelegate[i](this.ListOfDelegates.TpDelegateObj[i]),
                    Comment = _order != null ? _order.Comment : "Order is null",
                    OrderTypeId = _slOrdereType.FirstOrDefault(x => x.Behavior == OrderTypeBehavior.Market).Id,
                    AdditionalParameters = new List<SettingItem>
                    {
                        new SettingItemBoolean(OrderType.REDUCE_ONLY, true)
                    },
                    TimeInForce = TimeInForce.GTC,
                    Quantity = this.RoundQuantity(trade.Quantity / this.tp_items, trade.Symbol),
                    Side = side,
                };

                var tp_resoult = Core.Instance.PlaceOrder(_tp);

                if (tp_resoult.Status != TradingOperationResultStatus.Success)
                {
                    Core.Instance.Loggers.Log("Error placing tp", LoggingLevel.Trading);
                    items.AddTp(Core.Instance.Orders.FirstOrDefault(x => x.Id == tp_resoult.OrderId));
                }
            }
           
        }

        public void UpdateOrder(List<Order>order, bool isTp)
        {
            try
            {
                for (int i = 0; i < order.Count; i++)
                {
                    var request = new ModifyOrderRequestParameters(order[i]);
                    switch (isTp)
                    {
                        case true:
                            request.Price = this.ListOfDelegates.TpDelegate[i](this.ListOfDelegates.TpDelegateObj[i]);
                            break;
                        case false:
                            request.Price = this.ListOfDelegates.SlDelegate[i](this.ListOfDelegates.SlDelegateObj[i]);
                            break;
                    }

                    var resoult = Core.Instance.ModifyOrder(request);

                    if (resoult.Status == TradingOperationResultStatus.Failure)
                        Core.Instance.Loggers.Log("Error modifing sl or tp", LoggingLevel.Trading);
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private double RoundQuantity(double quantity, Symbol _Symbol)
        {
            //TODO: aggiungo un min lot per evitare l arrotondamento in difetto
            return (Math.Round(quantity / _Symbol.MinLot) * _Symbol.MinLot) + _Symbol.MinLot;
        }
    }
}
