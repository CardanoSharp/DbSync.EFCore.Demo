using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Queries
{

    public record GetBlockBySlotNumber(DateTime DateTime, short Epoch, int Block, int Confirmations, double TransactionID, List<string> Addresses, double TotalOutput, double TransactionFee) : IRequest<GetBlockBySlotNumber>;

}
