using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BT.APPLICATION;
using BT.APPLICATION.RequestResponse;
using MediatR;


namespace BT.API.Configurations
{
    public class Mediator
    {
        internal static void RegisterMediatr(WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(typeof(RecordNotFoundException).Assembly);
        }

        internal static void RegisterAutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(
                configAction =>
                {
                    configAction.ValidateInlineMaps = false;
                },
                typeof(Response)
            );
        }
    }
}