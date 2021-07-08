// Copyright (c) Mondol. All rights reserved.
// 
// Author:  frank
// Email:   frank@mondol.info
// Created: 2017-01-23
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
    /// <summary>
    /// OwnerToken codec
    /// </summary>
    internal class OwnerTokenCodec : IOwnerTokenCodec
    {
        private readonly byte[] _appSecretBytes;
        readonly IUrlDataCodec _urlDataCodec;

        public byte CurrentVersion => 2;

        public OwnerTokenCodec(IOptions<AuthOption> tokenOpt, IUrlDataCodec urlDataCodec)
        {
            _appSecretBytes = tokenOpt.Value.GetAppSecretBytes();
            _urlDataCodec = urlDataCodec;
        }

        public string Encode(OwnerToken token)
        {
            var ownerTypeBys = NetBitConverter.GetBytes(token.OwnerType);
            var ownerIdBys = NetBitConverter.GetBytes(token.OwnerId);
            var expireTimeBys = IOwnerTokenCodec.Datetime2Bytes(token.ExpireTime);

            var lstLen = 1 + ownerTypeBys.Length + ownerIdBys.Length + expireTimeBys.Length;
            var mdatLst = new List<byte>(lstLen)
            {
                CurrentVersion
            };
            mdatLst.AddRange(ownerTypeBys);
            mdatLst.AddRange(ownerIdBys);
            mdatLst.AddRange(expireTimeBys);
            var mdatBys = mdatLst.ToArray();

            //Sign
            var signBys = ArrayUtil.Addition(_appSecretBytes, mdatBys);
            var hashBys = IOwnerTokenCodec.CalcHash(signBys);

            //Encode to a string
            var encBys = ArrayUtil.Addition(hashBys, mdatBys);
            return _urlDataCodec.Encode(encBys);
        }

        public OwnerToken Decode(string tokenStr)
        {
            var encBys = _urlDataCodec.Decode(tokenStr);

            //Verify signature
            var mdatBys = new byte[encBys.Length - IOwnerTokenCodec.HashLen];
            Array.Copy(encBys, IOwnerTokenCodec.HashLen, mdatBys, 0, mdatBys.Length);
            var signBys = ArrayUtil.Addition(_appSecretBytes, mdatBys);
            var hashBys = IOwnerTokenCodec.CalcHash(signBys);
            if (!ArrayUtil.Equals(hashBys, 0, encBys, 0, IOwnerTokenCodec.HashLen))
            {
                throw new InvalidDataException("bad sign");
            }

            if (mdatBys[0] != CurrentVersion)
            {
                throw new NotSupportedException("bad token version");
            }

            //Resolve to object
            var index = 1; //Ignore version
            var ownerType = NetBitConverter.ToInt32(mdatBys, index);
            index += 4;
            var ownerId = NetBitConverter.ToInt32(mdatBys, index);
            index += 4;
            var expireTime = IOwnerTokenCodec.Bytes2DateTime(mdatBys, index);

            return new OwnerToken
            {
                OwnerType = ownerType,
                OwnerId = ownerId,
                ExpireTime = expireTime
            };
        }
    }
}
