using System.Collections.Generic;
using GMusic.API;
using GMusic.Core.Resources;

namespace GMusic.WP._8.Resources
{
    public class IsolatedSchedulerHelper
    {
        public IsolatedSchedulerHelper()
        {
            IsolatedScheduler = new IsolatedScheduler();
        }

        private IsolatedScheduler IsolatedScheduler;

        public void NewPlayList(IList<Models.GoogleMusicSong> songs)
        {
            IsolatedScheduler.Load();

            // Add new playlist
            IsolatedScheduler.BackgroundManager.NowPlaying = songs;
            IsolatedScheduler.BackgroundManager.NowPlayingIndex = 0;
            IsolatedScheduler.BackgroundManager.IsPlaying = false;
            IsolatedScheduler.BackgroundManager.IsWaitingForSongUrl = false;
            IsolatedScheduler.BackgroundManager.NeedsForcedPlay = true;

            // Tell The Background Agent to update the playlists
            IsolatedScheduler.BackgroundManager.Action = IsolatedScheduler.Manager.Actions.NewPlaylist;

            // Save
            IsolatedScheduler.Save();
        }
    }
}
