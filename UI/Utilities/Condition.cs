using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WMS.UI
{
    public class Condition
    {
        public List<ConditionItem> conditions = new List<ConditionItem>();
        public List<OrderItem> orders = new List<OrderItem>();

        public override string ToString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        public Condition AddCondition(String key, Object[] values, ConditionItemRelation relation = ConditionItemRelation.EQUAL)
        {
            ConditionItem conditionItem = new ConditionItem();
            conditionItem.key = key;
            conditionItem.relation = relation.ToString();
            conditionItem.values  = values;
            this.conditions.Add(conditionItem);
            return this;
        }

        public Condition AddCondition(String key, Object value, ConditionItemRelation relation = ConditionItemRelation.EQUAL)
        {
            ConditionItem conditionItem = new ConditionItem();
            conditionItem.key = key;
            conditionItem.relation = relation.ToString();
            conditionItem.values = new Object[] { value };
            this.conditions.Add(conditionItem);
            return this;
        }

        public Condition AddOrder(String key, OrderItemOrder order = OrderItemOrder.ASC)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.key = key;
            orderItem.order = order.ToString();
            this.orders.Add(orderItem);
            return this;
        }
    }

    public class ConditionItem
    {
        public string key;
        public string relation = ConditionItemRelation.EQUAL.ToString();
        public object[] values;
    }

    public enum ConditionItemRelation
    {
        EQUAL, NOT_EQUAL, GREATER_THAN, GREATER_THAN_OR_EQUAL_TO, LESS_THAN, LESS_THAN_OR_EQUAL_TO, BETWEEN, CONTAINS,IN
    }

    public class OrderItem
    { 
        public string key;
        public string order = OrderItemOrder.ASC.ToString();
    }

    public enum OrderItemOrder
    {
        ASC, DESC
    }
}
