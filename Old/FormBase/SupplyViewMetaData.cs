using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    public class SupplyViewMetaData
    {
        public static KeyName[] supplykeyNames = {
            new KeyName(){Name = "ID", Key = "ID", Visible = false, Editable = false,Save=false},
            //new KeyName(){Name = "项目ID", Key = "ProjectID",Editable = false,Save=false},
            //new KeyName(){Name = "仓库ID", Key = "WarehouseID", Editable = false,Save=false},
            new KeyName(){Name = "供货商名称", Key = "SupplierName" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择供应商"},
            //new KeyName(){Name = "供货商编号", Key = "SupplierNumber" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "自动生成"},
            new KeyName(){Key="ComponentName",Name="零件名",Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择零件"},
            new KeyName(){Key="No",Name="代号",Visible = true, Editable = true,NotNull=true},
            new KeyName(){Key="Number",Name="编号", Visible = true, Editable = true},

            new KeyName(){Key="ReceiveTimes",Name="到货次数",Visible = true, Editable = true},
            new KeyName(){Key="ShipmentTimes",Name="发货次数",Visible = true, Editable = true},
            new KeyName(){Key="DefaultReceiptUnit",Name="默认收货单位",Visible = true, Editable = true},
            new KeyName(){Key="DefaultReceiptUnitAmount",Name="默认收货单位数量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentUnit",Name="默认发货单位",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentUnitAmount",Name="默认发货单位数量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSubmissionAmount",Name="默认送检数量",Visible = true, Editable = true},
            new KeyName(){Key="SafetyStock",Name="安全库存数量",Visible = true, Editable = true},
            new KeyName(){Key="ValidPeriod",Name="有效期限（天）",Visible = true, Editable = true},

            new KeyName(){Key="Package",Name="套餐",Visible = true, Editable = true},
            new KeyName(){Key="PackageDefaultShipmentAmount",Name="套餐默认发货数量",Visible = true, Editable = true},
            new KeyName(){Key="PackageDefaultShipmentUnit",Name="套餐默认发货单位",Visible = true, Editable = true},
            new KeyName(){Key="PackageDefaultShipmentUnitAmount",Name="套餐默认单位数量",Visible = true, Editable = true },
            new KeyName(){Key="PhotoIndex",Name="照片索引",Visible = true, Editable = true},
            new KeyName(){Key="ContainerNo",Name="容器号",Visible = true, Editable = true},
            new KeyName(){Key="Factroy",Name="工厂",Visible = true, Editable = true},
            new KeyName(){Key="WorkPosition",Name="工位",Visible = true, Editable = true},
            new KeyName(){Key="SupplierType",Name="A系列/B系列供应商",Visible = true, Editable = true},
            new KeyName(){Key="Type",Name="机型区分",Visible = true, Editable = true},
            new KeyName(){Key="Size",Name="尺寸（大件/小件）",Visible = true, Editable = true},

            new KeyName(){Key="Charge1",Name="物流服务费1",Visible = true, Editable = true},
            new KeyName(){Key="Charge2",Name="物流服务费2",Visible = true, Editable = true},

            new KeyName(){Name = "创建用户", Key = "CreateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "创建时间", Key = "CreateTime" , Visible = true,Editable=true,DefaultValueFunc=(()=>DateTime.Now.ToString())},
            new KeyName(){Name = "最后修改用户", Key = "LastUpdateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "最后修改时间", Key = "LastUpdateTime" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "最新供货信息ID", Key = "NewestSupplyID" ,Visible = false    , Editable = false ,ImportVisible=false  },
            new KeyName(){Key="IsHistory",Name="是否历史信息",Visible = false , Editable = false ,ImportVisible=true ,NotNull =true },

        };
        public static KeyName[] KeyNames { get => supplykeyNames; set => supplykeyNames = value; }
       


        public static KeyName[] supplyShipmentInfokeyNames = {
            new KeyName(){Key="ShipmentInfoBoxType",Name="出货箱型",Visible = true, Editable = true},
            new KeyName(){Key="ShipmentInfoBoxLength",Name="出货箱体长度",Visible = true, Editable = true},
            new KeyName(){Key="ShipmentInfoBoxWidth",Name="出货箱体宽度",Visible = true, Editable = true},
            new KeyName(){Key="ShipmentInfoBoxHeight",Name="出货箱体高度",Visible = true, Editable = true},
            new KeyName(){Key="ShipmentInfoUnitAmount",Name="出货单车（箱）数量",Visible = true, Editable = true},

        };
        public static KeyName[] KeyNames1 { get => supplyShipmentInfokeyNames; set => supplyShipmentInfokeyNames = value; }

        public static KeyName[] supplySingleBoxTranPackingInfokeyNames = {
            new KeyName(){Key="SingleBoxPhotoIndex",Name="单箱照片索引（单体，一拖现状）",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxPackagingBoxType",Name="单箱包装箱类型",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxLength",Name="单箱长",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxWidth",Name="单箱宽",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxHeight",Name="单箱高",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxSNP",Name="单箱SNP",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxRatedMinimumBoxCount",Name="单箱额定托拍对应小箱数",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxWeight",Name="单箱重量",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxLayerCount",Name="单箱包装现状码放层数",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxStorageCount",Name="单箱包装现状码放个数",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxTheoreticalLayerCount",Name="单箱理论码放层数",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxTheoreticalStorageHeight",Name="单箱理论码放高度",Visible = true, Editable = true},
            new KeyName(){Key="SingleBoxThroreticalStorageCount",Name="单箱理论码放个数",Visible = true, Editable = true},


        };
        public static KeyName[] KeyNames2 { get => supplySingleBoxTranPackingInfokeyNames; set => supplySingleBoxTranPackingInfokeyNames = value; }

        public static KeyName[] supplyOuterPackingSizekeyNames = {
            new KeyName(){Key="OuterPackingPhotoIndex",Name="外包装照片索引（标签，单包）",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingBoxType",Name="外包装包装箱类型",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingLength",Name="外包装长",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingWidth",Name="外包装宽",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingHeight",Name="外包装高",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingSNP",Name="外包装SNP",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingComment",Name="外包装备注（SNP）",Visible = true, Editable = true},
            new KeyName(){Key="OuterPackingRequiredLayers",Name="外包装包装要求码放层数",Visible = true, Editable = true},


        };
        public static KeyName[] KeyNames3 { get => supplyOuterPackingSizekeyNames; set => supplyOuterPackingSizekeyNames = value; }


        public static KeyName[] importsupplykeyNames = {
            new KeyName(){Name = "供货商名称", Key = "SupplierName" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择供应商" ,Import=false,NotNull =true},
            new KeyName(){Key="ComponentName",Name="零件名",Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择零件" ,Import=false ,NotNull =true},

            new KeyName(){Key="No",Name="代号",Visible = true, Editable = true},
            new KeyName(){Key="Number",Name="编号", Visible = true, Editable = true},

            new KeyName(){Key="ReceiveTimes",Name="到货次数",Visible = true, Editable = true ,NotNull =true},
            new KeyName(){Key="ShipmentTimes",Name="发货次数",Visible = true, Editable = true ,NotNull =true},
            new KeyName(){Key="DefaultReceiptUnit",Name="默认收货单位",Visible = true, Editable = true},
            new KeyName(){Key="DefaultReceiptUnitAmount",Name="默认收货单位数量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentUnit",Name="默认发货单位",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentUnitAmount",Name="默认发货单位数量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSubmissionAmount",Name="默认送检数量",Visible = true, Editable = true},
            new KeyName(){Key="SafetyStock",Name="安全库存数量",Visible = true, Editable = true},
            new KeyName(){Key="ValidPeriod",Name="有效期限（天）",Visible = true, Editable = true},

            new KeyName(){Key="Category",Name="套餐",Visible = true, Editable = true},
            new KeyName(){Key="PhotoIndex",Name="照片索引",Visible = true, Editable = true},
            new KeyName(){Key="ContainerNo",Name="容器号",Visible = true, Editable = true},
            new KeyName(){Key="Factroy",Name="工厂",Visible = true, Editable = true},
            new KeyName(){Key="WorkPosition",Name="工位",Visible = true, Editable = true},
            new KeyName(){Key="SupplierType",Name="A系列/B系列供应商",Visible = true, Editable = true},
            new KeyName(){Key="Type",Name="机型区分",Visible = true, Editable = true},
            new KeyName(){Key="Size",Name="尺寸（大件/小件）",Visible = true, Editable = true},
            new KeyName(){Key="Charge1",Name="物流服务费1",Visible = true, Editable = true},
            new KeyName(){Key="Charge2",Name="物流服务费2",Visible = true, Editable = true},

            //new KeyName(){Name = "创建用户", Key = "CreateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=true,Import=false},
            //new KeyName(){Name = "创建时间", Key = "CreateTime" , Visible = true,Editable=true,DefaultValueFunc=(()=>DateTime.Now.ToString()),ImportVisible=true,Import=false},
            new KeyName(){Name = "最后修改用户", Key = "LastUpdateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "最后修改时间", Key = "LastUpdateTime" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "最新供货信息ID", Key = "NewestSupplyID" ,Visible = false    , Editable = false ,ImportVisible=false  },
            new KeyName(){Key="IsHistory",Name="是否历史信息",Visible = false , Editable = false,ImportVisible=false,NotNull =true },

        };
        public static Dictionary<string, string> keyConvert = new Dictionary<string, string>()
        {
            {"SupplierName","SupplierName"},
            { "ComponentName","ComponentName"},
            { "No","No"},
            { "Number","Number"},

            { "ReceiveTimes","ReceiveTimes"},
            { "ShipmentTimes","ShipmentTimes"},
            { "DefaultReceiptUnit","DefaultReceiptUnit"},
            { "DefaultReceiptUnitAmount","DefaultReceiptUnitAmount"},
            { "DefaultShipmentUnit","DefaultShipmentUnit"},
            { "DefaultShipmentUnitAmount","DefaultShipmentUnitAmount"},
            { "DefaultSubmissionAmount","DefaultSubmissionAmount"},
            { "SafetyStock","SafetyStock"},
            { "ValidPeriod","ValidPeriod"},

            { "PhotoIndex","PhotoIndex"},
            { "ContainerNo","ContainerNo"},
            { "Factroy","Factroy"},
            { "WorkPosition","WorkPosition"},
            { "SupplierType","SupplierType"},
            { "Type","Type"},
            { "Size","Size"},
            { "Category","Category"},
            { "Charge1","Charge1"},
            { "Charge2","Charge2"},

            { "LastUpdateUserUsername","LastUpdateUserUsername"},
            { "LastUpdateTime","LastUpdateTime"}
        };


    }
}
