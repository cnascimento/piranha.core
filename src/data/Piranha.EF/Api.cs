﻿/*
 * Copyright (c) 2016 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;
using Piranha.Repositories;

namespace Piranha.EF
{
    public class Api : IApi
    {
        #region Members
        /// <summary>
        /// The private db context.
        /// </summary>
        private readonly Db db;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the archive repository.
        /// </summary>
        public IArchiveRepository Archives { get; private set; }

        /// <summary>
        /// Gets the category repository.
        /// </summary>
        public ICategoryRepository Categories { get; private set; }

        /// <summary>
        /// Gets the page repository.
        /// </summary>
        public IPageRepository Pages { get; private set; }

        /// <summary>
        /// Gets the page type repository.
        /// </summary>
        public IPageTypeRepository PageTypes { get; private set; }

        /// <summary>
        /// Gets the post repository.
        /// </summary>
        public IPostRepository Posts { get; private set; }
        #endregion

        public Api(Db db) {
            this.db = db;

            Archives = new Repositories.ArchiveRepository(db);
            Categories = new Repositories.CategoryRepository(db);
            PageTypes = new Repositories.PageTypeRepository(db);
            Posts = new Repositories.PostRepository(db);
        }

        /// <summary>
        /// Disposes the Api.
        /// </summary>
        public void Dispose() {
            db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
