using Microsoft.AspNetCore.Mvc;
using StoreFront.UI.MVC.Models;
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;


namespace StoreFront.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Contact()
        {
            {
                return View();

            }
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            //When a class has validation attributes, that validation should be checked BEFORE attempting to process any of the data provided.
            if (!ModelState.IsValid)
            {
                //Send them back to the form. We can pass the object to the view so the form will contain the original information they provided.
                return View(cvm);
            }

            //To handle sending an email, we'll need to install another NuGet package and add a few using statements.
            #region Email Setup Steps & Email Info

            //1. Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution
            //2. Go to the Browse tab and search for NETCore.MailKit
            //3. Click NETCore.MailKit
            //4. On the right, check the box next to the CORE1 project, then click "Install"
            //5. Once installed, return here
            //6. Add the following using statements & comments:
            //      - using MimeKit; //Added for access to MimeMessage class
            //      - using MailKit.Net.Smtp; //Added for access to SmtpClient class
            //7. Once added, return here to continue coding email functionality

            //MIME - Multipurpose Internet Mail Extensions - Allows email to contain information other than ASCII, including audio, video, images, HTML, etc.

            //SMTP - Simple Mail Transfer Protocol - An internet protocol (similar to HTTP)
            //that specializes in the collection & transfer of email data.
            #endregion

            //Create the format for the message content we will recieve from the contact form
            string message = $"You have recieved a new email from your site's contact form!<br/>" +
                $"Sender: {cvm.Name}<br/>" +
                $"Email: {cvm.Email}<br/>" +
                $"Subject: {cvm.Subject}<br/>" +
                $"Message: {cvm.Message}";

            //Create a MimeMessage object to assist with storing/transporting the email information from the contact form.

            var mm = new MimeMessage();

            //We can access the credentials for the email user from our appsettings.json file as shown below.

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            //The recipient of this email will be our personal email address, also stored in appsettings.json
            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            //The subject will be the one provided by the user, which is stored in the cvm object

            mm.Subject = cvm.Subject;

            //The body of the message will be formatted with the string we created above
            mm.Body = new TextPart("HTML") { Text = message };

            //We can set the priority of the message as "urgent" so it will be flagged in our email client.

            mm.Priority = MessagePriority.Urgent;

            //We can also add the user's provided email address to the list of ReplyTo addresses.
            //We can do this so our replies are sent directly to them instead of responding to our email user.
            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            //The using directive will create the SmtpClient object used to send the email.
            //Once all of the code inside of the using directive's scope has been executed,
            //it will close any open connections and dispose of the object for us.
            using (var client = new SmtpClient())
            {
                //Connect to the mail server using credentials in appsettings.json

                client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                //Log in to the mail server using the credentials for our email user
                client.Authenticate(
                    //Username
                    _config.GetValue<string>("Credentials:Email:User"),

                    //Password
                    _config.GetValue<string>("Credentials:Email:Password")

                    );

                //It's possible the mail server may be down when the user attempts to contact us,
                //or that we have errors somewhere in our code. So, we can "encapsulate" our code
                //to send the message in a try/catch block.

                try
                {
                    //Try to send the email
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    //If there is an issue, we can store an error message in a ViewBag variable
                    //to be displayed in the View.
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please" +
                        $"try again later.<br/>Error Message: {ex.StackTrace}";

                    //Return the user to the Contact View with their form information intact
                    return View(cvm);
                }

                //If all goes well, return a view that displays a confirmation to the
                //user that their email was sent


            }//As of this line, the client object will be diposed of automatically

            //If all goes well, return a view that displays a confirmation to the
            //user that their email was sent.
            return View("EmailConfirmation", cvm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}