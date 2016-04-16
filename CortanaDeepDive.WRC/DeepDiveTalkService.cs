using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace CortanaDeepDive.WRC
{
	public sealed class DeepDiveTalkService : IBackgroundTask
	{
		BackgroundTaskDeferral serviceDeferral;
		VoiceCommandServiceConnection voiceCommandServiceConnection;

		public async void Run(IBackgroundTaskInstance taskInstance)
		{
			try
			{
				// Take a service deferral so the service isn't terminated
				serviceDeferral = taskInstance.GetDeferral();

				// if cancelled, set deferral
				taskInstance.Canceled += OnTaskCanceled;

				// get the trigger details
				var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

				// is this something we're expecting (from our defined VCD file)?
				// if so, handle it!
				if (triggerDetails != null && triggerDetails.Name == "DeepDiveTalkService") // use to make absolutely sure you want this code to execute
				{
					// get the connection to the voice command service
					voiceCommandServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);

					// set the deferral when the command is completed 
					voiceCommandServiceConnection.VoiceCommandCompleted += VoiceCommandCompleted;

					// get the actual voice command
					var voiceCommand = await voiceCommandServiceConnection.GetVoiceCommandAsync();

					// is there actually a voice Command?
					if (voiceCommand != null)
					{
						// figure out which one and what to do with it
						switch (voiceCommand.CommandName)
						{
							case "ccd001ReadOutLoud":
								var targetFileKey = "targetFile";
								string targetFile = null;
								if (voiceCommand.SpeechRecognitionResult != null
									&& voiceCommand.SpeechRecognitionResult.SemanticInterpretation != null
									&& voiceCommand.SpeechRecognitionResult.SemanticInterpretation.Properties != null
									&& voiceCommand.SpeechRecognitionResult.SemanticInterpretation.Properties.Keys.Contains(targetFileKey))
								{
									targetFile = voiceCommand.SpeechRecognitionResult.SemanticInterpretation.Properties[targetFileKey][0];
								}

								var content = await GetFileContentAsString(targetFile);
								await SendCompletionMessageForReadFileOutLoudRequest(content, targetFile);
								break;
							default:
								LaunchAppInForeground();
								break;
						}
					}
				}
			}
			finally
			{
				SetDeferralCompleted();
			}
		}


		private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
		{
			// do nothing and set the deferral
			SetDeferralCompleted();
		}


		private void SetDeferralCompleted()
		{
			serviceDeferral?.Complete();
		}

		private async Task SendCompletionMessageForReadFileOutLoudRequest(string content, string targetFile)
		{
			// create a message to display something on the canvas
			var message = new VoiceCommandUserMessage
			{
				DisplayMessage = string.IsNullOrWhiteSpace(targetFile) ? "Here we go..." : string.Format("Reading from \"{0}\"", targetFile),
				SpokenMessage = content
			};

			// create a response to transport the message
			var response = VoiceCommandResponse.CreateResponse(message);

			// queue the response for transport back to the user
			await SendCompletionMessageForRequest(response);
		}

		private async Task SendCompletionMessageForRequest(VoiceCommandResponse response)
		{
			// queue the response for the user spoken by Cortana
			await voiceCommandServiceConnection.ReportSuccessAsync(response);

			// set deferral
			SetDeferralCompleted();
		}

		private async Task<string> GetFileContentAsString(string fileName = null)
		{
			var result = string.Empty;

			// this sets which folder to look for; 
			//     ... on Mobile it's the SD Card
			//     ... on "Desktop" it's where ever you specify... but we'll leave it at D: drive for now
			//     ... and we're diving into a folder called "Cortana"
			var folder = await StorageFolder.GetFolderFromPathAsync(@"D:\Cortana");

			var fileExtension = string.Empty;
			var parsedFileName = string.Empty;

			if (!string.IsNullOrWhiteSpace(fileName))
			{
				fileExtension = !fileName.EndsWith(".txt")
					? ".txt"
					: string.Empty;

				parsedFileName = string.Format("{0}{1}", fileName.Replace(" ", ""), fileExtension);
			}

			// here we're specifying the file name for the demo's sake...
			// ... you can actually grab this from the voice command
			var targetFile = string.IsNullOrWhiteSpace(parsedFileName)
				? "test.txt"
				: parsedFileName;

			var file = await folder.GetFileAsync(targetFile);

			using (var fileStream = (await file.OpenReadAsync()).GetInputStreamAt(0))
			{
				using (var reader = new StreamReader(fileStream.AsStreamForRead()))
				{
					result = await reader.ReadToEndAsync();
				}
			}

			return result;
		}

		private async void LaunchAppInForeground()
		{
			// create a message to show
			var message = new VoiceCommandUserMessage
			{
				SpokenMessage = "Sorry; I couldn't figure out what you wanted to do... so I'm launching the app instead."
			};

			// create a response to transport the message
			var response = VoiceCommandResponse.CreateResponse(message);

			// request app launch with response/message
			await voiceCommandServiceConnection.RequestAppLaunchAsync(response);

			// set deferral
			SetDeferralCompleted();
		}

		private void VoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
		{
			SetDeferralCompleted();
		}
	}
}
