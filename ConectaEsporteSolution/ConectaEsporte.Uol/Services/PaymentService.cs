using ConectaEsporte.Uol.Entity;
using ConectaEsporte.Uol.Models;
using ConectaEsporte.Uol.Repository;
using System.Globalization;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Xml;
using Uol.PagSeguro.NET8;
using Uol.PagSeguro.NET8.Constants;
using Uol.PagSeguro.NET8.Domain;
using Uol.PagSeguro.NET8.Exception;
using Uol.PagSeguro.NET8.Resources;

namespace ConectaEsporte.Uol.Services
{
    public class PaymentService : IPaymentRepository
    {

        /// <summary>
        /// Construtor.
        /// Define SecurityProtocolType.
        /// </summary>
        public PaymentService()
        {
            //Define protocolos de comunicação.
            //Importante para funcionar SSL e TLS.
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
        }

        public PaymentResponseEntity CreatePayment(PaymentItemEntity entity, AppSetupEntity setup)
        {

            var paymentResponse = new PaymentResponseEntity();
            bool isSandbox = true;
            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

            // Instantiate a new payment request
            PaymentRequest payment = new PaymentRequest();

            // Sets the currency
            payment.Currency = Currency.Brl;

            // Add an item for this payment request
            payment.Items.Add(new Item(entity.Code, entity.Description, 1, entity.Price));


            // Add another item for this payment request
            //payment.Items.Add(new Item("0002", "Notebook Rosa", 2, 150.99m));

            // Sets a reference code for this payment request, it is useful to identify this payment in future notifications.
            payment.Reference = entity.UserReference;

            // Sets shipping information for this payment request
            payment.Shipping = new Shipping();
            payment.Shipping.ShippingType = ShippingType.NotSpecified;

            //Passando valor para ShippingCost
            payment.Shipping.Cost = 0;// 10.00m;

            //payment.Shipping.Address = new Address(
            //    "BRA",
            //    "SP",
            //    "Sao Paulo",
            //    "Jardim Paulistano",
            //    "01452002",
            //    "Av. Brig. Faria Lima",
            //    "1384",
            //    "5o andar"
            //);

            // Sets your customer information.
            payment.Sender = new Sender(
            string.Format("{0} Comprador", entity.UserName),//  "Joao Comprador",
            entity.UserEmail, //    "comprador@uol.com.br",
                new Phone(entity.UserPhoneDDD, entity.UserPhone) // "11", "56273440")
            );

            //SenderDocument document = new SenderDocument(Documents.GetDocumentByType("CPF"), "12345678909");
            //payment.Sender.Documents.Add(document);

            // Sets the url used by PagSeguro for redirect user after ends checkout process
            payment.RedirectUri = new Uri(setup.RedirectUri);

            // Add checkout metadata information
            //payment.AddMetaData(MetaDataItemKeys.GetItemKeyByDescription("CPF do passageiro"), "123.456.789-09", 1);
            //payment.AddMetaData("PASSENGER_PASSPORT", "23456", 1);

            // Another way to set checkout parameters
            //payment.AddParameter("senderBirthday", "07/05/1980");
            //payment.AddIndexedParameter("itemColor", "verde", 1);
            //payment.AddIndexedParameter("itemId", "0003", 3);
            //payment.AddIndexedParameter("itemDescription", "Mouse", 3);
            //payment.AddIndexedParameter("itemQuantity", "1", 3);
            //payment.AddIndexedParameter("itemAmount", "200.00", 3);

            // Add discount per payment method
            //payment.AddPaymentMethodConfig(PaymentMethodConfigKeys.DiscountPercent, 50.00, PaymentMethodGroup.CreditCard);

            // Add installment without addition per payment method
            //payment.AddPaymentMethodConfig(PaymentMethodConfigKeys.MaxInstallmentsNoInterest, 6, PaymentMethodGroup.CreditCard);

            // Add installment limit per payment method
            //payment.AddPaymentMethodConfig(PaymentMethodConfigKeys.MaxInstallmentsLimit, 8, PaymentMethodGroup.CreditCard);

            // Add and remove groups and payment methods
            //List<string> accept = new List<string>();
            //accept.Add(ListPaymentMethodNames.DebitoItau);
            //accept.Add(ListPaymentMethodNames.DebitoHSBC);
            //payment.AcceptPaymentMethodConfig(ListPaymentMethodGroups.CreditCard, accept);

            //List<string> exclude = new List<string>();
            //exclude.Add(ListPaymentMethodNames.Boleto);
            //payment.ExcludePaymentMethodConfig(ListPaymentMethodGroups.Boleto, exclude);

            try
            {
                /// Create new account credentials
                /// This configuration let you set your credentials from your ".cs" file.
                //AccountCredentials credentials = new AccountCredentials("leandrofonseca.ti@gmail.com", "2d51c128-ec11-4133-bb3a-6d4fc6989f8d1c7e61864b5998cd3c0efd2f4a52290f3d32-6433-44a1-a58d-a71553960442");

                /// @todo with you want to get credentials from xml config file uncommend the line below and comment the line above.
                AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);
                Uri paymentRedirectUri = payment.Register(credentials);
                paymentResponse.Uri = paymentRedirectUri;
                paymentResponse.Success = true;
            }
            catch (PagSeguroServiceException exception)
            {
                paymentResponse.MessageError = exception.Message;
                foreach (ServiceError element in exception.Errors)
                {
                    paymentResponse.Errors.Add(element);
                }
            }

