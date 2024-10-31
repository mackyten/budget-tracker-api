

using BT.APPLICATION.RequestResponse;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using AutoMapper;
using BT.APPLICATION;

namespace BT.API.Configurations
{
    public static class Mediator
    {
        internal static void RegisterMediatr(WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(typeof(RecordNotFoundException).Assembly);
        }

        //https://docs.fluentvalidation.net/en/latest/built-in-validators.html
        internal static void AddFluentValidation(WebApplicationBuilder builder)
        {
            builder.Services.AddFluentValidationAutoValidation();
            //builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<RecordNotFoundException>();
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
