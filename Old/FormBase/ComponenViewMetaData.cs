using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    public class ComponenViewMetaData
    {
        public static KeyName[] componenkeyNames = {
            new KeyName(){Name = "ID", Key = "ID", Visible = false, Editable = false,Save=false,ImportVisible=false},
            new KeyName(){Key="Name",Name="零件名称",Visible = true, Editable = true,NotNull=true},
            new KeyName(){Key="SingleCarUsageAmount",Name="单台用量",Visible = true, Editable = true,NotNull=true,Positive=true},
            new KeyName(){Key="DailyProduction",Name="单日产量",Visible = true, Editable = true,NotNull=true,Positive=true},

            //new KeyName(){Key="DefaultReceiptUnit",Name="默认收货单位",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultReceiptUnitAmount",Name="默认收货单位数量",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultShipmentUnit",Name="默认发货单位",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultShipmentUnitAmount",Name="默认发货单位数量",Visible = true, Editable = true},

            new KeyName(){Key="DefaultSingleBoxPackagingBoxType",Name="默认单箱包装箱类型",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxLength",Name="默认单箱长",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxWidth",Name="默认单箱宽",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxHeight",Name="默认单箱高",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxSNP",Name="默认单箱SNP",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxRatedMinimumBoxCount",Name="默认单箱额定托拍对应小箱数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxWeight",Name="默认单箱重量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxLayerCount",Name="默认单箱包装现状码放层数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxStorageCount",Name="默认单箱包装现状码放个数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxTheoreticalLayerCount",Name="默认单箱理论码放层数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxTheoreticalStorageHeight",Name="默认单箱理论码放高度",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxThroreticalStorageCount",Name="默认单箱理论码放个数",Visible = true, Editable = true},

            new KeyName(){Key="DefaultOuterPackingPhotoIndex",Name="默认外包装照片索引（标签，单包）",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingBoxType",Name="默认外包装包装箱类型",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingLength",Name="默认外包装长",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingWidth",Name="默认外包装宽",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingHeight",Name="默认外包装高",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingSNP",Name="默认外包装SNP",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingComment",Name="默认外包装备注（SNP）",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingRequiredLayers",Name="默认外包装包装要求码放层数",Visible = true, Editable = true},

            new KeyName(){Key="DefaultShipmentInfoBoxType",Name="默认出货箱型",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentInfoBoxLength",Name="默认出货箱体长度",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentInfoBoxWidth",Name="默认出货箱体宽度",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentInfoBoxHeight",Name="默认出货箱体高度",Visible = true, Editable = true},



        };
        public static KeyName[] KeyNames { get => componenkeyNames; set => componenkeyNames = value; }

        public static KeyName[] componenmodifykeyNames = {
            new KeyName(){Name = "ID", Key = "ID", Visible = false, Editable = false,Save=false},
            new KeyName(){Key="Name",Name="零件名称",Visible = true, Editable = true},
            new KeyName(){Key="SingleCarUsageAmount",Name="单台用量",Visible = true, Editable = true},
            new KeyName(){Key="DailyProduction",Name="单日产量",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultReceiptUnit",Name="默认收货单位",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultReceiptUnitAmount",Name="默认收货单位数量",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultShipmentUnit",Name="默认发货单位",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultShipmentUnitAmount",Name="默认发货单位数量",Visible = true, Editable = true},
        };


        public static KeyName[] ComponentShipmentInfokeyNames = {
            new KeyName(){Key="DefaultShipmentInfoBoxType",Name="默认出货箱型",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentInfoBoxLength",Name="默认出货箱体长度",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentInfoBoxWidth",Name="默认出货箱体宽度",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentInfoBoxHeight",Name="默认出货箱体高度",Visible = true, Editable = true},

        };
        public static KeyName[] KeyNames1 { get => ComponentShipmentInfokeyNames; set => ComponentShipmentInfokeyNames = value; }

        public static KeyName[] ComponentSingleBoxTranPackingInfokeyNames = {
            new KeyName(){Key="DefaultSingleBoxPackagingBoxType",Name="默认单箱包装箱类型",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxLength",Name="默认单箱长",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxWidth",Name="默认单箱宽",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxHeight",Name="默认单箱高",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxSNP",Name="默认单箱SNP",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxRatedMinimumBoxCount",Name="默认单箱额定托拍对应小箱数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxWeight",Name="默认单箱重量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxLayerCount",Name="默认单箱包装现状码放层数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxStorageCount",Name="默认单箱包装现状码放个数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxTheoreticalLayerCount",Name="默认单箱理论码放层数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxTheoreticalStorageHeight",Name="默认单箱理论码放高度",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSingleBoxThroreticalStorageCount",Name="默认单箱理论码放个数",Visible = true, Editable = true},


        };
        public static KeyName[] KeyNames2 { get => ComponentSingleBoxTranPackingInfokeyNames; set => ComponentSingleBoxTranPackingInfokeyNames = value; }

        public static KeyName[] ComponentOuterPackingSizekeyNames = {
            new KeyName(){Key="DefaultOuterPackingPhotoIndex",Name="默认外包装照片索引（标签，单包）",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingBoxType",Name="默认外包装包装箱类型",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingLength",Name="默认外包装长",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingWidth",Name="默认外包装宽",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingHeight",Name="默认外包装高",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingSNP",Name="默认外包装SNP",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingComment",Name="默认外包装备注（SNP）",Visible = true, Editable = true},
            new KeyName(){Key="DefaultOuterPackingRequiredLayers",Name="默认外包装包装要求码放层数",Visible = true, Editable = true},


        };
        public static KeyName[] KeyNames3 { get => ComponentOuterPackingSizekeyNames; set => ComponentOuterPackingSizekeyNames = value; }


    }
}
