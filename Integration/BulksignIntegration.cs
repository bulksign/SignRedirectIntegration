using System;
using System.IO;
using Bulksign.Api;

namespace Integration
{
	public class BulksignIntegration
	{
		private const string userEmail = "";
		private const string key = "";


		public BulksignResult<SendEnvelopeResultApiModel> SendEnvelope(string filePath)
		{
			//specify the integration url for on-premise version of Bulksign
			//BulksignApiClient api = new BulksignApiClient("http://bulksign_instance_api_endpoint/");

			//to target bulksign.com, leave empty to target bulksign.com
			BulksignApiClient api = new BulksignApiClient();

			EnvelopeApiModel envelope = new EnvelopeApiModel();
			envelope.Name = "Website Integration Sample";
			envelope.DisableSignerEmailNotifications = true; //no email notifications
			

			RecipientApiModel recipient = new RecipientApiModel();
			recipient.Index = 1;
			recipient.Email = "test@test.com";
			recipient.Name = "Test";
			recipient.RecipientType = RecipientTypeApi.Signer;

			envelope.Recipients = new RecipientApiModel[1] { recipient };


			DocumentApiModel document = new DocumentApiModel();
			document.FileName = "test.pdf";
			document.FileContentByteArray = new FileContentByteArray()
			{
				ContentBytes = File.ReadAllBytes(filePath)
			};

			envelope.Documents = new DocumentApiModel[1] { document };


			//specify that Bulksign should redirect back to us when signer finishes
			//if you want to test with localhost account, the Bulksign instance must be local
			envelope.OverwriteSignSettings = new SignSettingsApiModel()
			{
				AutomaticFinishAfterSigning = true,
				DocumentDownload = SignerDownloadDocumentActionTypeApi.RedirectToUrl, 
				DocumentDownloadRedirectUrl = "http://localhost/WebSignRedirectIntegration/Home/FinishedSigning/?envelopeId={{#id#}}&email={{#email#}}"
			};

			AuthenticationApiModel auth = new AuthenticationApiModel();
			auth.UserEmail = userEmail;
			auth.Key = key;


			BulksignResult<SendEnvelopeResultApiModel> result = api.SendEnvelope(auth, envelope);

			return result;
		}

	}
}