using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormReceipt
{

    public class ReceiptMetaData
    {
        public static KeyName[] receiptNameKeys =
        {
            new KeyName(){Name = "ID", Key = "ID", Visible = false, Editable = false},
            new KeyName(){Name = "项目ID", Key = "ProjectID",Visible = false, Editable = false},
            new KeyName(){Name = "仓库ID", Key = "WarehouseID", Visible = false, Editable = false},
            new KeyName(){Name = "供应商ID", Key = "SupplierID", Visible = false, Editable = false, Save = true},
            new KeyName(){Name = "单号", Key = "No", Visible = true, Editable = true, EditPlaceHolder = "留空自动生成"},
            new KeyName(){Name = "编号", Key = "Number", Visible = true, Editable = true, EditPlaceHolder = "留空自动生成"},
            new KeyName(){Name = "项目名称", Key = "ProjectName" , Visible = true, Editable = false, Save=false},
            new KeyName(){Name = "供应商编号", Key = "SupplierNumber", Visible = true, Editable = false, Save = false, EditPlaceHolder = "自动生成"},
            new KeyName(){Name = "供货商名称", Key = "SupplierName" , Visible = true, Editable = true, Save=false, EditPlaceHolder = "点击添加供应商"},
            new KeyName(){Name = "仓库名", Key = "WarehouseName" , Visible = true, Editable = false, Save=false},
            new KeyName(){Name = "单据类型", Key = "Type", Visible = true, Editable = true, GetAllValueToComboBox="ReceiptTicket.Type"},
            new KeyName(){Name = "内向交货单号", Key = "InwardDeliverTicketNo", Visible = true, Editable = true},
            new KeyName(){Name = "状态", Key = "State", Visible = true, Editable = false},
            new KeyName(){Name = "是否已经送检",Key = "HasSubmission", Visible = true, Editable = false, ComboBoxItems = new ComboBoxItem[]{ new ComboBoxItem("是", 1), new ComboBoxItem("否", 0)}, Translator = Translator.BoolTranslator },
            new KeyName(){Name = "是否已经生成上架单", Key = "HasPutawayTicket", Visible = true, Editable = false, Save = true, EditPlaceHolder = "自动生成"},
            new KeyName(){Name = "收货日期", Key = "ReceiptDate", Visible = true, Editable = true, Save = true},
            new KeyName(){Name = "送货单号（SRM)", Key = "DeliverTicketNoSRM", Visible = true, Editable = true},
            new KeyName(){Name = "凭证来源", Key = "VoucherSource", Visible = true, Editable = true},
            new KeyName(){Name = "凭证号", Key = "VoucherNo", Visible = true, Editable = true},
            new KeyName(){Name = "凭证行号", Key = "VoucherLineNo", Visible = true, Editable = true},
            new KeyName(){Name = "凭证年", Key = "VoucherYear", Visible = true, Editable = true},
            new KeyName(){Name = "关联凭证号", Key = "ReletedVoucherNo", Visible = true, Editable = true},
            new KeyName(){Name = "关联凭证行号", Key = "ReletedVoucherLineNo", Visible = true, Editable = true},
            new KeyName(){Name = "关联凭证年", Key = "ReletedVoucherYear", Visible = true, Editable = true},
            new KeyName(){Name = "抬头文本", Key = "HeadingText", Visible = true, Editable = true},
            new KeyName(){Name = "过账日期", Key = "PostCountDate", Visible = true, Editable = true},
            new KeyName(){Name = "内向交货行号", Key = "InwardDeliverLineNo", Visible = true, Editable = true},
            new KeyName(){Name = "外向交货单号", Key = "OutwardDeliverTicketNo", Visible = true, Editable = true},
            new KeyName(){Name = "外向交货行号", Key = "OutwardDeliverLineNo", Visible = true, Editable = true},
            new KeyName(){Name = "采购订单", Key = "PurchaseTicketNo", Visible = true, Editable = true},
            new KeyName(){Name = "采购订单行号", Key = "PurchaseTicketLineNo", Visible = true, Editable = true},
            new KeyName(){Name = "订单日期", Key = "OrderDate", Visible = true, Editable = true},
            new KeyName(){Name = "收货库位", Key = "ReceiptStorageLocation", Visible = true, Editable = true},
            new KeyName(){Name = "托盘号", Key = "BoardNo", Visible = true, Editable = true},
            new KeyName(){Name = "收货包装", Key = "ReceiptPackage", Visible = true, Editable = true},
            //new KeyName(){Name = "收货数量", Key = "ReceiptCount", Visible = true, Editable = true},
            new KeyName(){Name = "移动类型", Key = "MoveType", Visible = true, Editable = true, GetAllValueToComboBox = "ReceiptTicket.MoveType"},
            new KeyName(){Name = "单据来源", Key = "Source", Visible = true, Editable = true},
            new KeyName(){Name = "负责人ID", Key = "PersonID", Visible = false, Editable = false},
            new KeyName(){Name = "负责人", Key = "PersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName(){Name = "是否过账", Key = "HasPosted", Visible = true, Editable = false, ComboBoxItems = new ComboBoxItem[]{new ComboBoxItem("是", "是"), new ComboBoxItem("否", "否") } },
            //new KeyName(){Name = "箱号", Key = "BoxNo", Visible = true, Editable = true},
            new KeyName(){Name = "创建用户ID", Key = "CreateUserID", Visible = false, Editable = false, Save = true},
            new KeyName(){Name = "创建用户用户名", Key = "CreateUserUsername", Visible = true, Editable = false, Save = false,  EditPlaceHolder = "自动生成"},
            new KeyName(){Name = "创建时间", Key = "CreateTime", Visible = true, Editable = false,  EditPlaceHolder = "自动生成"},
            new KeyName(){Name = "最后修改用户ID", Key = "LastUpdateUserID", Visible = false, Editable = false},
            new KeyName(){Name = "最后修改用户用户名", Key = "LastUpdateUserUsername", Visible = true, Editable = false, Save = false,  EditPlaceHolder = "自动生成"},
            new KeyName(){Name = "最后修改时间", Key = "LastUpdateTime", Visible = true, Editable = false, EditPlaceHolder = "自动生成"},
            //new KeyName()
        };
        public static KeyName[] submissionTicketKeyName =
        {
            new KeyName(){Name="ID", Key="ID",Visible=false,Editable=false, Save = true},
            new KeyName(){Name="收货单ID", Key="ReceiptTicketID",Visible=false,Editable=false, Save = true},
            new KeyName(){Name="送检单单号", Key="No",Visible=true,Editable=true, EditPlaceHolder = "自动生成"},
            new KeyName(){Name="收货单单号", Key="ReceiptTicketNo", Visible = true, Editable = false, Save = false},
            new KeyName(){Name="内向交货单号", Key="ReceiptTicketInwardDeliverTicketNo", Visible = true, Editable = false, Save = false},
            new KeyName(){Name="SAP凭证", Key="SAPNo", Visible=true, Editable = true, Save = true},
            new KeyName(){Name="状态", Key="State",Visible=true,Editable=false},
            new KeyName(){Name="是否生成上架单", Key = "ReceiptTicketHasPutawayTicket", Visible=true,Editable=false, Save=false},
            new KeyName(){Name="有无自检报告", Key="HasSelfInspectionReport",Visible=true,Editable=false, ComboBoxItems = new ComboBoxItem[]{new ComboBoxItem("有", "有"), new ComboBoxItem("无", "无")} },
            new KeyName(){Name="责任人ID", Key="PersonID", Visible=false,Editable=false,Save=true},
            new KeyName(){Name="责任人",Key="PersonName",Visible=true, Editable=true, Save=false, EditPlaceHolder = "点击选择人员"},
            //new KeyName(){Name="送检员ID",Key}
            new KeyName(){Name="送检员ID", Key="DeliverSubmissionPersonID",Visible=false,Editable=false},
            new KeyName(){Name="送检员", Key="DeliverSubmissionPersonName",Visible=true,Editable=true,Save=false, EditPlaceHolder = "点击选择人员"},
            new KeyName(){Name="质检接收员ID", Key="ReceivePersonID",Visible=false,Editable=false},
            new KeyName(){Name="质检接收员", Key="ReceivePersonName",Visible=true,Editable=true, Save = false,EditPlaceHolder = "点击选择人员"},
            new KeyName(){Name="质检员ID", Key="SubmissionPersonID",Visible=false,Editable=false},
            new KeyName(){Name="质检员", Key="SubmissionPersonName",Visible=true,Editable=true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName(){Name="送检时间", Key="SubmissionDate", Visible=true, Editable=false,Save=true},
            //new KeyName(){Name="库房返回", Key="Result",Visible=true,Editable=true},
            new KeyName(){Name="创建用户ID", Key="CreateUserID",Visible=false,Editable=false, Save = true},
            new KeyName(){Name="创建用户用户名", Key="CreateUserUsername", Visible=true, Editable=false, Save=false},
            new KeyName(){Name="创建日期", Key="CreateTime",Visible=true,Editable=false},
            new KeyName(){Name="最后修改用户ID", Key="LastUpdateUserID",Visible=false,Editable=false, Save = true},
            new KeyName(){Name="最后修改用户用户名", Key="LastUpdateUserUsername", Visible=true,Editable=false, Save = false},
            new KeyName(){Name="最后修改时间", Key="LastUpdateTime",Visible=true,Editable=false}
        };

        public static KeyName[] itemsKeyName =
        {
            new KeyName(){Name="ID", Key="ID",Visible=false,Editable=false,Save=true, ImportVisible = false,Import = false},
            new KeyName(){Name="收货单ID", Key="ReceiptTicketID",Visible=false,Editable=false,Save=true, ImportVisible = false,Import = false},
            new KeyName(){Name="零件ID", Key="SupplyID",Visible=false,Editable=false,Save=true, ImportVisible = false,Import = true},
            new KeyName(){Name="零件编号/名称", Key="Component", Visible=false,Editable=false,Save=false,ImportVisible=true,Import=false},
            new KeyName(){Name="零件编号", Key="SupplyNo", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择零件", ImportVisible = false,Import = false},
            new KeyName(){Name="零件名称", Key="ComponentName", Visible=true, Editable = true, Save = false, EditPlaceHolder = "点击选择零件" ,ImportVisible = false, Import = false},
            new KeyName(){Name="已分配上架数", Key = "HasPutwayAmount", Visible = true, Editable = false, Save = true,ImportVisible = false ,Import = false},
            new KeyName(){Name="状态", Key="State", Visible=true, Editable=false,Save=true, Import = false, ImportVisible =false},
            //new KeyName(){Name="收货数量", Key="ReceiviptAmount", Visible=true,Editable=false,Save=true, NotNegative = true,ImportVisible = false, Import = false},
            
            //new KeyName(){Name="收货数量", Key="ReceiviptAmount",Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="订单数量", Key="ExpectedUnitCount",Visible=true,Editable=true,Save=true, NotNull=true ,NotNegative = true},
            //new KeyName(){Name="实收数量", Key="RealReceiptUnitCount", Visible=true,Editable=true,Save=true,NotNegative=true,NotNull=true},
            new KeyName(){Name="订单数量", Key="ExpectedAmount", Visible=true,Editable=true,Save=true,NotNull=true,NotNegative=true, ImportVisible = true, Import = true},
            new KeyName(){Name="实收总数", Key="RealReceiptAmount", Visible=true,Editable=true,Save=true,NotNull=true,NotNegative=true, Import = true, ImportVisible = true},
            new KeyName(){Name="实收数量", Key="RealReceiptUnitCount",Visible=true,Editable=false,Save=true,NotNegative=true, Import = true, ImportVisible = true},
            new KeyName(){Name="单位名称", Key="Unit",Visible=true,Editable=true,Save=true, NotNull = true},
            new KeyName(){Name="单位数量", Key="UnitAmount",Visible=true,Editable=true,Save=true, NotNegative = true, NotNull = true},

            //new KeyName(){Name="包装名称", Key="PackageName",Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="期待包装数量", Key="ExpectedPackageAmount",Visible=true,Editable=true,Save=true, NotNegative = true},
            //new KeyName(){Name="收货数量", Key="ReceiviptAmount",Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="错件数", Key="WrongComponentAmount",Visible=true,Editable=true,Save=true, NotNegative =true},
            //new KeyName(){Name="错件数", Key="WrongComponentUnitCount", Visible=true,Editable=true,Save=true,NotNull=true,NotNegative=true},
            //new KeyName(){Name="错件单位",Key="WrongComponentUnit", Visible=true,Editable=true,Save=true,NotNull=true},
            //new KeyName(){Name="错件单位数量",Key="WrongComponentUnitAmount",Visible=true,Editable=true,Save=true,NotNull=true,NotNegative=true},
            //new KeyName(){Name="短缺数量", Key="ShortageAmount",Visible=true,Editable=true,Save=true, NotNegative = true},

            //new KeyName(){Name="拒收数量", Key="RefuseAmount", Visible=true,Editable=true,Save=true,NotNegative=true},
            new KeyName(){Name="拒收数量", Key="RefuseAmount",Visible=true,Editable=true,Save=true, NotNull=true,NotNegative=true},
            new KeyName(){Name="拒收单位", Key="RefuseUnit",Visible=true,Editable=true,Save=true, NotNull = true},
            new KeyName(){Name="拒收单位数量", Key="RefuseUnitAmount",Visible=true, Editable=true,Save=true,NotNull=true},
            new KeyName(){Name="拒收备注", Key="RefuseComment",Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="不合格数量", Key="DisqualifiedAmount",Visible=true,Editable=true,Save=true, NotNegative = true},
            //new KeyName(){Name="自检不良数量", Key="DisqualifiedUnitCount",Visible=true,Editable=true,Save=true,NotNegative = true,NotNull=true},
            //new KeyName(){Name="自检不良单位", Key="DisqualifiedUnit", Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="自检不良单位数量",Key="DisqualifiedUnitAmount", Visible=true,Editable=true,Save=true, NotNegative=true,NotNull=true},

            new KeyName(){Name="生产日期", Key="ManufactureDate", Visible=true,Editable=true,Save=true},
            new KeyName(){Name="存货日期", Key="InventoryDate",Visible=true,Editable=true,Save=true, NotNull = false },
            new KeyName(){Name="失效日期", Key="ExpiryDate",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="作业人ID", Key = "JobPersonID",Visible = false, Editable = false, Save = true, ImportVisible = false, Import = false},
            new KeyName(){Name="作业人", Key = "JobPersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员", Import = false},
            new KeyName(){Name="确认人ID", Key = "ConfirmPersonID",Visible = false, Editable = false, Save = true, ImportVisible = false, Import = false},
            new KeyName(){Name="确认人", Key = "ConfirmPersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员", Import = false},
            new KeyName(){Name="厂商批号", Key="ManufactureNo",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="物权属性", Key="RealRightProperty",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="批次号", Key="BatchNumber",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="采购订单", Key="PurchaseOrder", Visible=true,Editable=true,Save=true},
            new KeyName(){Name="箱号", Key="BoxNo",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="备注", Key="Comment", Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="零件名称", Key="ComponentName", Visible=false,Editable=false,Save=false},
        };

        public static KeyName[] submissionTicketItemKeyName =
        {
            new KeyName(){Name="ID", Key="ID",Visible=false,Editable=false,Save=true},
            new KeyName(){Name="送检单ID", Key="SubmissionTicketID",Visible=false,Editable=false,Save=true},
            new KeyName(){Name= "零件编号", Key="SupplyNo", Visible = true, Editable = false, Save = false},
            new KeyName(){Name="零件名称", Key="ComponentName", Visible = true, Editable = false, Save = false},
            new KeyName(){Name="行项目", Key="LineItem",Visible=true,Editable=true,Save=true},
            //new KeyName(){Name="物料ID", Key="ComponentID",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="状态", Key="State",Visible=true,Editable=false,Save=true, ComboBoxItems = new ComboBoxItem[] { new ComboBoxItem("合格", "合格"), new ComboBoxItem("不合格", "不合格"), new ComboBoxItem("待检", "待检")} },
            new KeyName(){Name="到货数量", Key="ArriveAmount",Visible=true,Editable=false,Save=true},
            //new KeyName(){Name="单位", Key="Unit",Visible=true,Editable=true,Save=true},
            new KeyName(){Name="送检数量", Key="SubmissionAmount",Visible=true,Editable=false,Save=true},
            new KeyName(){Name="不合格数量", Key="RejectAmount",Visible=true,Editable=true, Save =true, NotNegative = true},
            //new KeyName(){Name="拒收数量", Key="RefuseAmount", Visible=true,Editable=true,Save=true},
            new KeyName(){Name="返回数量", Key="ReturnAmount",Visible=true,Editable=true,Save=true, NotNegative = true},
            new KeyName(){Name="作业人ID", Key = "JobPersonID",Visible = false, Editable = false, Save = true},
            new KeyName(){Name="作业人", Key = "JobPersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName(){Name="确认人ID", Key = "ConfirmPersonID",Visible = false, Editable = false, Save = true},
            new KeyName(){Name="确认人", Key = "ConfirmPersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName(){Name="批次号", Key = "BatchNumber", Visible = true, Editable = true, Save = true},
            new KeyName(){Name="采购订单", Key = "PurchaseOrder", Visible = true, Editable = true, Save = true},
            new KeyName(){Name="备注", Key="Comment",Visible=true,Editable=true,Save=true},
            
            //new KeyName(){Name="库房返回", Key = "Result", Visible=true,Editable=true,Save=true}
        };

        public static KeyName[] putawayTicketKeyName =
        {
            new KeyName{Name = "ID", Key = "ID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "上架单编号", Key = "No", Visible = true, Editable = true, Save = true, EditPlaceHolder= "留空自动生成"},
            new KeyName{Name = "单据类型", Key = "Type", Visible = true, Editable = true, Save = true, GetAllValueToComboBox = "PutawayTicket.Type"},
            new KeyName{Name = "收货单ID", Key = "ReceiptTicketID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "收货单编号", Key = "ReceiptTicketNo", Visible = true, Editable = false, Save = false},
            new KeyName{Name = "状态", Key = "State", Visible = true, Editable = false, Save = true},
            new KeyName{Name = "到货日期", Key = "ReceiptTicketReceiptDate", Visible = true, Editable = false, Save = false},
            //new KeyName{Name = "计划移位数量", Key = "ScheduledDisplacementAmount", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "实际移位数量", Key = "DisplacementAmount", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "分配数量", Key = "DistributeAmount", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "打印次数", Key = "PrintTimes", Visible = true, Editable = false, Save = true},
            new KeyName{Name = "作业组名称", Key = "JobGroupName", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "打印次数", Key = "PrintTimes", Visible = true, Editable = false, Save = false},
            new KeyName{Name = "责任人ID", Key = "PersonID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "责任人", Key = "PersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName{Name = "创建用户ID", Key = "CreateUserID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "创建用户用户名", Key = "CreateUserUsername", Visible = true, Editable = false, Save = false},
            new KeyName{Name = "创建时间", Key = "CreateTime", Visible = true, Editable = false, Save = true},
            new KeyName{Name = "最后操作用户用户ID", Key = "LastUpdateUserID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "最后操作用户",Key = "LastUpdateUserUsername", Visible = true, Editable = false, Save = false},
            new KeyName{Name = "最后更新时间", Key = "LastUpdateTime", Visible = true, Editable = false, Save = true}
        };

        public static KeyName[] putawayTicketItemKeyName =
        {
            new KeyName{Name = "ID", Key = "ID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "上架单ID", Key = "PutawayTicketID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "收货单条目ID", Key = "ReceiptTicketItemID", Visible = false, Editable = false, Save = true},
            new KeyName{Name = "零件编号", Key = "SupplyNo", Visible = true, Editable = false, Save = false},
            new KeyName{Name = "零件名称", Key = "ComponentName",Visible = true ,Editable = false, Save = false},
            new KeyName{Name = "库位库区", Key = "DisplacementPositionNo", Visible = true, Editable = true, Save = true, GetAllValueToComboBox = "PutawayTicketItem.DisplacementPositionNo"},
            new KeyName{Name = "目标库位", Key = "TargetStorageLocation", Visible = true, Editable = true, Save = true, GetAllValueToComboBox = "PutawayTicketItem.TargetStorageLocation"},
            //new KeyName{Name = "单位", Key = "Unit", Visible = true, Editable = true, Save=true },
            //new KeyName{Name = "单位数量", Key = "UnitAmount", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "托盘号", Key = "BoardNo", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "状态", Key = "State", Visible = true, Editable = false, Save = true},
            //new KeyName{Name = "分配数量", Key = "DistrabuteCount", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "上架数量", Key = "UnitCount",Visible = true, Editable = true, Save = true},
            new KeyName{Name = "计划上架总数", Key = "ScheduledMoveCount", Visible = true, Editable = false, Save = true},
            new KeyName{Name = "实际上架总数", Key = "PutawayAmount", Visible = true, Editable = true, Save = true, NotNegative = true},
            new KeyName{Name = "实际上架数", Key = "UnitCount", Visible = true, Editable = false, Save =true},
            new KeyName{Name = "单位", Key = "Unit", Visible = true, Editable = false, Save = true},
            new KeyName{Name = "单位数量", Key = "UnitAmount", Visible = true, Editable = false, Save = true},
            new KeyName{Name="作业人ID", Key = "JobPersonID",Visible = false, Editable = false, Save = true},
            new KeyName{Name="作业人", Key = "JobPersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName{Name="确认人ID", Key = "ConfirmPersonID",Visible = false, Editable = false, Save = true},
            new KeyName{Name="确认人", Key = "ConfirmPersonName", Visible = true, Editable = true, Save = false, EditPlaceHolder = "点击选择人员"},
            new KeyName{Name = "作业时间", Key = "OperateTime", Visible = true, Editable = true, Save = true},
            new KeyName{Name="溢库区数量", Key="StockInfoOverflowAreaAmount", Visible = true, Editable = false, Save = false},
            new KeyName{Name="发货区数量", Key="StockInfoShipmentAreaAmount", Visible = true, Editable = false, Save = false},
        };

        public static KeyName[] supplierKeyName =
        {
            new KeyName{Name = "ID", Key = "ID", Visible = false, Editable = true, Save = true},
            new KeyName{Name = "供货商名称", Key = "Name", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "合同编码", Key = "ContractNo", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "起始有效日期", Key = "StartDate", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "结束有效日期", Key = "EndDate", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "开票日期", Key = "InvoiceDate", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "结算日期", Key = "BalanceDate", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "供货商全称", Key = "FullName", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "纳税人识别号", Key = "TaxpayerNumber", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "地址", Key = "Address", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "电话", Key = "Tel", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "开户行", Key = "BankName", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "帐号", Key = "BankAccount", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "开户行行号", Key = "BankNo", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "邮编", Key = "ZipCode", Visible = true, Editable = true, Save = true},
            new KeyName{Name = "收件人", Key = "RecipientName", Visible = true, Editable = true, Save = true}
        };

        public static KeyName[] componentKeyName =
        {
            //new KeyName{Name = "ID", Key = "ID", Visible = false, Editable = true, Save = true},
            //new KeyName{Name = "项目ID", Key = "ProjectID", Visible = false, Editable = true, Save = true},
            //new KeyName{Name = "仓库ID", Key = "WarehouseID", Visible = false, Editable = true, Save = true},
            //new KeyName{Name = "供货商ID", Key = "SupplierID", Visible = false, Editable = true, Save = true},
            //new KeyName{Name = "零件代号", Key = "No", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "零件名称", Key = "ComponentName", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "供应商", Key = "SupplierName", Visible = true,Editable = true, Save = true},
            //new KeyName{Name = "容器号", Key = "ContainerNo", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "工厂", Key = "Factroy", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "工位", Key = "WorkPosition", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "A系列/B系列供应商", Key = "SupplierType", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "机型区分", Key = "Type", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "尺寸（大件/小件）", Key = "Size", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "分类", Key = "Category", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "分组责任人", Key = "GroupPrincipal", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "单台用量", Key = "ComponentSingleCarUsageAmount", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "物流服务费1", Key = "Charge1", Visible = true, Editable = true, Save = true},
            //new KeyName{Name = "物流服务费2", Key = "Charge2", Visible = true, Editable = true, Save = true},



            //new KeyName(){Name = "ID", Key = "ID", Visible = false, Editable = false,Save=false},
            //new KeyName(){Name = "项目ID", Key = "ProjectID",Visible = false, Editable = false,Save=false},
            //new KeyName(){Name = "仓库ID", Key = "WarehouseID", Visible = false, Editable = false,Save=false},
            //new KeyName(){Name = "供货商名称", Key = "SupplierName" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择供应商"},
            //new KeyName(){Name = "供货商编号", Key = "SupplierNumber" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "自动生成"},

            //new KeyName(){Key="No",Name="代号",Visible = true, Editable = true},
            //new KeyName(){Key="Number",Name="零件编号", Visible = true, Editable = true},
            //new KeyName(){Key="ComponentName",Name="零件名称",Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择零件"},

            //new KeyName(){Key="DefaultReceiptUnit",Name="默认收货单位",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultReceiptUnitAmount",Name="默认收货单位数量",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultShipmentUnit",Name="默认发货单位",Visible = true, Editable = true},
            //new KeyName(){Key="DefaultShipmentUnitAmount",Name="默认发货单位数量",Visible = true, Editable = true},

            //new KeyName(){Key="PhotoIndex",Name="照片索引",Visible = true, Editable = true},
            //new KeyName(){Key="ContainerNo",Name="容器号",Visible = true, Editable = true},
            //new KeyName(){Key="Factroy",Name="工厂",Visible = true, Editable = true},
            //new KeyName(){Key="WorkPosition",Name="工位",Visible = true, Editable = true},
            //new KeyName(){Key="SupplierType",Name="A系列/B系列供应商",Visible = true, Editable = true},
            //new KeyName(){Key="Type",Name="机型区分",Visible = true, Editable = true},
            //new KeyName(){Key="Size",Name="尺寸（大件/小件）",Visible = true, Editable = true},
            //new KeyName(){Key="Category",Name="分类",Visible = true, Editable = true},
            //new KeyName(){Key="GroupPrincipal",Name="分组负责人",Visible = true, Editable = true},
            //new KeyName(){Key="Charge1",Name="物流服务费1",Visible = true, Editable = true},
            //new KeyName(){Key="Charge2",Name="物流服务费2",Visible = true, Editable = true},

            //new KeyName(){Name = "创建用户", Key = "CreateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            //new KeyName(){Name = "创建时间", Key = "CreateTime" , Visible = true,Editable=true,DefaultValueFunc=(()=>DateTime.Now.ToString())},
            //new KeyName(){Name = "最后修改用户", Key = "LastUpdateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            //new KeyName(){Name = "最后修改时间", Key = "LastUpdateTime" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            //new KeyName(){Name = "最新供货信息ID", Key = "NewestSupplyID" ,Visible = false    , Editable = false ,ImportVisible=false  },
            //new KeyName(){Key="IsHistory",Name="是否历史信息",Visible = false , Editable = false ,ImportVisible=true ,NotNull =true },

             new KeyName(){Name = "ID", Key = "ID", Visible = false, Editable = false,Save=false},
            //new KeyName(){Name = "项目ID", Key = "ProjectID",Visible = false, Editable = false,Save=false},
            //new KeyName(){Name = "仓库ID", Key = "WarehouseID", Visible = false, Editable = false,Save=false},
            new KeyName(){Name = "供货商名称", Key = "SupplierName" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择供应商"},
            //new KeyName(){Name = "供货商编号", Key = "SupplierNumber" , Visible = true, Editable = true,Save=false,EditPlaceHolder= "自动生成"},
            new KeyName(){Key="ComponentName",Name="零件名",Visible = true, Editable = true,Save=false,EditPlaceHolder= "点击选择零件"},
            new KeyName(){Key="No",Name="零件代号",Visible = true, Editable = true,NotNull=true},
            new KeyName(){Key="Number",Name="零件编号", Visible = true, Editable = true},


            new KeyName(){Key="DefaultReceiptUnit",Name="默认收货单位",Visible = true, Editable = true},
            new KeyName(){Key="DefaultReceiptUnitAmount",Name="默认收货单位数量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentUnit",Name="默认发货单位",Visible = true, Editable = true},
            new KeyName(){Key="DefaultShipmentUnitAmount",Name="默认发货单位数量",Visible = true, Editable = true},
            new KeyName(){Key="DefaultSubmissionAmount",Name="默认送检数量",Visible = true, Editable = true},
            new KeyName(){Key="SafetyStock",Name="安全库存数量",Visible = true, Editable = true},
            new KeyName(){Key="ValidPeriod",Name="有效期限（天）",Visible = true, Editable = true},

            new KeyName(){Key="PhotoIndex",Name="照片索引",Visible = true, Editable = true},
            new KeyName(){Key="ContainerNo",Name="容器号",Visible = true, Editable = true},
            new KeyName(){Key="Factroy",Name="工厂",Visible = true, Editable = true},
            new KeyName(){Key="WorkPosition",Name="工位",Visible = true, Editable = true},
            new KeyName(){Key="SupplierType",Name="A系列/B系列供应商",Visible = true, Editable = true},
            new KeyName(){Key="Type",Name="机型区分",Visible = true, Editable = true},
            new KeyName(){Key="Size",Name="尺寸（大件/小件）",Visible = true, Editable = true},
            new KeyName(){Key="Category",Name="分类",Visible = true, Editable = true},
            new KeyName(){Key="GroupPrincipal",Name="分组负责人",Visible = true, Editable = true},
            new KeyName(){Key="Charge1",Name="物流服务费1",Visible = true, Editable = true},
            new KeyName(){Key="Charge2",Name="物流服务费2",Visible = true, Editable = true},

            new KeyName(){Name = "创建用户", Key = "CreateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "创建时间", Key = "CreateTime" , Visible = true,Editable=true,DefaultValueFunc=(()=>DateTime.Now.ToString())},
            new KeyName(){Name = "最后修改用户", Key = "LastUpdateUserUsername" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "最后修改时间", Key = "LastUpdateTime" , Visible = true, Editable = false,Save=false,EditPlaceHolder= "自动生成",ImportVisible=false},
            new KeyName(){Name = "最新供货信息ID", Key = "NewestSupplyID" ,Visible = false    , Editable = false ,ImportVisible=false  },
            new KeyName(){Key="IsHistory",Name="是否历史信息",Visible = false , Editable = false ,ImportVisible=true ,NotNull =true },
        };

        public static KeyName[] importPutawayTicket =
        {
            new KeyName(){Name = "零件名称/编号", Key = "Component", ImportVisible = true, Import = false},
            new KeyName(){Name = "计划上架数量(单位)", Key = "ScheduledMoveCount", ImportVisible = true, Import = true},
            new KeyName(){Name = "作业人", Key = "JobPersonName", ImportVisible = true, Import =false},
            new KeyName(){Name = "确认人", Key = "ConfirmPersonName", ImportVisible = true, Import = false}
        };


    }
}