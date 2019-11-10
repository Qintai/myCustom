using System;
using System.Net;
using System.Threading.Tasks;
using crud_entity;
using crud_server.Achieve;
using crud_server.connector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using crud_web;
using crud_web.Custom.Filter;

namespace crud_Web.crud_Controllers
{
    /// <summary>
    /// RestFul风格--Chloe.Mysql半自动ORM的增删改查
    /// </summary>
    [ApiController]
    [Route("[Controller]/{callback}")]
    [AllowAnonymous]
    [CostomProducesAttribute]
    public class PController : ControllerBase
    {
        [Route("M")]
        public string get()
        {
            return "222";
        }
    }
}


