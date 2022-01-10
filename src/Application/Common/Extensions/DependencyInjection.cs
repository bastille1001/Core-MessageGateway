using System;
using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Services;
using FluentValidation;
using Kibrit.Common.Kafka.Interfaces;
using Kibrit.Common.Kafka.Options;
using Kibrit.Common.Kafka.Producers;
using Kibrit.Smpp.Options;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class DependencyInjection
    {
        public static IConfigurationBuilder AddApplicationConfigurationFile(this IConfigurationBuilder builder)
        {
            var path = Environment.GetEnvironmentVariable("CONFIG_PATH");
            builder.AddJsonFile(path);

            return builder;
        }
        
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddKafka(configuration);
            services.AddMessageBroker(configuration);
            
            return services;
        }
        
        private static IConfigurationRoot GetConfigurationFromEnvironment(string envVariable)
        {
            var configPath = Environment.GetEnvironmentVariable(envVariable);
            var configValue = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();
            return configValue;
        }
        
        private static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaOptions>(configuration.GetSection(nameof(KafkaOptions)));
            services.AddSingleton<IKafkaProducer<string, string>, KafkaProducer<string, string>>();
        }

        private static void AddMessageBroker(this IServiceCollection services,
            IConfiguration configuration)
        {
            var rabbitCreds = GetConfigurationFromEnvironment("RABBIT_CREDS_PATH")
                .Get<RabbitMqCredentials>();
            var rabbitOptions = configuration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>();
            
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((_, cfg) =>
                {
                    cfg.Host(rabbitOptions.Host, rabbitOptions.VHost, h =>
                    {
                        h.Username(rabbitCreds.Username);
                        h.Password(rabbitCreds.Password);
                    });
                });
            });
        }
    }
}