﻿using System;
using System.Threading.Tasks;
using ContactsGateway.Exceptions;
using ContactsGateway.Models.Contacts;
using ContactsGateway.Services.Fetchers;
using Microsoft.AspNetCore.Mvc;

namespace ContactsGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : Controller
    {
        private readonly GitHubFetcher _fetcher;
        
        public GitHubController(GitHubFetcher fetcher)
        {
            _fetcher = fetcher;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(ulong id)
        {
            try
            {
                return Json(await _fetcher.FetchAsync(id));
            }
            catch (ContactNotFoundException<GitHubContact> _)
            {
                return NotFound();
            }
        }
    }
}
