using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace QPOS2008
{
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.Web.Services.WebServiceBindingAttribute(Name = "CVoucherSoap", Namespace = "http://eznz.com/")]
	class csVoucher : System.Web.Services.Protocols.SoapHttpClientProtocol
	{
		public csVoucher(string Url)
		{
			this.Url = Url;
		}

		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://eznz.com/CreateVoucher", RequestNamespace = "http://eznz.com", ResponseNamespace = "http://eznz.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string CreateVoucher(string vip_barcode, string password, string invoice_number)
		{
			object[] results = this.Invoke("CreateVoucher", new object[] { vip_barcode, password, invoice_number });
			return ((string)(results[0]));
		}
		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://eznz.com/UseVoucher", RequestNamespace = "http://eznz.com", ResponseNamespace = "http://eznz.com", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		public string UseVoucher(string voucher_barcode, string auth_key)
		{
			object[] results = this.Invoke("UseVoucher", new object[] { voucher_barcode, auth_key });
			return ((string)(results[0]));
		}
	}
}