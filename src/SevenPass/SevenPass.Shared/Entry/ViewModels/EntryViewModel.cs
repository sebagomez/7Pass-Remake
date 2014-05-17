﻿using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using SevenPass.Services.Cache;

namespace SevenPass.Entry.ViewModels
{
    public class EntryViewModel : Conductor<IEntrySubViewModel>.Collection.OneActive
    {
        private readonly ICacheService _cache;
        private readonly IEntrySubViewModel[] _views;

        private string _databaseName;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                _databaseName = value;
                NotifyOfPropertyChange(() => DatabaseName);
            }
        }

        /// <summary>
        /// Gets or sets the entry UUID.
        /// </summary>
        public string Id { get; set; }

        public EntryViewModel(ICacheService cache,
            IEnumerable<IEntrySubViewModel> views)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            if (views == null) throw new ArgumentNullException("views");

            _cache = cache;
            _views = views.ToArray();
        }

        protected override void OnInitialize()
        {
            Items.AddRange(_views);
            DatabaseName = _cache.Database.Name;

            var entry = _cache.GetEntry(Id);
            if (entry == null)
            {
                // TODO: handle entry not found
                return;
            }

            foreach (var view in _views)
                view.Loads(entry);
        }
    }
}