            return paymentResponse;
        }



        /// <summary>
        /// Realiza checkout com a conta parametrizada na configuração do sistema.
        /// </summary>
        /// <param name="emailUsuario">E-mail usuário pagseguro.</param>
        /// <param name="token">Token.</param>
        /// <param name="urlCheckout">URL Checkout.</param>
        /// <param name="itens">Itens de venda.</param>
        /// <param name="comprador">Dados do comprador.</param>
        /// <param name="reference">Referência da transação.</param>
        /// <returns></returns>
        public string Checkout(string emailUsuario, string token, string urlCheckout, List<PagSeguroItemDTO> itens, PagSeguroCompradorDTO comprador, string reference)
        {
            //Conjunto de parâmetros/formData.
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("email", emailUsuario);
            postData.Add("token", token);
            postData.Add("currency", "BRL");

            for (int i = 0; i < itens.Count; i++)
            {
                postData.Add(string.Concat("itemId", i + 1), itens[i].itemId);
                postData.Add(string.Concat("itemDescription", i + 1), itens[i].itemDescription);
                postData.Add(string.Concat("itemAmount", i + 1), itens[i].itemAmount);
                postData.Add(string.Concat("itemQuantity", i + 1), itens[i].itemQuantity);
                postData.Add(string.Concat("itemWeight", i + 1), itens[i].itemWeight);
            }

            //Reference.
            postData.Add("reference", reference);

            //Comprador.
            if (comprador != null)
            {
                postData.Add("senderName", comprador.SenderName);
                postData.Add("senderAreaCode", comprador.SenderAreaCode);
                postData.Add("senderPhone", comprador.senderPhone);
                postData.Add("senderEmail", comprador.senderEmail);
            }

            //Shipping.
            postData.Add("shippingAddressRequired", "false");

            //Formas de pagamento.
            //Cartão de crédito e boleto.
            postData.Add("acceptPaymentMethodGroup", "CREDIT_CARD,BOLETO");

            //String que receberá o XML de retorno.
            string xmlString = null;

            //Webclient faz o post para o servidor de pagseguro.
            using (WebClient wc = new WebClient())
            {
                //Informa header sobre URL.
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                //Faz o POST e retorna o XML contendo resposta do servidor do pagseguro.
                var result = wc.UploadValues(urlCheckout, postData);

                //Obtém string do XML.
                xmlString = Encoding.ASCII.GetString(result);
            }

            //Cria documento XML.
            XmlDocument xmlDoc = new XmlDocument();

            //Carrega documento XML por string.
            xmlDoc.LoadXml(xmlString);

            //Obtém código de transação (Checkout).
            var code = xmlDoc.GetElementsByTagName("code")[0];

            //Obtém data de transação (Checkout).
            var date = xmlDoc.GetElementsByTagName("date")[0];

            //Retorna código do checkout.
            return code.InnerText;
        }

