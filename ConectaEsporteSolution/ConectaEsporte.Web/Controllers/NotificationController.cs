using Microsoft.AspNetCore.Mvc;
using ConectaEsporte.Web.Models;
using System.Xml.Linq;

namespace ConectaEsporte.Web.Controllers
{
    public class NotificationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

 

        [HttpPost]
        public IActionResult ReturnPayment(FormCollection form)
        {
            try
            {
                string Email = "leandrofonseca.ti@gmail.com";
                string Token = "879D4D4F17C748A18D4DB24C029E5E31";
                string TransacaoID = Request.Form["notificationCode"];//.Params["notificationCode"];
                string Pagina = "https://ws.pagseguro.uol.com.br/v2/transactions/notifications/" + TransacaoID + "?email=" + Email + "&token=" + Token;
                XElement xml = XElement.Load(Pagina);

                // Dados Gerais do Pedido
                var Pedido = new PedidoModel();
                Pedido.TransacaoID = xml.Element("code").Value;
                Pedido.Data = DateTime.Parse(xml.Element("date").Value);
                Pedido.TipoTransacao = int.Parse(xml.Element("type").Value);
                Pedido.Referencia = xml.Element("reference").Value;
                Pedido.Status = int.Parse(xml.Element("status").Value);
                Pedido.Total = decimal.Parse(xml.Element("grossAmount").Value);
                Pedido.TotalDescontos = decimal.Parse(xml.Element("discountAmount").Value);
                Pedido.TotalTaxas = decimal.Parse(xml.Element("feeAmount").Value);
                Pedido.TotalLiquido = decimal.Parse(xml.Element("netAmount").Value);
                Pedido.TotalItensPedido = int.Parse(xml.Element("itemCount").Value);

                // Dados Cliente do Pedido
                Pedido.Cliente.Nome = xml.Element("sender").Element("name").Value;
                Pedido.Cliente.Email = xml.Element("sender").Element("email").Value;
                Pedido.Cliente.TelefoneCodigoArea = xml.Element("sender").Element("phone").Element("areaCode").Value;
                Pedido.Cliente.Telefone = xml.Element("sender").Element("phone").Element("number").Value;
                Pedido.Cliente.Endereco = xml.Element("shipping").Element("address").Element("street").Value;
                Pedido.Cliente.EnderecoNumero = xml.Element("shipping").Element("address").Element("number").Value;
                Pedido.Cliente.Complemento = xml.Element("shipping").Element("address").Element("complement").Value;
                Pedido.Cliente.Bairro = xml.Element("shipping").Element("address").Element("district").Value;
                Pedido.Cliente.Cidade = xml.Element("shipping").Element("address").Element("city").Value;
                Pedido.Cliente.UF = xml.Element("shipping").Element("address").Element("state").Value;
                Pedido.Cliente.Pais = xml.Element("shipping").Element("address").Element("country").Value;
                Pedido.Cliente.CEP = xml.Element("shipping").Element("address").Element("postalCode").Value;

                // Dados de Pagamento do Pedido
                Pedido.Pagamento.Tipo = int.Parse(xml.Element("paymentMethod").Element("type").Value);
                Pedido.Pagamento.Parcelas = int.Parse(xml.Element("installmentCount").Value);
                Pedido.Pagamento.Codigo = int.Parse(xml.Element("paymentMethod").Element("code").Value);

                // Monta Itens do Pedido
                foreach (var Item in xml.Elements("items").Elements("item").ToList())
                {
                    Pedido.Itens.Add(new ItemModel()
                    {
                        ID = int.Parse(Item.Element("id").Value),
                        Descricao = Item.Element("description").Value,
                        Quantidade = int.Parse(Item.Element("quantity").Value),
                        Preco = decimal.Parse(Item.Element("amount").Value)
                    });
                }


                return new JsonResult(new
                {
                    ErrorMessage = "",
                    Data = true,
                    Object = Pedido
                });
            }
            catch (Exception ex)
            {

                return new JsonResult(new
                {
                    ErrorMessage = ex.Message,
                    Data = false
                });
            }

            //Aqui você tem todos os dados e pode prosseguir com a inclusão no banco e regra de negócio.
        }
    }
}
