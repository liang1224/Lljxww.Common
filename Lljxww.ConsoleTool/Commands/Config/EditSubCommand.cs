﻿using Lljxww.ApiCaller.Models.Config;
using Lljxww.ConsoleTool.Utils;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Lljxww.ConsoleTool.Commands.Config;

[Command("edit", Description = "编辑配置文件")]
internal class EditSubCommand
{
    [Required(ErrorMessage = "请输入要编辑的配置文件的标签")]
    [Argument(0, Description = "要编辑的配置文件的标签")]
    public string Tag { get; }

    private int OnExecute(IConsole console)
    {
        ActionResult<(string, string)> actionResult = SystemManager.GetCallerConfig(Tag);
        if (!actionResult.Success)
        {
            console.Error(actionResult.Message);
            return -1;
        }

        (string path, string fileContent) = actionResult.Content;
        try
        {
            ApiCallerConfig? apiCallerConfig = JsonSerializer.Deserialize<ApiCallerConfig>(fileContent);

            if (apiCallerConfig == null)
            {
                _ = console.WriteLine("当前配置文件为空");
                return 1;
            }

            // ServiceItem
            ActionResult<ServiceItem> serviceItemResult = ConfigEditor.NodeSelecter(console, apiCallerConfig.ServiceItems);
            if (!serviceItemResult.Success)
            {
                return -1;
            }

            // ApiItem
            ActionResult<ApiItem> apiItemResult = ConfigEditor.NodeSelecter(console, serviceItemResult.Content.ApiItems);
            if (!apiItemResult.Success)
            {
                return -1;
            }

            ApiItem current = apiItemResult.Content;

            // 处理节点编辑
            var editActionResult = ConfigEditor.Edit(console, current);

            if (!editActionResult.Success)
            {
                console.Error(editActionResult.Message);
                return -1;
            }

            apiCallerConfig = ConfigEditor.UpdateApiCallerConfig(apiCallerConfig, editActionResult.Content);

            // 保存修改的信息
            var udpateActionResult = SystemManager.SaveUpdatedCallerConfig(apiCallerConfig!, path);

            if (udpateActionResult.Success)
            {
                console.Success("更新成功");
                return 1;
            }
            else
            {
                console.Error(udpateActionResult.Message);
                return -1;
            }
        }
        catch (Exception ex)
        {
            console.Error($"配置解析失败: {ex.Message}");
            return -1;
        }
    }
}