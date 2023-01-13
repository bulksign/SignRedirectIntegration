using System.Web.Mvc;
using Bulksign.Api;
using Integration;

namespace WebSignRedirectIntegration.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}


		public ActionResult FinishedSigning(string envelopeId, string email)
		{

			ViewBag.EnvelopeId = envelopeId;
			ViewBag.Email = email;

			return View();
		}

		[HttpPost]
		public ActionResult SendEnvelope()
		{
			BulksignResult<SendEnvelopeResultApiModel> result = new BulksignIntegration().SendEnvelope(this.Request.MapPath("~/TestFile/bulksign_test_sample.pdf"));

			if (!result.IsSuccessful)
			{
				//the request failed, log the output here 

				return RedirectToAction("Index");
			}
			
			//if the SendEnvelope request was success, get the signing url for frist recipient and redirect to it 

			string url = result.Response.RecipientAccess[0].SigningUrl;
			
			return RedirectPermanent(url);
		}
	}
}