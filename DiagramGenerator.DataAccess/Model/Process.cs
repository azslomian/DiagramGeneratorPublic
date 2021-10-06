using DiagramGenerator.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DiagramGenerator.DataAccess.Model
{
    [Index(nameof(Id))]
    public class Process : Entity
    {
        public Guid DiagramId { get; set; }

        public string Purpose { get; set; }

        [ForeignKey("DiagramId")]
        public Diagram Diagram { get; set; }

        public ICollection<Operation> Operations => operations;

        private IList<Operation> operations;

        public void AddOperation(Operation operation)
        {
            if (operations.Any(x => x.Id == operation.Id))
            {
                throw new InvalidOperationException($"Operation with Id: {operation.Id} exists in this Process");
            }

            operations.Add(operation);
        }

        public void RemoveOperation(Operation operation)
        {
            if (!operations.Any(x => x.Id == operation.Id))
            {
                throw new InvalidOperationException($"Operation with Id: {operation.Id} does not exist in this Process");
            }

            operations.Remove(operation);
        }

        public void UpdateOperation(Operation operation)
        {
            var existingOperation = operations.Where(x => x.Id == operation.Id).FirstOrDefault();
            if (existingOperation == null)
            {
                throw new InvalidOperationException($"Operation with Id: {operation.Id} does not exist in this Process");
            }

            existingOperation = operation;
        }
    }
}
