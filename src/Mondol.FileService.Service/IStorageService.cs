// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2016-11-17
// 
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mondol.FileService.Db.Entities;
using Mondol.FileService.Service.Models;

namespace Mondol.FileService.Service
{
    /// <summary>
    /// 本地存储服务
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// 为指定用户创建文件
        /// </summary>
        Task<FileStorageInfo> CreateFileAsync(FileOwnerTypeId ownerTypeId, string hash, Stream file, string fileName, int periodMinute = 0);

        Task<FileStorageInfo> CreateFileAsync(FileOwnerTypeId ownerTypeId, string hash, IFormFile file, string fileName, int periodMinute = 0);

        uint GeneratePseudoId(string data);
        string GetFileDirectoryPath(uint pseudoId, DateTime fileCreateTime, int fileId);

        /// <summary>
        /// 获取文件真实的路径
        /// </summary>
        string GetRawFilePath(uint pseudoId, DateTime fileCreateTime, int fileId);

        bool RawFileExists(uint pseudoId, DateTime fileCreateTime, int fileId);
        Task DeleteFileAsync(uint pseudoId, DateTime fileCreateTime, int fileId);

        /// <summary>
        /// 移动文件到指定路径
        /// </summary>
        Task MoveToPathAsync(string srcFilePath, string destFilePath, bool overrideDest);
    }
}
