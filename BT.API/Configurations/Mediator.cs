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
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Mediator>>();
            logger.LogInformation("Registering Mediatr...");
            builder.Services.AddMediatR(typeof(RecordNotFoundException).Assembly);
            logger.LogInformation("Mediatr Registered.");
        }

        internal static void RegisterAutoMapper(WebApplicationBuilder builder)
        {
            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Mediator>>();

            try
            {
                logger.LogInformation("Registering Automapper...");
                builder.Services.AddAutoMapper(
                    configAction =>
                    {
                        configAction.ValidateInlineMaps = false;
                    },
                    typeof(Response)
                );

                logger.LogInformation("Automapper Registered.");

            }
            catch (Exception e)
            {
                logger.LogInformation($"Error Occured at RegisterAutoMapper : {e.GetBaseException().Message}");
                throw new Exception($"Error Occured at RegisterAutoMapper : {e.GetBaseException().Message}");
            }
        }
    }
}