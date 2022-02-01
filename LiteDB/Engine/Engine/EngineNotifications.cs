using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB.Engine
{
    public partial class LiteEngine
    {
        public delegate void LiteEngineNotification(string collectionName, BsonValue id);
        public LiteEngineNotification EngineNotification;
        private void CheckNotification(string collectionName, BsonValue id)
        {
            if (EngineNotification == null)
                return;

            EngineNotification(collectionName, id);
        }
    }
}
