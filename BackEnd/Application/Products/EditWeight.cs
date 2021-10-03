using Application.Errors;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class EditWeight
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(p => p.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext context;
            
            public Handler(DataContext context) 
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var productToUpdate = await context.Products.FindAsync(request.Id);
                if(productToUpdate == null)
                {
                    throw new RestException(HttpStatusCode.BadRequest, "Invalid product!");
                }
                productToUpdate.SearchWeight++;
                bool success = await context.SaveChangesAsync() > 0;
                if (success)
                {
                    return Unit.Value;
                }
                throw new Exception("Product weight was not updated!");
            }
        }
    }
}
