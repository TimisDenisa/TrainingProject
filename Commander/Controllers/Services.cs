using Commander.Data;
using Commander.Models;
using System.Collections.Generic;

namespace Commander.Controllers
{
    public class Services : IServices
    {
        private readonly ICommanderRepo _repository;

        public Services(ICommanderRepo repository)
        {
            _repository = repository;
        }
        public void CreateCommand(Command cmd)
        {
            _repository.CreateCommand(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            _repository.DeleteCommand(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _repository.GetAllCommands();
        }

        public Command GetCommandById(int id)
        {
            return _repository.GetCommandById(id);
        }

        public bool SaveChanges()
        {
            return _repository.SaveChanges();
        }

        public void UpdateCommand(Command cmd)
        {
            _repository.UpdateCommand(cmd);
        }
    }
}