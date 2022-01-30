// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Lalena.Application.Read;
// using Lalena.Application.Write;
// using Lalena.Application.Write.Resultaten;
// using Lalena.Database;
// using Lalena.Database.Repositories;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace Lalena.Api.Controllers
// {
//     [ApiController]
//     [Route("[controller]")]
//     public class ResultatenController : ControllerBase
//     {
//         private readonly MaaltafelsDbContext _dbContext;
//
//         public ResultatenController()
//         {
//             var options = new DbContextOptionsBuilder<MaaltafelsDbContext>()
//                 .UseSqlServer("Server=.\\SQL2019;Database=Maaltafels;Trusted_Connection=True;")
//                 .Options;
// ;
//             _dbContext = new MaaltafelsDbContext(options);
//         }
//         
//         [HttpPost]
//         [ProducesResponseType(StatusCodes.Status201Created)]
//         public async Task SaveResultaat(SaveResultaat.SaveResultaatRequest request, CancellationToken cancellationToken) => 
//             await new SaveResultaat.Handler(new ResultatenRepository(_dbContext)).Handle(request, cancellationToken);
//
//         [HttpGet]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         public async Task<GetResultaten.Response> GetResultaten(CancellationToken cancellationToken) =>
//             await new GetResultaten.Handler(_dbContext).Handle(cancellationToken);
//
//         [HttpGet("{id:guid}")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         public async Task<GetResultaat.GetResultaatResponse> GetResultaat(Guid id, CancellationToken cancellationToken) => 
//             await new GetResultaat.Handler(_dbContext).Handle(id, cancellationToken);
//     }
// }