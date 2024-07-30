﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highgeek.McWebApp.Common.Helpers
{
    public class ConfigProvider
    {
        public static readonly ConfigProvider Instance = new ConfigProvider();

        public ConfigurationManager configurationManager;

        public ConfigurationManager GetConfigurationManager()
        {
            if (configurationManager == null)
            {
                configurationManager = new ConfigurationManager();
            }

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                configurationManager.SetBasePath("/appsettings/").AddJsonFile("appsettings.json").AddEnvironmentVariables();
            }
            else
            {
                configurationManager.SetBasePath("/app/").AddJsonFile("appsettings.json").AddEnvironmentVariables();
            }
            return configurationManager;
        }

        public string GetConfigString(string key)
        {
            return GetConfigurationManager()[key] ?? throw new InvalidOperationException("Configuration key '" + key + "' not found.");
        }

        public string GetConnectionString(string connectionstring)
        {
            return GetConfigurationManager().GetConnectionString(connectionstring) ?? throw new InvalidOperationException("Connection string '"+ connectionstring + "' not found.");
        }


    }
}