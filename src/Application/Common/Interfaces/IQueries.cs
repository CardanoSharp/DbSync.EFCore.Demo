namespace Application.Common.Interfaces
{
    public interface IQueries
    {
        int GetBlockInformation(int slotNumber);

        int GetTransactionsPerEpoch(int epoch); 
    }
}