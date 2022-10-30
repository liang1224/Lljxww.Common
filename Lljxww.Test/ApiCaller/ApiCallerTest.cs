﻿using Lljxww.ApiCaller;
using Lljxww.ApiCaller.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test.ApiCaller;

[TestClass]
public class ApiCallerTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public ApiCallerTest()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("./ApiCaller/apicaller.json")
            .Build();

        _services.ConfigureCaller(config);

        CallerEvents.OnExecuted += context =>
        {
            string apiName = context.ApiName;
        };
    }

    [TestMethod]
    public void InvokeTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

        string username = "lljxww";

        ApiResult? result = caller.InvokeAsync("gh.GetUserInfo", new
        {
            username
        }, new RequestOption
        {
            CustomAuthorizeInfo = "123"
        }).Result;

        Assert.AreEqual(username, result!["login"]);
        Assert.AreEqual(username, result!["Login"]);
        Assert.AreEqual(username, result!["lOgin"]);
        Assert.AreEqual(username, result!["loGin"]);
        Assert.AreEqual(username, result!["logIn"]);
        Assert.AreEqual(username, result!["logiN"]);
    }

    [TestMethod]
    public void CallerSettingTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

        // LogDetail
        string username = "lljxww";

        int i = 0;
        while (i < 10)
        {
            ApiResult? result = caller.InvokeAsync("gh.GetUserInfo2", new
            {
                username
            }).Result;

            i++;
        }
    }
}