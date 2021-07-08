// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2016-11-17
// ---------------------------------------------
// Refactored by alan.yu @ 2021-07-08
// 

using Microsoft.Extensions.Options;
using Mondol.FileService.Authorization.Options;
using Mondol.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mondol.FileService.Authorization.Codecs.Impls
{
    public class FileTokenCodec : IFileTokenCodec
    {
        private readonly byte[] _appSecretBytes;
        private readonly IUrlDataCodec _urlDataCodec;

        public byte CurrentVersion => 2;

        public FileTokenCodec(IOptions<AuthOption> tokenOpt, IUrlDataCodec urlDataCodec)
        {
            _appSecretBytes = tokenOpt.Value.GetAppSecretBytes();
            _urlDataCodec = urlDataCodec;
        }

        public string Encode(FileToken token)
        {
            var pseudoIdBys = NetBitConverter.GetBytes(token.PseudoId);
            var fileIdBys = NetBitConverter.GetBytes(token.FileId);
            var ownerIdBys = NetBitConverter.GetBytes(token.FileOwnerId);
            var mimeBys = NetBitConverter.GetBytes(token.MimeId);
            var expireTimeBys = IFileTokenCodec.Datetime2Bytes(token.ExpireTime);
            var fileCreateTimeBys = IFileTokenCodec.Datetime2Bytes(token.FileCreateTime);

            var lstLen = 1 + pseudoIdBys.Length + fileIdBys.Length + ownerIdBys.Length + mimeBys.Length + expireTimeBys.Length + fileCreateTimeBys.Length;
            var mdatLst = new List<byte>(lstLen)
            {
                CurrentVersion
            };
            mdatLst.AddRange(pseudoIdBys);
            mdatLst.AddRange(fileIdBys);
            mdatLst.AddRange(ownerIdBys);
            mdatLst.AddRange(mimeBys);
            mdatLst.AddRange(expireTimeBys);
            mdatLst.AddRange(fileCreateTimeBys);

            var mdatBys = mdatLst.ToArray();

            //Sign
            var signBys = ArrayUtil.Addition(_appSecretBytes, mdatBys);
            var hashBys = IFileTokenCodec.CalcHash(signBys);

            //Encode to a string
            var encBys = ArrayUtil.Addition(hashBys, mdatBys);
            return _urlDataCodec.Encode(encBys);
        }

        public FileToken Decode(string tokenStr)
        {
            var encBys = _urlDataCodec.Decode(tokenStr);

            //Verify signature
            var mdatBys = new byte[encBys.Length - IFileTokenCodec.HashLen];
            Array.Copy(encBys, IFileTokenCodec.HashLen, mdatBys, 0, mdatBys.Length);
            var signBys = ArrayUtil.Addition(_appSecretBytes, mdatBys);
            var hashBys = IFileTokenCodec.CalcHash(signBys);
            if (!ArrayUtil.Equals(hashBys, 0, encBys, 0, IFileTokenCodec.HashLen))
            {
                throw new InvalidDataException("bad sign");
            }

            if (mdatBys[0] != CurrentVersion)
            {
                throw new NotSupportedException("bad token version");
            }

            //Resolve to object
            var index = 1; //Ignore version
            var pseudoId = NetBitConverter.ToUInt32(mdatBys, index);
            index += 4;
            var fileId = NetBitConverter.ToInt32(mdatBys, index);
            index += 4;
            var ownerId = NetBitConverter.ToInt32(mdatBys, index);
            index += 4;
            var mimeId = NetBitConverter.ToUInt32(mdatBys, index);
            index += 4;
            var expireTime = IFileTokenCodec.Bytes2DateTime(mdatBys, index);
            index += sizeof(long);
            var fileCreateTime = IFileTokenCodec.Bytes2DateTime(mdatBys, index);

            return new FileToken
            {
                PseudoId = pseudoId,
                FileId = fileId,
                FileOwnerId = ownerId,
                MimeId = mimeId,
                ExpireTime = expireTime,
                FileCreateTime = fileCreateTime
            };
        }
    }
}
