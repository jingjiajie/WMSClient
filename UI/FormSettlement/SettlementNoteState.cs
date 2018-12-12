using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormSettlement
{
    class SettlementNoteState
    {
        public const int NoComfirm = 0;//待确认
        public const int Receivables = 1;//应收
        public const int Receipts = 2;//实收
    }
}
