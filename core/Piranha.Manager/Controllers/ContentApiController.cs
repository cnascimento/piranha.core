/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * https://github.com/piranhacms/piranha.core
 *
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piranha.Manager.Models;
using Piranha.Manager.Services;

namespace Piranha.Manager.Controllers
{
    /// <summary>
    /// Api controller for content management.
    /// </summary>
    [Area("Manager")]
    [Route("manager/api/content")]
    [Authorize(Policy = Permission.Admin)]
    [ApiController]
    public class ContentApiController : Controller
    {
        private readonly ContentService _content;
        private readonly ContentTypeService _contentType;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContentApiController(ContentService content, ContentTypeService contentType)
        {
            _content = content;
            _contentType = contentType;
        }

        /// <summary>
        /// Gets the currently available block types.
        /// </summary>
        /// <param name="parentType">The optional parent group type</param>
        /// <returns>The block list model</returns>
        [Route("blocktypes/{parentType?}")]
        [HttpGet]
        public BlockListModel GetBlockTypes(string parentType = null)
        {
            return _contentType.GetBlockTypes(parentType);
        }

        /// <summary>
        /// Creates a new block of the specified type.
        /// </summary>
        /// <param name="type">The block type</param>
        /// <returns>The new block</returns>
        [Route("block/{type}")]
        [HttpGet]
        public async Task<IActionResult> CreateBlockAsync(string type)
        {
            var block = await _contentType.CreateBlockAsync(type);

            if (block != null)
            {
                return Ok(block);
            }
            return NotFound();
        }

        /// <summary>
        /// Creates a new region for the specified content type.
        /// </summary>
        /// <param name="content">The type of content</param>
        /// <param name="type">The content type</param>
        /// <param name="region">The region id</param>
        /// <returns>The new region model</returns>
        [Route("region/{content}/{type}/{region}")]
        [HttpGet]
        public async Task<IActionResult> CreateRegionAsync(string content, string type, string region)
        {
            if (content == "page")
            {
                return Ok(await _contentType.CreatePageRegionAsync(type, region));
            }
            else if (content == "post")
            {
                return Ok(await _contentType.CreatePostRegionAsync(type, region));
            }
            else if (content == "site")
            {
                return Ok(await _contentType.CreateSiteRegionAsync(type, region));
            }
            return NotFound();
        }

        [Route("{contentGroup}/list")]
        [HttpGet]
        [Authorize(Policy = Permission.Content)]
        public async Task<IActionResult> List(string contentGroup)
        {
            var model = await _content.GetListAsync(contentGroup);

            return Ok(model);
        }

        /// <summary>
        /// Gets the post with the given id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The post edit model</returns>
        [Route("{id:Guid}")]
        [HttpGet]
        [Authorize(Policy = Permission.Content)]
        public async Task<ContentEditModel> Get(Guid id)
        {
           return await _content.GetByIdAsync(id);
        } 

        /// <summary>
        ///
        /// </summary>
        /// <param name="contentType">The content type</param>
        /// <returns>The edit model</returns>
        [Route("create/{contentType}")]
        [HttpGet]
        [Authorize(Policy = Permission.ContentAdd)]
        public async Task<ContentEditModel> Create(string contentType)
        {
           return await _content.CreateAsync(contentType);
        }

        /// <summary>
        /// Saves the given model
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>The result of the operation</returns>
        [Route("save")]
        [HttpPost]
        [Authorize(Policy = Permission.ContentSave)]
        public async Task<ContentEditModel> Save(ContentEditModel model)
        {
             try
            {
                await _content.SaveAsync(model);
            }
            catch (ValidationException e)
            {
                model.Status = new StatusMessage
                {
                    Type = StatusMessage.Error,
                    Body = e.Message
                };

                return model;
            }

            var ret = await _content.GetByIdAsync(model.Id);
            ret.Status = new StatusMessage
            {
                Type = StatusMessage.Success,
                Body = "The content was successfully saved"
            };

            return ret;
        }

        /// <summary>
        /// Deletes the content with the given id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The result of the operation</returns>
        [Route("delete/{id}")]
        [HttpGet]
        [Authorize(Policy = Permission.ContentDelete)]
        public async Task<StatusMessage> Delete(Guid id)
        {
            try
            {
                await _content.DeleteAsync(id);
            }
            catch (ValidationException e)
            {
                // Validation did not succeed
                return new StatusMessage
                {
                    Type = StatusMessage.Error,
                    Body = e.Message
                };
            }
            catch
            {
                return new StatusMessage
                {
                    Type = StatusMessage.Error,
                    Body = "An error occured while deleting the content"
                };
            }

            return new StatusMessage
            {
                Type = StatusMessage.Success,
                Body = "The content was successfully deleted"
            };
        }
    }
}