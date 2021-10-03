using Application.DTO;
using Application.Errors;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class List
    {
        public class Query : IRequest<List<ProductDTO>>
        {
            public string SearchedText { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ProductDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<ProductDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.SearchedText) || request.SearchedText.Length < 3 || request.SearchedText.Length > 50)
                    throw new RestException(HttpStatusCode.BadRequest, "The length of the searched text should be grather then 3 and less then 50 characters!");

                var prdTopWeight = _context
                                        .Products
                                        .OrderByDescending(p => p.SearchWeight)
                                        .Where(p => p.SearchWeight > 0)
                                        .Take(5);

                var prdTopStartWith = _context
                                        .Products
                                        .Where(p => p.Name.ToLower().StartsWith(request.SearchedText.ToLower()))
                                        .OrderBy(p => p.Name)
                                        .Except(prdTopWeight)
                                        .Take(5);

                var prdTopContainsWith = _context
                                            .Products
                                            .Where(p => !p.Name.ToLower().StartsWith(request.SearchedText.ToLower()) && p.Name.ToLower().Contains(request.SearchedText.ToLower()))
                                            .OrderBy(p => p.Name)
                                            .Except(prdTopWeight)
                                            .Take(20);
                
                var products = await prdTopWeight.Concat(prdTopStartWith).Concat(prdTopContainsWith).ToListAsync();
                return _mapper.Map<List<ProductDTO>>(products);
            }
        }
    }
}
