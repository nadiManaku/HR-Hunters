﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HRHunters.Common.Enums;
using HRHunters.Common.Interfaces;
using HRHunters.Common.Requests.Users;
using HRHunters.Common.Responses;
using HRHunters.Common.Responses.AdminDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRHunters.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientManager _clientManager;
        public ClientsController(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }
        [Authorize(policy: "RequireAdminRole")]
        [HttpGet]
        public ActionResult<ClientResponse> GetMultipleClients(int pageSize = 0, int currentPage = 0, string sortedBy = "Id", SortDirection sortDir = SortDirection.ASC, string filterBy = "", string filterQuery = "")
        {
            return Ok(_clientManager.GetMultiple(pageSize, currentPage, sortedBy, sortDir, filterBy, filterQuery));
        }
        [Authorize(policy: "RequireClientRole")]
        [HttpPut]
        public async Task<ActionResult<GeneralResponse>> UpdateClientProfile(ClientUpdate clientUpdate)
        {
            return Ok(await _clientManager.UpdateClientProfile(clientUpdate));
        }
        [Authorize(policy: "RequireAdminRole")]
        [HttpPut("{id}")]
        public ActionResult<ClientInfo> UpdateClientStatus(int id, string status)
        {
            return Ok(_clientManager.UpdateClientStatus(id, status));
        }      
    }
}