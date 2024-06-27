using ConectaEsporte.Uol.Models;
using ConectaEsporte.Uol.Repository;
using System.Net;
using System.Numerics;
using System.Reflection;
using Uol.PagSeguro;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Resources;

namespace ConectaEsporte.Uol.Services
{
    public class PaymentService : IPaymentRepository
    {

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


    }
}
