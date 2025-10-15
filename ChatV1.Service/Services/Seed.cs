using ChatV1.DataAccess.Models;
using ChatV1.DataAccess.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services
{
    public interface ISeed
    {
        void SeedAll();
    }

    public class Seed : ISeed
    {
        private IChatV1Repository<ActionType> _actionType;
        ILogger<Seed> _logger;

        public Seed(IChatV1Repository<ActionType> actionType, ILogger<Seed> logger)
        {
            _actionType = actionType;
            _logger = logger;
        }

        public void SeedAll()
        {
            SeedAcionType();
        }

        public void SeedAcionType()
        {
            try
            {
                List<string> acionTypes = ["SEEN_MESSAGE", "DELETE_MESSAGE", "DELETE_MESSAGE_ROOM"];
                foreach (var i in acionTypes)
                {
                    var foundActionType = _actionType.Find(x => x.Name == i).FirstOrDefault();
                    if (foundActionType == null)
                    {
                        _actionType.Add(new ActionType { Name = i });
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
        }

    }
}
