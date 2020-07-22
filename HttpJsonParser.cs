using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QPOS2008
{
	class Latipay
	{
		public class CreateInvoice
		{
			public int code;
			public string message;
			public string messageCN;
			public string invoice_id;
			public string qrcode_pic;
			public string signature;
		}
		public class QueryAnInvoice
		{ 
			public int code;
			public string message;
			public string messageCN;
			public string token;
			public string status;
			public string paid_method;
			public string refund_amount;
			public int open_count;
			public string paid_time;
			public string created_time;
			public int max_open_count;
			public string user_id;
			public string wallet_id;
			public string wallet_name;
			public string currency;
			public int organisation_id;
			public string organisation;
			public string amount;
			public string product_name;
			public string period_time;
			public string customer_order_id;
			public string customer_reference;
			public string return_url;
			public string notify_url;
			public string signature;
			public string invoice_id;
			public string invoice_url;
			public string[] payment_methods;
			public string payer;
			public string payee;
			public string attachments;
			public MarginsData margins;
			public string return_query;
			public string individual;
		}
		public class MarginsData
		{
			public double dd;
			public double alipay;
			public double latipay;
			public double wechat;
		}
	}
	class MyPosMate
	{
		public class PosPayResponse
		{
			public string reference_id;
			public string message;
			public bool status;
		}
		public class PosRefundResponse
		{
			public string nonce_str;
			public string code;
			public string signature;
			public string refund_fee;
			public string refund_trade_no;
			public string refund_pay_time;
			public string message;
			public string refund_state;
			public string sign_type;
			public bool status;
		}
		public class getTransactionDetailsResponse
		{
			public string config_id;
			public string remaining_amount;
			public string refunded_amount;
			public string channel;
			public string status_id;
			public string ref1;
			public string ref2;
			public string created_on;
			public string code_url;
			public string code_data;
			public string increment_id;
			public string message;
			public string status_description;
			public string code;
			public string currency;
			public string grandtotal;
			public bool status;
		}
	}
}
