using System;
using System.Threading.Tasks;
using MercadoPago.Client.Common;
using MercadoPago.Client;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using ConectaEsporte.Core.Services.Repositories;

namespace ConectaEsporte.Core.Services
{
    public class MercadoPagoService : IMercadoPago
    {
        //"APP_USR-4042451558766693-060410-0b9e1b55ce41203952b1b723ca56fdfb-86179409";
        private string _accessToken = "TEST-4042451558766693-060410-adc658bc4eb53d805f7b3a609fc9d036-86179409";
        public async Task<Payment> SendPaymentVisa()
        {

            MercadoPagoConfig.AccessToken = _accessToken;

            var request = new PaymentCreateRequest
            {
                TransactionAmount = 10,
                Token = "CARD_TOKEN",
                Description = "Payment description",
                Installments = 1,
                PaymentMethodId = "visa",
                Payer = new PaymentPayerRequest
                {
                    Email = "test.payer@email.com",
                }
            };

            var client = new PaymentClient();
            Payment payment = await client.CreateAsync(request);

            Console.WriteLine($"Payment ID: {payment.Id}");

            return payment;
        }

        public async Task<Payment> SendPaymentPix()
        {
            try
            {
                MercadoPagoConfig.AccessToken = _accessToken;

                var requestOptions = new RequestOptions();
                requestOptions.CustomHeaders.Add("x-idempotency-key", "<SOME_UNIQUE_VALUE>");

                var request = new PaymentCreateRequest
                {
                    TransactionAmount = 105,
                    Description = "Título do produto",
                    PaymentMethodId = "pix",
                    Payer = new PaymentPayerRequest
                    {
                        Email = "leandrofonseca.ti@gmail.com",
                        FirstName = "Test",
                        LastName = "User",
                        Identification = new IdentificationRequest
                        {
                            Type = "CPF",
                            Number = "191191191-00",
                        },
                    },
                };

                var client = new PaymentClient();
                Payment payment = await client.CreateAsync(request, requestOptions);

                return payment;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
