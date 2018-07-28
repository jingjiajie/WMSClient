using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FromDeliverOrder
{
    class DeliveryOrderState
    {
        public const int DELIVERY_STATE_IN_LOADING = 0;
        public const int DELIVERY_STATE_PARTIAL_LOADING = 1;
        public const int DELIVERY_STATE_ALL_LOADING = 2;
        public const int DELIVERY_STATE_IN_DELIVER = 3;
        public const int DELIVERY_STATE_DELIVER_FINNISH = 4;
    }
}
