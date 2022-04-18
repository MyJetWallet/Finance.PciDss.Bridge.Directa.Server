using System.Diagnostics;
using Finance.PciDss.Abstractions;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments;
using Finance.PciDss.PciDssBridgeGrpc;
using Flurl;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Extensions
{
    public static class MapperExtensions
    {
        public static CreateDirectaInvoiceRequest ToDirectaRestModel(this IPciDssInvoiceModel model, string country2,
            SettingsModel settingsModel, Document document)
        {
            var lastName = model.GetLastName();
            var firstName = model.GetName();


            var activity = Activity.Current;

            var createDirectaInvoiceRequest = new CreateDirectaInvoiceRequest
            {
                Amount = model.Amount,
                OrderId = model.OrderId,
                CancelUrl = settingsModel.DirectaRedirectUrl
                    .SetQueryParam("orderId", model.OrderId)
                    .SetQueryParam("status", "cancel").ToString()
                    .SetQueryParam("activity", activity?.Id).ToString(),
                ErrorUrl = settingsModel.DirectaRedirectUrl
                    .SetQueryParam("orderId", model.OrderId)
                    .SetQueryParam("status", "error").ToString()
                    .SetQueryParam("activity", activity?.Id).ToString(),
                SuccessUrl = settingsModel.DirectaRedirectUrl
                    .SetQueryParam("orderId", model.OrderId)
                    .SetQueryParam("status", "success").ToString()
                    .SetQueryParam("activity", activity?.Id).ToString(),
                NotifyUrl = settingsModel.DirectaNotifyUrl
                    .SetQueryParam("activity", activity?.Id).ToString(),
                Country = country2,
                Currency = model.Currency,
                Payer = new Payer
                {
                    Address = new Address
                    {
                        City = model.City,
                        Street = model.Address
                        //ZipCode = model.Zip
                    },
                    Email = model.Email,
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = model.PhoneNumber,
                    TraderId = model.TraderId,
                    Document = document.Value,
                    DocumentType = document.Type
                },
                Card = new Card
                {
                    CardNumber = model.CardNumber,
                    Cvv = model.Cvv,
                    ExpirationMonth = model.ExpirationDate.Month.ToString(),
                    ExpirationYear = model.ExpirationDate.ToString("yy"),
                    HolderName = model.FullName
                }
            };

            return createDirectaInvoiceRequest;
        }
    }
}
