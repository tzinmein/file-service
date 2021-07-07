// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2016-11-17
// 
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Mondol.WebPlatform
{
    /// <summary>
    /// IHostingEnvironment的扩展方法
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// 获取配置文件根目录
        /// </summary>
        public static string GetConfigPath(this IWebHostEnvironment env)
        {
            return Path.Combine(env.ContentRootPath, "confs");
        }
    }
}
