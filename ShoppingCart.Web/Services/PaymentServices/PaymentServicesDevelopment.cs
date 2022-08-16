// using Stripe;
//
// namespace ShoppingCart.Web.Services
// {
//
//     public class PaymentServices
//     {
//         public async static  Task<PaymentMessaging> PayAsync(string cardNumber, string month,string year, string cvc,int value)
//         {
//             PaymentMessaging Messenger = new PaymentMessaging();
//             StripeConfiguration.ApiKey = "";
//             try
//             {
//                 //Generate token options
//                 var optionsToken = new TokenCreateOptions
//                 {
//                     //choose card as payment method, and create card options
//                     Card = new TokenCardOptions
//                     {
//                         Number = cardNumber,
//                         ExpMonth = month,
//                         ExpYear = year,
//                         Cvc = cvc,
//                     }
//                 };
//                 //create new Token service object
//                 var serviceToken = new TokenService();
//                 //request token
//                 var stripeToken = await serviceToken.CreateAsync(optionsToken);
//
//                 var chargeOptions = new ChargeCreateOptions
//                 {
//                     Amount = value,
//                     Currency = "usd",
//                     Description = "ShoppingCartMVC",
//                     Source = stripeToken.Id,
//                 };
//
//                 var chargeService = new ChargeService();
//                 var charge = await chargeService.CreateAsync(chargeOptions);
//
//                 Messenger.Status = PaymentStatus.Paid;
//                 Messenger.Charge = charge;
//             }
//             catch (Exception e)
//             {
//                 Messenger.Status = PaymentStatus.Failed;
//                 Messenger.Exception = e;
//             }
//
//             return Messenger;
//         }
//     }
//
// }