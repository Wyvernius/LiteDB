using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDB
{
    public partial class LiteDatabase
    {
        public delegate void UpdateHandler(string collectionName, object id);

        private Dictionary<object, List<UpdateHandler>> _delegates = new Dictionary<object, List<UpdateHandler>>();

        /// <summary>
        /// Add a notification for a specific collection
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        /// <param name="handler">Delegate handler to call on Update/Insert</param>
        public void RegisterCollectionNotification(string collectionName, UpdateHandler handler)
        {
            if (!_delegates.ContainsKey(collectionName))
            {
                _delegates.Add(collectionName, new List<UpdateHandler>());
            }
            if (handler != null)
            {
                _delegates[collectionName].Add(handler);
            }
        }

        /// <summary>
        /// De register all notifications
        /// </summary>
        public void DeRegisterAllNotifications()
        {
            _delegates = new Dictionary<object, List<UpdateHandler>>();
        }

        /// <summary>
        /// De register all notifications for a collection
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        public void DeRegisterCollectionNotifications(string collectionName)
        {
            if (_delegates.ContainsKey(collectionName))
            {
                _delegates.Remove(collectionName);
            }
        }

        /// <summary>
        /// De register a specific call notification for collection
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="handler"></param>
        public void DeRegisterCollectionNotification(string collectionName, UpdateHandler handler)
        {

            if (_delegates.ContainsKey(collectionName))
            {
                //Make a copy so we can update the main list
                var handlers = new List<UpdateHandler>(_delegates[collectionName]);
                foreach (UpdateHandler lphandler in handlers)
                {
                    if (lphandler == handler)
                    {
                        _delegates[collectionName].Remove(lphandler);
                    }
                }
                //If we don't have any remaining for this collection, remove the collection from the dictionary
                if (_delegates[collectionName].Count == 0)
                {
                    _delegates.Remove(collectionName);
                }

            }
            if (handler != null)
            {
                _delegates[collectionName].Add(handler);
            }
        }

        /// <summary>
        /// Add a notification for a specific collection
        /// </summary>
        /// <param name="id">Id of the document</param>
        /// <param name="handler">Delegate handler to call on Update/Insert</param>
        public void RegisterDocumentNotification(object id, UpdateHandler handler)
        {

            if (!_delegates.ContainsKey(id))
            {
                _delegates.Add(id, new List<UpdateHandler>());
            }
            if (handler != null)
            {
                _delegates[id].Add(handler);
            }
        }

        /// <summary>
        /// De register all notifications for a collection
        /// </summary>
        /// <param name="id">Id of the document</param>
        public void DeRegisterDocumentNotifications(object id)
        {
            if (_delegates.ContainsKey(id))
            {
                _delegates.Remove(id);
            }
        }

        /// <summary>
        /// De register a specific call notification for collection
        /// </summary>
        /// <param name="id">Id of the document</param>
        /// <param name="handler"></param>
        public void DeRegisterDocumentNotification(object id, UpdateHandler handler)
        {

            if (_delegates.ContainsKey(id))
            {
                //Make a copy so we can update the main list
                var handlers = new List<UpdateHandler>(_delegates[id]);
                foreach (UpdateHandler lphandler in handlers)
                {
                    if (lphandler == handler)
                    {
                        _delegates[id].Remove(lphandler);
                    }
                }
                //If we don't have any remaining for this collection, remove the collection from the dictionary
                if (_delegates[id].Count == 0)
                {
                    _delegates.Remove(id);
                }

            }
            if (handler != null)
            {
                _delegates[id].Add(handler);
            }
        }

        /// <summary>
        /// Call any notification handlers registered for this collection
        /// </summary>
        /// <param name="collectionName">Name of the collection to check</param>
        /// <param name="id">Id of document to check</param>
        private void CheckNotification(string collectionName, BsonValue id)
        {
            if (_delegates.ContainsKey(collectionName))
            {
                foreach (UpdateHandler handler in _delegates[collectionName])
                {
                    handler(collectionName, id);
                }
            }

            foreach (var del in _delegates)
            {
                object realType = id.RawValue;
                if (del.Key.Equals(realType))
                    foreach (UpdateHandler handler in del.Value)
                    {
                        handler(collectionName, realType);
                    }
            }
        }
    }
}