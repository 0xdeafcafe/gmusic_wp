using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.BackgroundAudio;
using GMusic.API;
using MVPTracker.Core.CachingLayer;
using GMusic.Agent._8.Resources;
using System.Windows.Threading;

namespace GMusic.Agent._8
{
    public class AudioPlayer : AudioPlayerAgent
    {
        /// <remarks>
        /// AudioPlayer instances can share the same process.
        /// Static fields can be used to share state between AudioPlayer instances
        /// or to communicate with the Audio Streaming agent.
        /// </remarks>
        static AudioPlayer()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });

            // Initalize IsolatedStorage
            IsolatedStorage = new IsolatedStorage();
            IsolatedStorage.Load();

            // Initalize IsolatedScheduler
            IsolatedScheduler = new IsolatedScheduler();
            IsolatedScheduler.Load();

            // Initalize Api Manager
            ApiManager = new Manager();
            
            // Login
            Login();

            // Schedule Timer
            ScheduleTimer = new DispatcherTimer();
            ScheduleTimer.Interval = new TimeSpan(0, 0, 1);
            ScheduleTimer.Tick += ScheduleTimer_Action;
        }

        #region Functions
        private static void ScheduleTimer_Action(object sender, EventArgs eventArgs)
        {
            IsolatedScheduler.Load();

            if (IsLoggedIn)
            {
                if (!IsolatedSchedulerManager.IsPlaying && !IsolatedSchedulerManager.IsWaitingForSongUrl)
                {
                    // oh no, start playin son
                    PlaySong(0);
                }
            }
        }

        public static async Task<string> Login()
        {
            var tcs = new TaskCompletionSource<string>();

            ApiManager.OnError += tcs.SetException;
            ApiManager.Login(IsolatedStorage.GoogleAuthToken);
            ApiManager.OnLoginComplete += (sender, args) =>
                                              {
                                                  IsLoggedIn = true;
                                                  tcs.SetResult("done");
                                              };
            
            return tcs.Task.Result;
        }
        #endregion

        #region PlaylistModification
        public static void PlaySong(int index)
        {
            IsolatedSchedulerManager.NowPlayingIndex = index;
        }
        #endregion

        public static DispatcherTimer ScheduleTimer;

        /// <summary>
        /// 
        /// </summary>
        public static bool IsLoggedIn = false;

        /// <summary>
        /// 
        /// </summary>
        public static IsolatedScheduler IsolatedScheduler;
        public static IsolatedScheduler.Manager IsolatedSchedulerManager
        {
            get { return IsolatedScheduler.BackgroundManager; }
            set { IsolatedScheduler.BackgroundManager = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IsolatedStorage IsolatedStorage;

        /// <summary>
        /// Provides a pretty chill place to store Cached Url's
        /// </summary>
        public static MusicUrlManager UrlManager;

        /// <summary>
        /// Provides a pretty chill place to store Cached Url's
        /// </summary>
        public static Manager ApiManager;

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Called when the playstate changes, except for the Error state (see OnError)
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time the playstate changed</param>
        /// <param name="playState">The new playstate of the player</param>
        /// <remarks>
        /// Play State changes cannot be cancelled. They are raised even if the application
        /// caused the state change itself, assuming the application has opted-in to the callback.
        ///
        /// Notable playstate events:
        /// (a) TrackEnded: invoked when the player has no current track. The agent can set the next track.
        /// (b) TrackReady: an audio track has been set and it is now ready for playack.
        ///
        /// Call NotifyComplete() only once, after the agent request has been completed, including async callbacks.
        /// </remarks>
        protected override void OnPlayStateChanged(BackgroundAudioPlayer player, AudioTrack track, PlayState playState)
        {
            switch (playState)
            {
                case PlayState.TrackEnded:
                    //player.Track = GetPreviousTrack();
                    break;
                case PlayState.TrackReady:
                    player.Play();
                    break;
                case PlayState.Shutdown:
                    // TODO: Handle the shutdown state here (e.g. save state)
                    break;
                case PlayState.Unknown:
                    break;
                case PlayState.Stopped:
                    break;
                case PlayState.Paused:
                    break;
                case PlayState.Playing:
                    break;
                case PlayState.BufferingStarted:
                    break;
                case PlayState.BufferingStopped:
                    break;
                case PlayState.Rewinding:
                    break;
                case PlayState.FastForwarding:
                    break;
            }

            NotifyComplete();
        }

        /// <summary>
        /// Called when the user requests an action using application/system provided UI
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track playing at the time of the user action</param>
        /// <param name="action">The action the user has requested</param>
        /// <param name="param">The data associated with the requested action.
        /// In the current version this parameter is only for use with the Seek action,
        /// to indicate the requested position of an audio track</param>
        /// <remarks>
        /// User actions do not automatically make any changes in system state; the agent is responsible
        /// for carrying out the user actions if they are supported.
        ///
        /// Call NotifyComplete() only once, after the agent request has been completed, including async callbacks.
        /// </remarks>
        protected override void OnUserAction(BackgroundAudioPlayer player, AudioTrack track, UserAction action, object param)
        {
            switch (action)
            {
                case UserAction.Play:
                    if (player.PlayerState != PlayState.Playing)
                    {
                        player.Play();
                    }
                    break;
                case UserAction.Stop:
                    player.Stop();
                    break;
                case UserAction.Pause:
                    player.Pause();
                    break;
                case UserAction.FastForward:
                    player.FastForward();
                    break;
                case UserAction.Rewind:
                    player.Rewind();
                    break;
                case UserAction.Seek:
                    player.Position = (TimeSpan)param;
                    break;
                case UserAction.SkipNext:
                    //player.Track = GetNextTrack();
                    break;
                case UserAction.SkipPrevious:
                    //AudioTrack previousTrack = GetPreviousTrack();
                    //if (previousTrack != null)
                    //{
                    //    player.Track = previousTrack;
                    //}
                    break;
            }

            NotifyComplete();
        }

        /// <summary>
        /// Called whenever there is an error with playback, such as an AudioTrack not downloading correctly
        /// </summary>
        /// <param name="player">The BackgroundAudioPlayer</param>
        /// <param name="track">The track that had the error</param>
        /// <param name="error">The error that occured</param>
        /// <param name="isFatal">If true, playback cannot continue and playback of the track will stop</param>
        /// <remarks>
        /// This method is not guaranteed to be called in all cases. For example, if the background agent
        /// itself has an unhandled exception, it won't get called back to handle its own errors.
        /// </remarks>
        protected override void OnError(BackgroundAudioPlayer player, AudioTrack track, Exception error, bool isFatal)
        {
            if (isFatal)
            {
                Abort();
            }
            else
            {
                NotifyComplete();
            }

        }

        /// <summary>
        /// Called when the agent request is getting cancelled
        /// </summary>
        /// <remarks>
        /// Once the request is Cancelled, the agent gets 5 seconds to finish its work,
        /// by calling NotifyComplete()/Abort().
        /// </remarks>
        protected override void OnCancel()
        {

        }
    }
}