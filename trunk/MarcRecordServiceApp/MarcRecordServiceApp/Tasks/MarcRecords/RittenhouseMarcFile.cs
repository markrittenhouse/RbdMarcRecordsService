﻿using MarcRecordServiceApp.Core.DataAccess.Entities;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
    public class RittenhouseMarcFile : MarcFileData, IMarcFile
    {
        private readonly MarcRecordProvider _marcRecordProvider;

        public RittenhouseMarcFile(Product product)
            : base(product)
        {
            _marcRecordProvider = MarcRecordProvider.Rbd;
        }

        public MarcRecordProvider RecordProvider
        {
            get { return _marcRecordProvider; }
        }
    }
}