        /// <summary>
        /// Consulta por código referência.
        /// </summary>
        /// <param name="emailUsuario">E-mail usuário pagseguro.</param>
        /// <param name="token">Token.</param>
        /// <param name="urlConsultaTransacao">URL consulta transação.</param>
        /// <param name="codigoReferencia">Código de referência.</param>
        /// <returns>DTO com resultados do XML.</returns>
        public ConsultaTransacaoPagSeguroDTO ConsultaPorCodigoReferencia(string emailUsuario, string token, string urlConsultaTransacao, string codigoReferencia)
        {
            //Variável de retorno.
            ConsultaTransacaoPagSeguroDTO retorno = new ConsultaTransacaoPagSeguroDTO();
            retorno.listTransaction = new List<ConsultaTransacaoPagSeguroTransactionDTO>();

            try
            {
                //uri de consulta da transação.
                string uri = string.Concat(urlConsultaTransacao, "?email=", emailUsuario, "&token=", token, "&reference=", codigoReferencia);

                //Classe que irá fazer a requisição GET.
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

                //Método do webrequest.
                request.Method = "GET";

                //String que vai armazenar o xml de retorno.
                string xmlString = null;

                //Obtém resposta do servidor.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //Cria stream para obter retorno.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        //Lê stream.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            //Xml convertido para string.
                            xmlString = reader.ReadToEnd();

                            //Cria xml document para facilitar acesso ao xml.
                            XmlDocument xmlDoc = new XmlDocument();

                            //Carrega xml document através da string com XML.
                            xmlDoc.LoadXml(xmlString);

                            //Resultados na página.
                            //Por padrão é retornado a página 1 com 50 transações.
                            var resultsInThisPage = Convert.ToInt32(xmlDoc.GetElementsByTagName("resultsInThisPage")[0].InnerText);

                            //Total de páginas.
                            var totalPages = Convert.ToInt32(xmlDoc.GetElementsByTagName("totalPages")[0].InnerText);

                            //CurrentPage.
                            var currentPage = Convert.ToInt32(xmlDoc.GetElementsByTagName("currentPage")[0].InnerText);

                            //Obtém lista de Transações.
                            var listTransactions = xmlDoc.GetElementsByTagName("transactions")[0];

                            //Popula retorno.
                            retorno.CurrentPage = currentPage;
                            retorno.TotalPages = totalPages;
                            retorno.ResultsInThisPage = resultsInThisPage;

                            //Usado para conversão de data W3C.
                            string formatStringW3CDate = "yyyy-MM-ddTHH:mm:ss.fffzzz";
                            System.Globalization.CultureInfo cInfoW3CDate = new System.Globalization.CultureInfo("en-US", true);

                            //Popula transações.
                            if (listTransactions != null)
                            {
                                foreach (XmlNode childNode in listTransactions)
                                {
                                    //Cria novo item de transação.
                                    var itemTransacao = new ConsultaTransacaoPagSeguroTransactionDTO();

                                    foreach (XmlNode childNode2 in childNode.ChildNodes)
                                    {
                                        if (childNode2.Name == "date")
                                        {
                                            var date = System.DateTime.ParseExact(childNode2.InnerText, formatStringW3CDate, cInfoW3CDate);
                                            itemTransacao.Date = date;
                                        }
                                        else if (childNode2.Name == "reference")
                                        {
                                            itemTransacao.Reference = childNode2.InnerText;
                                        }
                                        else if (childNode2.Name == "code")
                                        {
                                            itemTransacao.Code = childNode2.InnerText;
                                        }
                                        else if (childNode2.Name == "type")
                                        {
                                            itemTransacao.type = Convert.ToInt32(childNode2.InnerText);
                                        }
                                        else if (childNode2.Name == "status")
                                        {
                                            itemTransacao.Status = Convert.ToInt32(childNode2.InnerText);
                                        }
                                        else if (childNode2.Name == "paymentMethod")
                                        {
                                            foreach (XmlNode nodePaymentMethod in childNode2.ChildNodes)
                                            {
                                                if (nodePaymentMethod.Name == "type")
                                                {
                                                    itemTransacao.PaymentMethodType = Convert.ToInt32(childNode2.InnerText);
                                                }
                                            }
                                        }
                                        else if (childNode2.Name == "grossAmount")
                                        {
                                            itemTransacao.GrossAmount = Convert.ToDouble(childNode2.InnerText, CultureInfo.InvariantCulture);
                                        }
                                        else if (childNode2.Name == "discountAmount")
                                        {
                                            itemTransacao.DiscountAmount = Convert.ToDouble(childNode2.InnerText, CultureInfo.InvariantCulture);
                                        }
                                        else if (childNode2.Name == "feeAmount")
                                        {
                                            itemTransacao.FeeAmount = Convert.ToDouble(childNode2.InnerText, CultureInfo.InvariantCulture);
                                        }
                                        else if (childNode2.Name == "netAmount")
                                        {
                                            itemTransacao.NetAmount = Convert.ToDouble(childNode2.InnerText, CultureInfo.InvariantCulture);
                                        }
                                        else if (childNode2.Name == "extraAmount")
                                        {
                                            itemTransacao.ExtraAmount = Convert.ToDouble(childNode2.InnerText, CultureInfo.InvariantCulture);
                                        }
                                        else if (childNode2.Name == "lastEventDate")
                                        {
                                            var lastEventDate = System.DateTime.ParseExact(childNode2.InnerText, formatStringW3CDate, cInfoW3CDate);
                                            itemTransacao.LastEventDate = lastEventDate;
                                        }
                                    }

                                    //Adiciona item de transação.
                                    retorno.listTransaction.Add(itemTransacao);
                                }
                            }

                            //Fecha reader.
                            reader.Close();

                            //Fecha stream.
                            dataStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Retorno.
            return retorno;
        }

        /// <summary>
        /// Cancela transação com status de "Aguardando Pagamento" ou "Em Análise".
        /// </summary>
        /// <param name="emailUsuario">E-mail usuário pagseguro.</param>
        /// <param name="token">Token.</param>
        /// <param name="urlCancelamento">URL Cancelamento.</param>
        /// <param name="transactionCode">Código da transação.</param>
        /// <returns>Bool. Caso true, transação foi cancelada. Caso false, transação não foi cancelada.</returns>
        public bool CancelarTransacao(string emailUsuario, string token, string urlCancelamento, string transactionCode)
        {
            //Monta url completa para solicitação.
            string urlCompleta = string.Concat(urlCancelamento);

            //String que receberá o XML de retorno.
            string xmlString = null;

            //Webclient faz o post para o servidor de pagseguro.
            using (WebClient wc = new WebClient())
            {
                //Informa header sobre URL.
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                //PostData.
                System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
                postData.Add("email", emailUsuario);
                postData.Add("token", token);
                postData.Add("transactionCode", transactionCode);

                //Faz o POST e retorna o XML contendo resposta do servidor do pagseguro.
                var result = wc.UploadValues(urlCancelamento, postData);

                //Obtém string do XML.
                xmlString = Encoding.ASCII.GetString(result);
            }

            //Cria documento XML.
            XmlDocument xmlDoc = new XmlDocument();

            //Carrega documento XML por string.
            xmlDoc.LoadXml(xmlString);

            //Obtém código de transação (Checkout).
            //Caso ocorra tudo ok, a API do PagSeguro retornará uma tag "result" com o conteúdo "OK"
            //Caso o cancelamento não ocorra, será retornado as tags errors -> error e dentro da error, as tags code e message.
            var xmlResult = xmlDoc.GetElementsByTagName("result");

            //Retorno.
            bool retorno;

            //Verifica se tem a tag resultado.
            if (xmlResult.Count > 0)
            {
                retorno = xmlResult[0].InnerText == "OK";
            }
            else
            {
                retorno = false;
            }

            //Retorno do método.
            return retorno;
        }

        /// <summary>
        /// Nome amigável para status do pagseguro.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <returns>Nome amigável.</returns>
        public string NomeAmigavelStatusPagSeguro(StatusTransacaoEnum status)
        {
            string retorno;

            if (status == StatusTransacaoEnum.NaoExisteTransacao)
            {
                retorno = "Nenhuma Transação Encontrada";
            }
            else if (status == StatusTransacaoEnum.AguardandoPagamento)
            {
                retorno = "Aguardando Pagamento";
            }
            else if (status == StatusTransacaoEnum.EmAnalise)
            {
                retorno = "Em Análise";
            }
            else if (status == StatusTransacaoEnum.Paga)
            {
                retorno = "Pago";
            }
            else if (status == StatusTransacaoEnum.Disponivel)
            {
                retorno = "Disponível";
            }
            else if (status == StatusTransacaoEnum.EmDisputa)
            {
                retorno = "Em Disputa";
            }
            else if (status == StatusTransacaoEnum.Devolvida)
            {
                retorno = "Devolvida";
            }
            else if (status == StatusTransacaoEnum.Cancelada)
            {
                retorno = "Cancelada";
            }
            else if (status == StatusTransacaoEnum.Debitado)
            {
                retorno = "Debitado (Devolvido)";
            }
            else if (status == StatusTransacaoEnum.RetencaoTemporaria)
            {
                retorno = "Retenção Temp.";
            }
            else
            {
                throw new Exception("Falha ao resolver status pagseguro.");
            }

            return retorno;
        }

        /// <summary>
        /// Nome amigável para tipo de pagamento do pagseguro.
        /// </summary>
        /// <param name="tipoPagamento">Tipo do pagamento.</param>
        /// <returns>Tipo pagamento.</returns>
        public string NomeAmigavelTipoPagamentoPagSeguro(TipoPagamentoEnum tipoPagamento)
        {
            string retorno;

            if (tipoPagamento == TipoPagamentoEnum.Boleto)
            {
                retorno = "Boleto";
            }
            else if (tipoPagamento == TipoPagamentoEnum.CartaoDeCredito)
            {
                retorno = "Cartão de Crédito";
            }
            else if (tipoPagamento == TipoPagamentoEnum.DebitoOnLineTEF)
            {
                retorno = "Débito Online (TEF)";
            }
            else if (tipoPagamento == TipoPagamentoEnum.OiPago)
            {
                retorno = "Oi Pago";
            }
            else if (tipoPagamento == TipoPagamentoEnum.SaldoPagSeguro)
            {
                retorno = "Saldo PagSeguro";
            }
            else if (tipoPagamento == TipoPagamentoEnum.DepositoEmConta)
            {
                retorno = "Depósito em Conta";
            }
            else
            {
                throw new Exception("Falha ao resolver tipo pagamento pagseguro.");
            }

            return retorno;
        }


    }
}
