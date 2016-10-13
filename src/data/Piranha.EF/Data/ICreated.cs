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

namespace Piranha.EF.Data
{
    /// <summary>
    /// Interface for data models tracking created date.
    /// </summary>
    public interface ICreated
    {
        /// <summary>
        /// Gets/sets the created date.
        /// </summary>
        DateTime Created { get; set; }
    }
}
