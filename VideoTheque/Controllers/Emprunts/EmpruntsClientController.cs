﻿using Microsoft.AspNetCore.Mvc;
using VideoTheque.Businesses.Emprunts;
using VideoTheque.Businesses.Genres;
using VideoTheque.DTOs;
using VideoTheque.ViewModels;

namespace VideoTheque.Controllers.Emprunts
{
    [ApiController]
    [Route("emprunts/client")]
    public class EmpruntsClientController : ControllerBase
    {
        private readonly IEmpruntsClientBusiness _empruntsClientBusiness;
        protected readonly ILogger<EmpruntsClientController> _logger;
        

        public EmpruntsClientController(ILogger<EmpruntsClientController> logger, IEmpruntsClientBusiness empruntsClientBusiness)
        {
            _logger = logger;
            _empruntsClientBusiness = empruntsClientBusiness;
        }

        [HttpGet("{idHost}")]
        public Task<List<EmpruntableViewModel>> GetFilmsEmpruntablesFromHost([FromRoute] int idHost)
        {
            return _empruntsClientBusiness.GetFilmsEmpruntablesFromHost(idHost);
        }

        [HttpPost("{idHost}/{idFilm}")]
        public void GetEmprunt([FromRoute] int idHost, [FromRoute] int idFilm)
        {
            _empruntsClientBusiness.EmpruntFilm(idHost, idFilm);
        }
    }
}