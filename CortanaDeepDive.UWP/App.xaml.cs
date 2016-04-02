using CortanaDeepDive.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CortanaDeepDive.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			var _rootFrame = EnsureRootFrameIsInitialized();

			if (_rootFrame.Content == null)
			{
				_rootFrame.Navigate(typeof(MainPage), e.Arguments);
			}

			// Ensure the current window is active
			Window.Current.Activate();
		}

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

		//// We don't actually need this because we're not using the "Navigate" node in the General.xml voice command definition file
		//protected override void OnActivated(IActivatedEventArgs args)
		//{
		//	base.OnActivated(args);

		//	// if this isn't a voice command activatin then exit method
		//	if (args.Kind != ActivationKind.VoiceCommand) { return; }

		//	// Handle Cortana activation...


		//	// parse the args to get the "stuff"
		//	var speechRecognitinoResult = new ParsedVoiceCommandResult(args);

		//	// assume that we'll need force navigation to MainPage unless we can prove otherwise
		//	var forciblyNavigate = true;

		//	// get the navigation target based on the perceived command
		//	Type navigationTarget = null;

		//	var navigationTargeString = speechRecognitinoResult.SemanticInterpretations["NavigationTarget"][0].Replace(".xaml", string.Empty);
		//	if (!string.IsNullOrWhiteSpace(navigationTargeString))
		//	{
		//		navigationTarget = Type.GetType(string.Format("{0}.{1}", typeof(MainPage).Namespace, navigationTargeString));

		//		// if we have a target then we've proven we don't need to forcibly navigate
		//		if (navigationTarget != null) { forciblyNavigate = false; }
		//	}

		//	// if nothing was detected, forcibly set the navigation target to MainPage
		//	if (forciblyNavigate)
		//	{
		//		navigationTarget = typeof(MainPage);
		//	}

		//	// enure that the root frame is initialized
		//	var _rootFrame = EnsureRootFrameIsInitialized();

		//	// handle navigation to the target destination... throw an exception if not successful
		//	if (!_rootFrame.Navigate(navigationTarget, speechRecognitinoResult))
		//	{
		//		throw new Exception("Failed to parse the voice command!");
		//	}
		//}

		private Frame EnsureRootFrameIsInitialized()
		{
			var _rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (_rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				_rootFrame = new Frame();

				_rootFrame.NavigationFailed += OnNavigationFailed;

				// Place the frame in the current Window
				Window.Current.Content = _rootFrame;
			}

			return _rootFrame;
		}

		public async static void LoadVoiceCommandFile()
		{
			var vcdFileUri = new Uri("ms-appx:///VoiceCommandDefinitions/General.xml");
			var vcdFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(vcdFileUri);
			await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdFile);
		}
	}
}
