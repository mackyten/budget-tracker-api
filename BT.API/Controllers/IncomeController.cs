using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BT.APPLICATION;
using BT.APPLICATION.BudgetTracker.Income.Models;
using BT.APPLICATION.RequestResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Commands = BT.APPLICATION.BudgetTracker.Income.Command;
using Queries = BT.APPLICATION.BudgetTracker.Income.Queries;

namespace BT.API.Controllers
{
    [ApiController]
    [Route("api/income")]
    public class IncomeController : BaseController
    {
        [HttpPost("test")]
        [Description("Creates new Income Data")]
        [ProducesResponseType(typeof(IncomeModel), (int)HttpStatusCode.Created)]
        public IActionResult Test()
        {


            return Ok("Connected");
        }


        [HttpPost("create")]
        [Description("Creates new Income Data")]
        [ProducesResponseType(typeof(IncomeModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] Commands.Create.Command command)
        {

            var result = await Mediator.Send(command);

            if (result is BadRequestResponse)
            {
                return BadRequest(result.Message);
            }

            var data = ((SuccessResponse<IncomeModel>)result).Data;
            return Ok(data);
        }

        [HttpPut("update")]
        [Description("Updates Income Data, Returns Icome Mode")]
        [ProducesResponseType(typeof(IncomeModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] Commands.Update.Command command)
        {
            var result = await Mediator.Send(command);

            if (result is BadRequestResponse)
            {
                return BadRequest(result.Message);
            }

            var data = ((SuccessResponse<IncomeModel>)result).Data;
            return Ok(data);
        }

        // [HttpDelete("delete")]
        // [Description("Returns temp test record id")]
        // [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        // public async Task<IActionResult> Delete([FromBody] Commands.Delete.Command command)
        // {
        //     var result = await Mediator.Send(command);

        //     if (result is BadRequestResponse)
        //     {
        //         return BadRequest(result.Message);
        //     }

        //     var data = ((SuccessResponse<int>)result).Data;
        //     return Ok(data);
        // }

        // [HttpGet("get-id")]
        // [Description("Query returns a TempTestRecordModel")]
        // [ProducesResponseType(typeof(TempTestRecordModel), (int)HttpStatusCode.Created)]
        // public async Task<IActionResult> GetById([FromQuery] Queries.GetById.Query query)
        // {
        //     var result = await Mediator.Send(query);
        //     if (result is BadRequestResponse)
        //     {
        //         return BadRequest(result.Message);
        //     }

        //     var data = ((SuccessResponse<TempTestRecordModel>)result).Data;
        //     return Ok(data);
        // }

        [HttpGet("get-list")]
        [Description("Query returns a List of Income Model")]
        [ProducesResponseType(typeof(PaginatedResponse<List<IncomeModel>>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> GetList([FromQuery] Queries.GetList.Query query)
        {
            var result = await Mediator.Send(query);
            if (result is BadRequestResponse)
            {
                return BadRequest(result.Message);
            }

            var data = ((SuccessResponse<PaginatedResponse<List<IncomeModel>>>)result).Data;
            return Ok(data);
        }
    }
}