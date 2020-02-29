﻿namespace PiControlPanel.Api.GraphQL.Types.Output
{
    using global::GraphQL.Relay.Types;
    using global::GraphQL.Types;
    using NLog;
    using PiControlPanel.Api.GraphQL.Extensions;
    using PiControlPanel.Domain.Contracts.Application;
    using PiControlPanel.Domain.Models.Hardware.Memory;

    public class MemoryType : ObjectGraphType<Memory>
    {
        public MemoryType(IMemoryService memoryService, ILogger logger)
        {
            Field(x => x.Total);

            Field<MemoryStatusType>()
                .Name("Status")
                .ResolveAsync(async context =>
                {
                    logger.Info("Memory status field");
                    GraphQLUserContext graphQLUserContext = context.UserContext as GraphQLUserContext;
                    var businessContext = graphQLUserContext.GetBusinessContext();

                    return await memoryService.GetLastStatusAsync();
                });

            Connection<MemoryStatusType>()
                .Name("Statuses")
                .Bidirectional()
                .ResolveAsync(async context =>
                {
                    logger.Info("Memory statuses connection");
                    GraphQLUserContext graphQLUserContext = context.UserContext as GraphQLUserContext;
                    var businessContext = graphQLUserContext.GetBusinessContext();

                    var statuses = await memoryService.GetStatusesAsync();

                    return ConnectionUtils.ToConnection(statuses, context);
                });
        }
    }
}
