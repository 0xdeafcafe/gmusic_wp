#if WINDOWS_PHONE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Microsoft.Phone.Data.Linq;

namespace MVPTracker.Core.CachingLayer
{
    public class MusicUrlManager : DataContext
    {
        public string ConnectionString { get; private set; }
        public Table<UrlItem> StorageCache;

        public MusicUrlManager(string connectionString)
            : base(connectionString)
        {
            ConnectionString = connectionString;

            if (!DatabaseExists())
                CreateDatabase();

            // Verify Tables
            VerifyTable<UrlItem>();
        }
        public Table<TEntity> VerifyTable<TEntity>() where TEntity : class
        {
            var table = GetTable<TEntity>();
            try
            {
                // can call any function against the table to verify it exists
                table.Any();
            }
            catch (DbException exception)
            {
                if (exception.Message.StartsWith("The specified table does not exist."))
                {
                    var databaseSchemaUpdater = this.CreateDatabaseSchemaUpdater();
                    databaseSchemaUpdater.AddTable<TEntity>();
                    databaseSchemaUpdater.Execute();
                }
                else
                {
                    throw;
                }
            }
            return table;
        }

        public string GetCacheData(string songId)
        {
            var entry = StorageCache.First(item => item.SongId == songId && item.Expires < DateTime.Now);

            return entry == null ? null : entry.Url;
        }
        public void SetCacheData(string songId, string url, DateTime expires)
        {
            #region Read And Delete All Entries
            var cachedDataInDb = StorageCache.Where(t => t.SongId == songId).ToList();
            var cachedDataData = new List<UrlItem>();
            try { cachedDataData = new List<UrlItem>(cachedDataInDb); } catch (Exception) { }
            StorageCache.DeleteAllOnSubmit(cachedDataData);
            #endregion

            #region Add New Data
            StorageCache.InsertOnSubmit(new UrlItem
                                            {
                                                Expires = expires,
                                                SongId = songId,
                                                Url = url
                                            });
            #endregion

            Save();
        }

        public void Save()
        {
            SubmitChanges();
        }
    }

    [Table]
    public class UrlItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _id;
        [Column(Name = "Id", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;

                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        private string _songId;
        [Column(Name = "SongId", AutoSync = AutoSync.OnInsert)]
        public string SongId
        {
            get { return _songId; }
            set
            {
                if (_songId == value) return;

                NotifyPropertyChanging("SongId");
                _songId = value;
                NotifyPropertyChanged("SongId");
            }
        }

        private string _url;
        [Column(Name = "Url", AutoSync = AutoSync.OnInsert)]
        public string Url
        {
            get { return _url; }
            set
            {
                if (_url == value) return;

                NotifyPropertyChanging("Url");
                _url = value;
                NotifyPropertyChanged("Url");
            }
        }

        private DateTime _expires;
        [Column(Name = "Expires", AutoSync = AutoSync.OnInsert)]
        public DateTime Expires
        {
            get { return _expires; }
            set
            {
                if (_expires == value) return;

                NotifyPropertyChanging("Expires");
                _expires = value;
                NotifyPropertyChanged("Expires");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public event PropertyChangingEventHandler PropertyChanging;
        public void NotifyPropertyChanging(String info)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(info));
        }
    }
}
#endif